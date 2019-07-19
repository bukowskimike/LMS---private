using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LMS.Core.Models;
using LMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace LMS.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Lärare, Admin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        //public SelectList allCourses;
        //public SelectList roles;
        public int? courseId { get; set; }
        public string courseName { get; set; }
        public string role;
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(30, ErrorMessage = "{0}et måste vara minst {2} och max {1} tecken långt.", MinimumLength = 1)]
            [Display(Name = "Förnamn")]
            public string FirstName { get; set; }
            [Required]
            [StringLength(30, ErrorMessage = "{0}et måste vara minst {2} och max {1} tecken långt.", MinimumLength = 2)]
            [Display(Name = "Efternamn")]
            public string LastName { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string Email { get; set; }
            [Display(Name = "Kurs")]
            public int? CourseId { get; set; }
            [Display(Name = "Roll")]
            public string RoleName { get; set; }
        }

        public void OnGet(int? id, string returnUrl = null)
        {
            if(id == null)
            {
                role = "Lärare";
                //role = _context.Roles.Where(r => r.Name == "Lärare").FirstOrDefault().Name;
            }
            else
            {
                role = "Elev";
                courseId = id;
                courseName = _context.Course.Where(c => c.Id == id).FirstOrDefault().Name;
                //role = _context.Roles.Where(r => r.Name == "Elev").FirstOrDefault().Name;
            }
            //allCourses = new SelectList(_context.Course, "Id", "Name");
            //roles = new SelectList(_context.Roles.Where(r => r.Name != "Admin"), "Name", "Name");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { FirstName = Input.FirstName, LastName = Input.LastName, UserName = Input.Email, Email = Input.Email, CourseId = Input.CourseId };

                var result = await _userManager.CreateAsync(user, "Abc123!");
                await _userManager.AddToRoleAsync(user, Input.RoleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return RedirectToAction("Index", "Students");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
