using System;
using System.Collections.Generic;
using System.Text;
using LMS.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS.Core.ViewModels;

namespace LMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LMS.Core.Models.Course> Course { get; set; }
        public DbSet<LMS.Core.Models.Module> Module { get; set; }
        public DbSet<LMS.Core.Models.Activity> Activity { get; set; }
        public DbSet<LMS.Core.Models.Document> Document { get; set; }
        public DbSet<LMS.Core.Models.ActivityType> ActivityType{ get; set; }
        public DbSet<LMS.Core.ViewModels.DocumentViewModel> DocumentViewModel { get; set; }
    }
}
