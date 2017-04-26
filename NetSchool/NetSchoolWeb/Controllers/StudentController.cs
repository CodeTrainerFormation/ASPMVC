using DAL;
using DomainModel;
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
        private SchoolDb context = new SchoolDb();

        [ChildActionOnly]
        public PartialViewResult List()
        {
            return PartialView("_StudentsList", context.Students.ToList());
        }

        // GET: Student
        public ActionResult Index()
        {
            var students = context.Students.ToList();


            if (students.Count > 0)
            {
                ViewBag.Count = students.Count;
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
            Student student = context.Students.Find(id);
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
                context.Students.Add(student);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            Student student = context.Students.Find(id);

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
                context.Entry(student).State = EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            Student student = context.Students.Find(id);

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
            Student student = context.Students.Find(id);

            context.Students.Remove(student);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [ActionName("StudentPhoto")]
        public ActionResult Photo(int studentid)
        {
            return Redirect("https://www.youtube.com/watch?v=oavMtUWDBTM&t=30");
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
