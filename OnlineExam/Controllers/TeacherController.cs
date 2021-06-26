using OnlineExam.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineExam.Controllers
{
    [CustomAuthorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        // GET: Teacher
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