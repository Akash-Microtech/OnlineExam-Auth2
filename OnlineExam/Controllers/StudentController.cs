using OnlineExam.Authentication;
using OnlineExam.DbContext;
using OnlineExam.ViewModel;
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
        private readonly Exam_DBEntities db = new Exam_DBEntities();

        // GET: Student
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            int id = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            DateTime today = DateTime.Now.Date;
            var data = db.GetExamByUserId(id, today).ToList();

            StudentDashboardViewModel studentDashboard = new StudentDashboardViewModel()
            {
                GetExamByUserId = data
            };

            return View(studentDashboard);
        }

        public ActionResult ExamDetails(int? examId)
        {
            var data = db.GetAllExamById(examId).FirstOrDefault();
            if (examId != null && data != null )
            {
                return View(data);
            }

            TempData["StatusMessage"] = "Exam Not Found.";
            return RedirectToAction("Dashboard");
        }

        public ActionResult Instructions(int? examId)
        {
            var data = db.GetAllExamById(examId).FirstOrDefault();
            if (examId != null && data != null)
            {
                return View(data);
            }

            TempData["StatusMessage"] = "Exam Not Found.";
            return RedirectToAction("Dashboard");
        }

        public ActionResult Exam(int? examId)
        {
            var data = db.GetAllExamById(examId).FirstOrDefault();
            var qus = db.GetExamIdWiseQuestions(examId).ToList();
            if (examId != null && data != null)
            {
                ExamViewModel exam = new ExamViewModel()
                {
                    GetExam = data,
                    GetAllQus = qus
                };

                return View(exam);
            }

            TempData["StatusMessage"] = "Exam Not Found.";
            return RedirectToAction("Dashboard");
        }
    }
}