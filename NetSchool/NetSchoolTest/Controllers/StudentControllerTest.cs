using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSchoolWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NetSchoolTest.Controllers
{
    [TestClass]
    public class StudentControllerTest
    {
        [TestMethod]
        public void TestIndexView()
        {
            var controller = new StudentController();

            var result = controller.Index() as ViewResult;

            Assert.AreEqual("IndexList", result.ViewName);
        }

        [TestMethod]
        public void TestDetailType()
        {
            var controller = new StudentController();

            var result = controller.Details(3) as ViewResult;

            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Student));
        }

        [TestMethod]
        public void TestDetailError()
        {
            var controller = new StudentController();

            var result = controller.Details(1);

            Assert.AreEqual(typeof(HttpNotFoundResult), result.GetType());
        }
    }
}
