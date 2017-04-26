using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DomainModel;
using Repository.Repositories.Abstract;
using System.Data.Entity;

namespace Repository.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private SchoolDb context;

        public StudentRepository() : this(new SchoolDb())
        { }

        public StudentRepository(SchoolDb ctx) 
        {
            this.context = ctx;
        }

        public int AddStudent(Student student)
        {
            context.Students.Add(student);
            return context.SaveChanges();
        }

        public IEnumerable<Student> AllStudents()
        {
            return context.Students.ToList();
        }

        public int DeleteStudent(int id)
        {
            Student student = this.GetStudent(id);
            if(student.Equals(null))
            {
                return 0;
            }

            context.Students.Remove(student);
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public int EditStudent(Student student)
        {
            context.Entry(student).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public Student GetStudent(int id)
        {
            return context.Students.Find(id);
        }
    }
}
