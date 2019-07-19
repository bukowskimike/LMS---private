using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Models;
using LMS.Data;
using Microsoft.AspNetCore.Authorization;
using LMS.Core.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace LMS.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var courseId = loggedInUser.CourseId;

            var applicationDbContext = _context.Course.Include(c => c.Documents);

            if (User.IsInRole("Elev"))
            {
                var model = applicationDbContext
                    .Where(c => c.Id == courseId
                    || c.Documents.FirstOrDefault().Id == courseId);
                return View(await model.ToListAsync());
            }

            else if (User.IsInRole("Lärare") || User.IsInRole("Admin"))
            {
                return View(await applicationDbContext.ToListAsync());
            }
            return NotFound();
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin, Lärare")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin, Lärare")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDateTime,EndDateTime")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MainIndex));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDateTime,EndDateTime")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MainIndex");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MainIndex));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }

        // GET: Courses and Modules
        public async Task<IActionResult> MainIndex()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var courseId = loggedInUser.CourseId;

            var applicationDbContext = _context.Course.Include(c => c.Documents);

            if (User.IsInRole("Elev"))
            {
                var model = applicationDbContext
                    .Where(c => c.Id == courseId || 
                    c.Documents.FirstOrDefault().Id == courseId).OrderBy(c=> c.StartDateTime);
                // return View(await model.ToListAsync());
                return View("MainIndex", model);
            }

            else if (User.IsInRole("Lärare") || User.IsInRole("Admin"))
            {
                return View("MainIndex", applicationDbContext.OrderBy(c => c.StartDateTime));
            }
            return NotFound();

        }

        public async Task<IActionResult> GetModulesByCourseId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var courseWithDocs = _context.Course.Include(c => c.Documents);
                ViewData["course"] = courseWithDocs.FirstOrDefault(c => c.Id == id);
                var model = await _context.Module.Where(c => c.CourseId == id).OrderBy(m => m.StartDateTime).ToListAsync();
                return PartialView("_ModulesIndexPartial", model);
            }
            return RedirectToAction(nameof(MainIndex));
        }
        public async Task<IActionResult> GetActivitiesByModuleId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var moduleWithDocs = _context.Module.Include(m => m.Documents);
                ViewData["module"] = moduleWithDocs.FirstOrDefault(m => m.Id == id);
                var model = await _context.Activity.Where(m => m.Module.Id == id).OrderBy(a => a.StartDateTime).ToListAsync();
                return PartialView("_ActivitiesIndexPartial", model);
            }
            return RedirectToAction(nameof(MainIndex));
        }
        public async Task<IActionResult> GetActivityBody(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var activityWithDocs = _context.Activity.Include(c => c.Documents);
                ViewData["activity"] = activityWithDocs.FirstOrDefault(c => c.Id == id);
                var model = await _context.Activity.ToListAsync();
                return PartialView("_ActivityBodyPartial", model);
            }
            return RedirectToAction(nameof(MainIndex));
        }
    }
}
