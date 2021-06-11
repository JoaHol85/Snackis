using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snackis.Areas.Identity.Data;
using Snackis.Data.Models;

namespace Snackis.Data
{
    public class SnackisContext : IdentityDbContext<SnackisUser>
    {
        public SnackisContext(DbContextOptions<SnackisContext> options)
            : base(options)
        {
        }

        public DbSet<MainThread> MainThreads { get; set; }
        public DbSet<SubThread> SubThreads { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ReportedMessage> ReportedMessages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<SmileyInfo> SmileyInfos { get; set; }
        public DbSet<SmileyMessageUser> SmileyMessageUsers { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<MessageImage> MessageImages { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
