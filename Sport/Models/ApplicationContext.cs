using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;




namespace Sport.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            /*Database.EnsureCreated();*/
        }
        public DbSet<Coach> Coach { get; set; }
       public DbSet<Klub> Klub { get; set; }

        public DbSet<Students> Students { get; set; }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<Sports> Sports { get; set; }
        public DbSet<Hall> Hall { get; set; }
        public DbSet<ClassesIndividual> ClassesIndividual { get; set; }
        public DbSet<RaspisIndividual> RaspisIndividual { get; set; }
        public DbSet<GroupClassesRaspis> GroupClassesRaspis { get; set; }
        public DbSet<ClassesGroup> ClassesGroup { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserClaim<int>>();
            modelBuilder.Ignore<IdentityRoleClaim<int>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
         

        }
       
    }
}
