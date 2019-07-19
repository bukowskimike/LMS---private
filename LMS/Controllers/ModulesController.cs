using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Models;
using LMS.Data;
using LMS.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LMS.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ModulesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var courseId = loggedInUser.CourseId;

            var applicationDbContext = _context.Module.Include(m => m.Course).Include(m => m.Documents);

            if (User.IsInRole("Elev"))
            {
                var model = applicationDbContext
                    .Where(m => m.CourseId == courseId 
                    || m.Documents.FirstOrDefault().Id == courseId);
                return View(await model.ToListAsync());
            }
            else if (User.IsInRole("Lärare") || User.IsInRole("Admin"))
            {
                return View(await applicationDbContext.ToListAsync());
            }
            return NotFound();
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Module
                .Include(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Admin, Lärare")]
        public async Task<IActionResult> Create(int? id)
        {
            var course = _context.Course.FirstOrDefault(c => c.Id == id);
            var model = new ModuleViewModel
            {
                CourseStart = course.StartDateTime,
                CourseEnd = course.EndDateTime,
                StartDateTime = course.StartDateTime,
                EndDateTime = course.EndDateTime,
                CourseId = course.Id
            };
            
            ViewBag.Course = await _context.Course
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToListAsync();
            return View(model);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Lärare")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDateTime,EndDateTime,CourseId,CourseStart,CourseEnd")] ModuleViewModel model)
        {
            var courses = _context.Course;
            var course = courses.FirstOrDefault(c => c.Id == model.CourseId);
            var CourseStart = model.CourseStart;

            var module = new Module()
            {
                Name = model.Name,
                Description = model.Description,
                StartDateTime = model.StartDateTime,
                EndDateTime = model.EndDateTime,
                CourseId = model.CourseId,
            };
            if (ModelState.IsValid)
            {
                _context.Add(module);
                await _context.SaveChangesAsync();
                return RedirectToAction("MainIndex", "Courses");
            }
            ViewBag.Course = await _context.Course
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToListAsync();
            return View(model);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var module = await _context.Module.Include(c => c.Course).FirstOrDefaultAsync(m => m.Id == id);
            
            if (module == null)
            {
                return NotFound();
            }

            var model = new ModuleViewModel()
            {
                Name = module.Name,
                Description = module.Description,
                StartDateTime = module.StartDateTime,
                EndDateTime = module.EndDateTime,
                CourseId = module.CourseId,
                CourseStart = module.Course.StartDateTime,
                CourseEnd = module.Course.EndDateTime,

        };

            ViewBag.Course = await _context.Course
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToListAsync();
            return View(model);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDateTime,EndDateTime,CourseId,CourseStart,CourseEnd")] ModuleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            var module = await _context.Module.FirstOrDefaultAsync(m => m.Id == id);

            if (ModelState.IsValid)
            {
                module.Name = model.Name;
                module.Description = model.Description;
                module.StartDateTime = model.StartDateTime;
                module.EndDateTime = model.EndDateTime;
                module.CourseId = model.CourseId;
                

                try
                {
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MainIndex", "Courses");
            }
            ViewBag.Course = await _context.Course
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToListAsync();
            return View(model);
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @module = await _context.Module.FindAsync(id);
            _context.Module.Remove(@module);
            await _context.SaveChangesAsync();
            return RedirectToAction("MainIndex", "Courses");
        }

        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.Id == id);
        }
    }
}
