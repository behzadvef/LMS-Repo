using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_Proj.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            return userIdentity;
        }


        // Attributes

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Connection
//        [ForeignKey("Group")]
        [Display(Name = "Group")]
        public int? GroupId { get; set; }
        public virtual Group Groups { get; set; }
       
        [ForeignKey("ApplicationUserId")]
        public virtual ICollection<File> Files { get; set; }

      

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Group> Groups { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<Activity> Activities { get; set; }

 //       public System.Data.Entity.DbSet<LMS_Proj.Models.ApplicationUser> ApplicationUsers { get; set; }

//        public System.Data.Entity.DbSet<LMS_Proj.Models.ApplicationUser> ApplicationUsers { get; set; }


    }
}