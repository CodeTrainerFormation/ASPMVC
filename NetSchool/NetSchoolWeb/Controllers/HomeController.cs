using DAL;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetSchoolWeb.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private SchoolDb context = new SchoolDb();

        public ActionResult Index()
        {
            var results = context.Students.Count();
            Response.Write(results);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();

            base.Dispose(disposing);
        }
    }
}