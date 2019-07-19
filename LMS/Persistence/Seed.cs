using LMS.Core.Models;
using LMS.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Persistence
{
    public static class Seed
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Adding Student Role
            var studentCheck = await roleManager.RoleExistsAsync("Elev");
            if (!studentCheck)
            {
                //create the roles and seed them to the database
                await roleManager.CreateAsync(new IdentityRole("Elev"));
            }
            //Adding Teacher Role
            var teacherCheck = await roleManager.RoleExistsAsync("Lärare");
            if (!teacherCheck)
            {
                //create the roles and seed them to the database
                await roleManager.CreateAsync(new IdentityRole("Lärare"));
            }
            //Adding Admin Role
            var adminCheck = await roleManager.RoleExistsAsync("Admin");
            if (!adminCheck)
            {
                //create the roles and seed them to the database
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }

        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //Adding Admin user
            var adminUser = await userManager.FindByEmailAsync("adminpw@lexicon.com");
            if (adminUser == null)
            {
                var AdminPW = new ApplicationUser()
                {
                    FirstName = "Admin",
                    LastName = "PW",
                    UserName = "adminpw@lexicon.com",
                    Email = "adminpw@lexicon.com"
                };
                //create the user and seed to the database
                await userManager.CreateAsync(AdminPW, "Abc123!");
                await userManager.AddToRoleAsync(AdminPW, "Admin");
            }
        }
        public static async Task CreateActivityTypes(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var appDbContext = context.ActivityType;



            var activityTypes = new List<string>() { "Föreläsning", "E-learning", "Inlämningsuppgift", "Övningsuppgift", "Repitition" };
            var typeDescriptions = new List<string>() { "Föreläsning enligt kursschema", "Titta på anvisad e-learningfilm enligt kursschemar", "Inlämningsuppgiften skall redovisas i elevhantering, i dokument", "Övningsuppgift enligt kursschema", "Repitition enligt kursschem alt. lärares anvisning!" };
            for (int i = 0; i < activityTypes.Count; i++)
            {
                var nameOfType = activityTypes[i];
                var activityType = appDbContext.FirstOrDefault(a => a.Name == nameOfType);

                if (activityType == null)
                {
                    var type = new ActivityType
                    {
                        Name = nameOfType,
                        Description = typeDescriptions[i]
                    };
                    context.Add(type);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
