using DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DAL
{
    public class SchoolDb : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public SchoolDb()
            : base("SchoolDatabase")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Classroom>().ToTable("Classroom");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");

            modelBuilder.Entity<Teacher>()
                .HasOptional(t => t.Classroom)
                .WithOptionalDependent(c => c.Teacher);


            base.OnModelCreating(modelBuilder);
        }
    }
}