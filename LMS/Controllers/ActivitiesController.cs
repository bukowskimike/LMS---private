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
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivitiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var courseId = loggedInUser.CourseId;

            var applicationDbContext = _context.Activity.Include(a => a.ActivityType).Include(a => a.Module);

            if (User.IsInRole("Elev"))
            {
                var model = applicationDbContext.Where(a => a.Module.Id == courseId);
                return View(await model.ToListAsync());
            }
            else if (User.IsInRole("Lärare") || User.IsInRole("Admin"))
            {
                return View(await applicationDbContext.ToListAsync());
            }
            return NotFound();
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var activity = await _context.Activity
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        [Authorize(Roles = "Admin, Lärare")]
        public IActionResult Create(int? id)
        {
            var module = _context.Module.FirstOrDefault(m => m.Id == id);
            var model = new LmsViewModel
            {
                ModuleStart = module.StartDateTime,
                ModuleEnd = module.EndDateTime,
                StartDateTime = module.StartDateTime,
                EndDateTime = module.EndDateTime,
                ModuleId = module.Id
            };

            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Name");
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name");
            
            return View(model);
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Lärare")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDateTime,EndDateTime,ModuleId,ActivityTypeId,ModuleStart,ModuleEnd")] LmsViewModel viewModel)
        {
            var activity = new Activity
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                StartDateTime = viewModel.StartDateTime,
                EndDateTime = viewModel.EndDateTime,
                ModuleId = viewModel.ModuleId,
                ActivityTypeId = viewModel.ActivityTypeId,
            };
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction("MainIndex", "Courses");
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Name", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", activity.ModuleId);
            return View(viewModel);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var activity = await _context.Activity.Include(m => m.Module).FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Name", activity.ActivityTypeId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", activity.ModuleId);
            var viewModel = new LmsViewModel
            {
                Name = activity.Name,
                Description = activity.Description,
                StartDateTime = activity.StartDateTime,
                EndDateTime = activity.EndDateTime,
                ActivityTypeId = activity.ActivityTypeId,
                ModuleId = activity.ModuleId,
                ModuleStart = activity.Module.StartDateTime,
                ModuleEnd = activity.Module.EndDateTime,
            };

            return View(viewModel);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDateTime,EndDateTime,ModuleId,ActivityTypeId,ModuleStart,ModuleEnd")] LmsViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activity = await _context.Activity.FirstOrDefaultAsync(m => m.Id == id);

                    activity.Name = viewModel.Name;
                    activity.Description = viewModel.Description;
                    activity.StartDateTime = viewModel.StartDateTime;
                    activity.EndDateTime = viewModel.EndDateTime;
                    activity.ActivityTypeId = viewModel.ActivityTypeId;
                    activity.ModuleId = viewModel.ModuleId;

                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(viewModel.Id))
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
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Name", viewModel.Id);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Name", viewModel.ModuleId);
            return View(viewModel);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var activity = await _context.Activity
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activity.FindAsync(id);
            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction("MainIndex", "Courses");
        }
        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ActivitiesOfTypeAssignment()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var courseId = loggedInUser.CourseId;

            var applicationDbContext = _context.Activity.Include(a => a.ActivityType).Include(a => a.Module);
            if (User.IsInRole("Elev"))
            {
                var model = applicationDbContext
                .Where(a => a.Module.Id == courseId)
                .Where(a => a.ActivityType.Name == "Övningsuppgift");

                return View(await model.ToListAsync());
            }
            else if (User.IsInRole("Lärare") || User.IsInRole("Admin"))
            {
                return View(await applicationDbContext.Where(a => a.ActivityType.Name == "Övningsuppgift").ToListAsync());
            }
            return NotFound();

        }
    }
}
