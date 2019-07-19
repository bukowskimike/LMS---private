using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Data;
using Microsoft.AspNetCore.Mvc;
using LMS.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using LMS.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await userManager.GetUserAsync(User);
            var loggedInUserCourseId = loggedInUser.CourseId;
            if (User.IsInRole("Elev"))
            {
                if (loggedInUserCourseId == null)
                {
                    return NotFound();
                }
                else
                {
                    var model = _context.Users
                        .Where(m => m.CourseId == loggedInUserCourseId)
                        .Include(n => n.Course).OrderBy(n => n.FirstName)
                        .Select(s => new StudentsViewModel { Id = s.Id, FirstName = s.FirstName, LastName = s.LastName, Email = s.Email, Course = s.Course.Name });
                    ViewBag.Courses = _context.Course.Where(c => c.Name == model.FirstOrDefault().Course);
                    return View(model);
                }
            }
            else
            {
                if (User.IsInRole("Admin") | User.IsInRole("Lärare"))
                {
                    var model = _context.Users
                        .Where(u => u.UserName != "adminpw@lexicon.com")
                        .OrderBy(n => n.FirstName)
                        .OrderBy(n => n.CourseId)
                        .Select(s => new StudentsViewModel { Id = s.Id, FirstName = s.FirstName, LastName = s.LastName, Email = s.Email, Course = s.Course.Name });
                    ViewBag.Courses = _context.Course;
                    return View(model);
                }
                else
                {
                    return NotFound();
                }
            }
        }
        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name");
            ViewData["RoleName"] = new SelectList(_context.Roles, "Name", "Name");
            var roleName = await userManager.GetRolesAsync(applicationUser);
            var viewModel = new StudentsViewModel
            {
                Id = applicationUser.Id,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Email = applicationUser.Email,
                CourseId = applicationUser.CourseId,
                RoleName = roleName.FirstOrDefault()
            };
            return View(viewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Email,CourseId,RoleName")] StudentsViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
                    var userRole = await userManager.GetRolesAsync(user);
                    if (viewModel.RoleName != userRole.FirstOrDefault())
                    {
                        if (userRole.FirstOrDefault() != null)
                        {
                            await userManager.RemoveFromRoleAsync(user, userRole.FirstOrDefault());
                        }
                        await userManager.AddToRoleAsync(user, viewModel.RoleName);
                    }

                    user.FirstName = viewModel.FirstName;
                    user.LastName = viewModel.LastName;
                    user.Email = viewModel.Email;
                    user.CourseId = viewModel.CourseId;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Name");
            ViewData["RoleName"] = new SelectList(_context.Roles, "Name", "Name");
            return View(viewModel);
        }
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            string course;
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Course == null)
            {
                course = "Ej registrerad";
            }
            else
            {
                course = user.Course.Name;
            }
            var roleName = await userManager.GetRolesAsync(user);
            var viewModel = new StudentsViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Course = course,
                RoleName = roleName.FirstOrDefault()
            };
            return View(viewModel);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool userExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}