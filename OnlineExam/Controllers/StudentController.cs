using OnlineExam.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExam.Controllers
{
    [CustomAuthorize(Roles = "Student")]
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}