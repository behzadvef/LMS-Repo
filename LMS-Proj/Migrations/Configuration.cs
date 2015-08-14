namespace LMS_Proj.Migrations
{
    using System;    
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LMS_Proj.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS_Proj.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LMS_Proj.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //




            // Lägger till följande  rader för att skapa/lägga till användardatabas
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!roleManager.RoleExists("admin"))
            {
                var role = new IdentityRole("admin");
                roleManager.Create(role);
            }
            context.SaveChanges();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
                userManager.Create(user, "Admin123!");
                userManager.AddToRole(user.Id, "admin");
            }
            context.SaveChanges();


            if (!context.Users.Any(u => u.UserName == "DeletedUserHeir@admin.com"))
            {
                var user = new ApplicationUser { UserName = "DeletedUserHeir@admin.com", Email = "DeletedUserHeir@admin.com" };
                userManager.Create(user, "Admin123!");
                userManager.AddToRole(user.Id, "admin");
            }
            context.SaveChanges();



            context.Activities.AddOrUpdate(
                new Activity { Name = "C#", StartDate = new DateTime(2015, 5, 26, 9, 0, 0), EndDate = new DateTime(2015, 5, 26, 12, 0, 0), Description = "An Introduction to C#" },
                new Activity { Name = "Classes", StartDate = new DateTime(2015, 5, 26, 9, 0, 0), EndDate = new DateTime(2015, 5, 26, 12, 0, 0), Description = "An Introduction to Classes" },
                new Activity { Name = "MVC", StartDate = new DateTime(2015, 5, 27, 9, 0, 0), EndDate = new DateTime(2015, 5, 27, 12, 0, 0), Description = "MVC Description" },
                new Activity { Name = "ASP", StartDate = new DateTime(2015, 5, 27, 9, 0, 0), EndDate = new DateTime(2015, 5, 27, 12, 0, 0), Description = "ASP Description" },
                new Activity { Name = "HTML", StartDate = new DateTime(2015, 5, 27, 13, 0, 0), EndDate = new DateTime(2015, 5, 27, 17, 0, 0), Description = "HTML Description" }
              );
            context.SaveChanges();  //Saves the buffers to database


            context.Groups.AddOrUpdate(
                new Group { MaxMembers = 15, GroupName = ".net", ClassLocation = "Gamma", GroupDescription = ".net group started in May" },
                new Group { MaxMembers = 15, GroupName = "SharePoint", ClassLocation = "Epsilon", GroupDescription = "SharePoint group started in May" },
                new Group { MaxMembers = 15, GroupName = "Java", ClassLocation = "Alpha", GroupDescription = "Java Programming group started in April" }
              );
            context.SaveChanges(); //Saves the buffers to database



            context.Files.AddOrUpdate(
                new File { GroupId = 2, Type = FileType.Lesson, SubmissionDate = new DateTime(2015, 5, 26, 9, 0, 0), FileName = "Lesson1", FilePath = "C:\\LMS-Files", FileLink ="http:/lexicon.se/lesson1", Comment = "" },
                new File { Receiver = context.Users.First(), Type = FileType.Exercise, SubmissionDate = new DateTime(2015, 5, 26, 9, 0, 0), FileName = "Exercise1", FilePath = "C:\\LMS-Files", FileLink = "", Comment = "Behzads Exercise 1" },
                new File { Type = FileType.Comment, SubmissionDate = new DateTime(2015, 5, 26, 9, 0, 0), FileName = "Lesson1", FilePath = "C:\\LMS-Files", FileLink ="", Comment = "OK" },
                new File {GroupId = 1, Type = FileType.SharedFile, SubmissionDate = new DateTime(2015, 5, 26, 9, 0, 0), FileName = "SharedFile1", FilePath = "C:\\LMS-Files", FileLink ="", Comment = "" }
             );
            context.SaveChanges(); //Saves the buffers to database


 
        }
    }
}


