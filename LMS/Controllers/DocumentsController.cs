using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Models;
using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LMS.Core.ViewModels;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace LMS.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileProvider _fileProvider;

        public DocumentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IFileProvider fileprovider)
        {
            _context = context;
            _userManager = userManager;
            _fileProvider = fileprovider;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var appUserId = loggedInUser.Id;
            var courseId = _userManager.Users.Where(u => u.Id == appUserId);

            var applicationDbContext = _context.Document;

            if (User.IsInRole("Elev"))
            {
                var model = applicationDbContext
                    .Where(d => d.ApplicationUserId == appUserId);
                return View(await model.ToListAsync());
            }
            else if (User.IsInRole("Lärare") || User.IsInRole("Admin"))
            {
                return View(await _context.Document.ToListAsync());
            }
            return NotFound();
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(c => c.Course)
                .Include(m => m.Module)
                .Include(a => a.Activity)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult CreateCourseDocument(int id)
        {

            // set default date
            var model = new DocumentViewModel()
            {
                CourseId = id
            };
            return View("Create", model);
        }

        // GET: Documents/Create
        public IActionResult CreateModuleDocument(int id)
        {

            // set default date
            var model = new DocumentViewModel()
            {
                ModuleId = id
            };
            return View("Create", model);
        }
        // GET: Documents/Create
        public IActionResult CreateActivityDocument(int id)
        {

            // set default date
            var model = new DocumentViewModel()
            {
                ActivityId = id
            };
            return View("Create", model);
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("File,Description,Feedback,CourseId,ModuleId,ActivityId")] DocumentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            model.Name = model.File.FileName;

            var document = new Document()
            {
                Name = model.File.FileName,
                Description = model.Description,
                Feedback = model.Feedback,
                UploadDateTime = DateTime.Now,
                Deadline = model.Deadline,
                ApplicationUserId = user.Id,
                CourseId = model.CourseId,
                ModuleId = model.ModuleId,
                ActivityId = model.ActivityId
            };

            if (ModelState.IsValid)
            {
                if (model.File == null || model.File.Length == 0)
                {
                    return Content("ingen fil vald!");
                }
                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "LMSDocuments",
                            model.File.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction("MainIndex", "Courses");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile FileToUpload)
        {
            if (FileToUpload == null || FileToUpload.Length == 0)
                return Content("ingen fil vald!");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "LMSDocuments",
                        FileToUpload.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await FileToUpload.CopyToAsync(stream);
            }

            return RedirectToAction("Files");
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            var documentviewmodel = new DocumentViewModel()
            {
                Name = document.Name,
                Description = document.Description,
                Feedback = document.Feedback,
                UploadDateTime = DateTime.Now,
                Deadline = document.Deadline,
                ApplicationUserId = document.ApplicationUserId,
                CourseId = document.CourseId,
                ModuleId = document.ModuleId,
                ActivityId = document.ActivityId
            };

            ViewBag.Course = await _context.Course
              .Select(c => new SelectListItem
              {
                  Text = c.Name,
                  Value = c.Id.ToString()
              }).ToListAsync();

            ViewBag.Module = await _context.Module
              .Select(m => new SelectListItem
              {
                  Text = m.Name,
                  Value = m.Id.ToString()
              }).ToListAsync();

            ViewBag.Activity = await _context.Activity
              .Select(a => new SelectListItem
              {
                  Text = a.Name,
                  Value = a.Id.ToString()
              }).ToListAsync();

            return View(documentviewmodel);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Feedback,UploadDateTime,FileLocationURL,ApplicationUserId,CourseId,ModuleId,ActivityId")] DocumentViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var document = await _context.Document.FirstOrDefaultAsync(d => d.Id == id);

            if (ModelState.IsValid)
            {
                document.Name = model.Name;
                document.Description = model.Description;
                document.Feedback = model.Feedback;
                document.UploadDateTime = DateTime.Now;
                document.Deadline = model.Deadline;
                document.ApplicationUserId = model.ApplicationUserId;
                document.CourseId = model.CourseId;
                document.ModuleId = model.ModuleId;
                document.ActivityId = model.ActivityId;

                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Document.FindAsync(id);
            _context.Document.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.Id == id);
        }

        public IActionResult Files()
        {
            var model = new FilesViewModel();
            foreach (var item in _fileProvider.GetDirectoryContents(""))
            {
                model.Files.Add(
                    new FileDetails { Name = item.Name, Path = item.PhysicalPath });
            }
            return View(model);
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filnamn saknas!");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "LMSDocuments", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".pptx", "application/pptx"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
