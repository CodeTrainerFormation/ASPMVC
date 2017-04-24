using NetSchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NetSchoolWeb.Data
{
    public class SchoolInitializer : DropCreateDatabaseAlways<SchoolDb>
    {
        protected override void Seed(SchoolDb context)
        {
            #region Classrooms
            var classrooms = new List<Classroom>()
            {
                new Classroom
                {
                    Name = "Salle Bill Gates",
                    Floor = 5,
                    Corridor = "Rouge",
                },
                new Classroom
                {
                    Name = "Salle Scott Hanselman",
                    Floor = 3,
                    Corridor = "Bleu",
                },
            };
            context.Classrooms.AddRange(classrooms);
            context.SaveChanges();
            #endregion

            #region Teachers
            var teachers = new List<Teacher>()
            {
                new Teacher()
                {
                    FirstName = "Barney",
                    LastName = "Stinson",
                    Age = 35,
                    Discipline = "Economie",
                    Email = "barney.stinson@gnb.com",
                    HiringDate = new DateTime(2008, 08, 30),
                    Classroom = classrooms[0],
                },
                new Teacher()
                {
                    FirstName = "Perry",
                    LastName = "Cox",
                    Age = 45,
                    Discipline = "Medecine",
                    Email = "perry@sacredheart.com",
                    HiringDate = new DateTime(2000, 05, 15),
                    Classroom = classrooms[1],
                },
            };
            context.Teachers.AddRange(teachers);
            context.SaveChanges();
            #endregion

            #region Students
            var students = new List<Student>()
            {
                new Student()
                {
                    FirstName = "Ted",
                    LastName = "Mosby",
                    Age = 20,
                    Average = 12.0,
                    IsClassDelegate = false,
                    Email = "ted@nschool.com",
                    Classroom = classrooms[0],
                },
                new Student()
                {
                    FirstName = "John",
                    LastName = "Dorian",
                    Age = 22,
                    Average = 19.0,
                    IsClassDelegate = true,
                    Email = "john@nschool.com",
                    Classroom = classrooms[0],
                },
                new Student()
                {
                    FirstName = "Marshall",
                    LastName = "Eriksen",
                    Age = 21,
                    Average = 9.0,
                    IsClassDelegate = false,
                    Email = "marshall@nschool.com",
                    Classroom = classrooms[0],
                },
                new Student()
                {
                    FirstName = "Robin",
                    LastName = "Scherbatsky",
                    Age = 23,
                    Average = 13.0,
                    IsClassDelegate = false,
                    Email = "robin@nschool.com",
                    Classroom = classrooms[1],
                },
                new Student()
                {
                    FirstName = "Lily",
                    LastName = "Aldrin",
                    Age = 20,
                    Average = 14.0,
                    IsClassDelegate = false,
                    Email = "lily@nschool.com",
                    Classroom = classrooms[1],
                },
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            #endregion

            base.Seed(context);
        }


    }
}