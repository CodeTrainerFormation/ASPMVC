using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Abstract
{
    public interface IStudentRepository
    {
        IEnumerable<Student> AllStudents();
        Student GetStudent(int id);
        int AddStudent(Student student);
        int EditStudent(Student student);
        int DeleteStudent(int id);
        void Dispose();
    }
}
