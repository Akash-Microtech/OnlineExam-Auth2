using OnlineExam.Authentication;
using OnlineExam.DbContext;
using OnlineExam.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            List<GetExamByUserId_Result> exams = new List<GetExamByUserId_Result>();

            foreach (var item in data)
            {
                AttendExam attend = db.AttendExams.Where(a => a.ExamId == item.Id && a.StudentId == id).FirstOrDefault();

                if(attend == null || attend.IsAttented != 1)
                {
                    exams.Add(item);
                }
            }

            StudentDashboardViewModel studentDashboard = new StudentDashboardViewModel()
            {
                GetExamByUserId = exams,
                StudentId = id
            };

            return View(studentDashboard);
        }

        public ActionResult ExamDetails(int? examId, int? Id)
        {
            if(examId != null && Id != null)
            {
                AttendExam attend = db.AttendExams.Where(a => a.ExamId == examId && a.StudentId == Id).FirstOrDefault();
                var data = db.GetAllExamById(examId).FirstOrDefault();

                if ((attend == null || attend.IsAttented == 2) && data != null)
                {
                    if(attend == null)
                    {
                        ViewBag.AttendData = "AttendDataEC5";
                        return View(data);
                    }
                    else
                    {
                        if(attend.IsAttented == 2)
                        {

                            ViewBag.AttendData = "AttendDataEC1";
                            return View(data);
                        }
                    }                    
                }

                TempData["ErrorMessage"] = "You are already attended the examination.";
                return RedirectToAction("Support");
            }

            TempData["ErrorMessage"] = "The Exam you are looking for could not be found.";
            return RedirectToAction("Support");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExamDetails(GetAllExamById_Result getExam)
        {
            if (Convert.ToString(getExam.Id) != null && Convert.ToString(getExam.ExGroupId) != null)
            {
                if (getExam.Name == "AttendDataEC5")
                {
                    AttendExam exam = new AttendExam()
                    {
                        ExamId = getExam.Id,
                        StudentId = getExam.ExGroupId,
                        IsAttented = 2,
                        IsDeletedEr = 0,
                        CreatedDateAe = DateTime.Now
                    };

                    db.AttendExams.Add(exam);
                    db.SaveChanges();
                    return RedirectToAction("Instructions", new { examId = getExam.Id, Id = getExam.ExGroupId });
                }

                if (getExam.Name == "AttendDataEC1")
                {
                    return RedirectToAction("Instructions", new { examId = getExam.Id, Id = getExam.ExGroupId });
                }

                TempData["ErrorMessage"] = "You are already attended the examination.";
                return RedirectToAction("Support");
            }

            TempData["ErrorMessage"] = "The Exam you are looking for could not be found.";
            return RedirectToAction("Support");
        }

        public ActionResult Instructions(int? examId, int? Id)
        {
            if (examId != null)
            {
                AttendExam attend = db.AttendExams.Where(a => a.ExamId == examId && a.StudentId == Id).FirstOrDefault();
                var data = db.GetAllExamById(examId).FirstOrDefault();

                if ((attend == null || attend.IsAttented == 2) && data != null)
                {
                    if (attend == null)
                    {
                        ViewBag.AttendData = "AttendDataEC5";
                        return View(data);
                    }
                    else
                    {
                        if (attend.IsAttented == 2)
                        {

                            ViewBag.AttendData = "AttendDataEC2";
                            return View(data);
                        }
                    }
                }

                TempData["ErrorMessage"] = "You are already attended the examination.";
                return RedirectToAction("Support");
            }

            TempData["ErrorMessage"] = "The Exam you are looking for could not be found.";
            return RedirectToAction("Support");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Instructions(GetAllExamById_Result attendExam)
        {
            if (Convert.ToString(attendExam.Id) != null && Convert.ToString(attendExam.ExGroupId) != null)
            {
                if (attendExam.Name == "AttendDataEC2")
                {
                    AttendExam attend = db.AttendExams.Where(a => a.ExamId == attendExam.Id && a.StudentId == attendExam.ExGroupId).FirstOrDefault();
                    attend.IsAttented = 3;
                    db.Entry(attend).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Exam", new { examId = attendExam.Id, Id = attendExam.ExGroupId });
                }

                if (attendExam.Name == "AttendDataEC5")
                {
                    return RedirectToAction("Exam", new { examId = attendExam.Id, Id = attendExam.ExGroupId });
                }

                TempData["ErrorMessage"] = "You are already attended the examination.";
                return RedirectToAction("Support");
            }

            TempData["ErrorMessage"] = "The Exam you are looking for could not be found.";
            return RedirectToAction("Support");
        }
    

        public ActionResult Exam(int? examId, int? Id)
        {
            if (examId == null || Id == null)
            {
                TempData["ErrorMessage"] = "The Exam you are looking for could not be found.";
                return RedirectToAction("Support");
            }

            AttendExam attend = db.AttendExams.Where(a => a.ExamId == examId && a.StudentId == Id).FirstOrDefault();

            if (attend != null && attend.IsAttented == 3)
            {
                attend.IsAttented = 1;
                db.Entry(attend).State = EntityState.Modified;
                db.SaveChanges();

                var data = db.GetAllExamById(examId).FirstOrDefault();
                var qus = db.GetAllQusByExamId(data.Id, data.QsAsFrom).ToList();

                if (examId != null && data != null)
                {
                    ExamViewModel exam = new ExamViewModel()
                    {
                        GetExam = data,
                        GetAllQus = qus
                    };

                    return View(exam);
                }
            } 

            TempData["ErrorMessage"] = "You are already attended the examination.";
            return RedirectToAction("Support");
        }

        public ActionResult Support()
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }

            return View();
        }
    }
}