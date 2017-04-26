using DAL;
using DomainModel;
using Repository.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NetSchoolWeb.Controllers
{
    public class StudentController : Controller
    {
        private IStudentRepository repository;

        public StudentController(IStudentRepository studentRepository)
        {
            this.repository = studentRepository;
        }

        [ChildActionOnly]
        public PartialViewResult List()
        {
            return PartialView("_StudentsList", repository.AllStudents());
        }

        // GET: Student
        public ActionResult Index()
        {
            var students = repository.AllStudents();


            if (students.ToList().Count > 0)
            {
                ViewBag.Count = students.ToList().Count;
                return View("IndexList", students);
            }
            else
                return View("Empty");
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = repository.GetStudent(id.Value);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View(new Student());
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                repository.AddStudent(student);

                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            Student student = repository.GetStudent(id);

            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                repository.EditStudent(student);

                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            Student student = repository.GetStudent(id);

            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // POST: Student/Delete/5
        //[HttpPost]
        //public ActionResult Delete(Student student)
        //{
        //    context.Entry(student).State = EntityState.Deleted;
        //    context.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        // POST: Student/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.DeleteStudent(id);

            return RedirectToAction("Index");
        }

        public ActionResult Photo(int studentid)
        {
            return Redirect("https://www.youtube.com/watch?v=oavMtUWDBTM&t=30");
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}
