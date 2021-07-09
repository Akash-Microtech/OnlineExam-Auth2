using OnlineExam.Authentication;
using OnlineExam.DbContext;
using OnlineExam.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineExam.Controllers
{
    [CustomAuthorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly Exam_DBEntities db = new Exam_DBEntities();

        // GET: Teacher
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult QaAsList()
        {
            if (TempData["StatusMessage"] != null)
            {
                ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
            }

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }

            int id = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            var list = db.GetAllTeacherQusAnsByUserId(id).ToList();
            return View(list);
        }

        public async Task<ActionResult> QsAs(int? id)
        {
            if (id == null)
            {
                var pro = new SelectList(db.Programmes.Where(p => p.IsDeleted == 0), "Id", "Name");
                ViewBag.PgmId = pro;
                var cou = new SelectList(db.Courses.Where(c => c.IsDeleted == 0), "Id", "Name");
                ViewBag.CourseId = cou;
                var subj = new SelectList(db.Subjects.Where(s => s.IsDeleted == 0), "Id", "Name");
                ViewBag.SubjectId = subj;

                var sub = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.SubPgmId = sub;
                ViewBag.ChapterId = sub;
                QsAsViewModel dtpQA = new QsAsViewModel()
                {
                    Questions = ""
                };

                return View(dtpQA);

            }
            else
            {
                var data = await db.Teachers_QuestionBank.Where(d => d.Id == id).FirstOrDefaultAsync();
                QsAsViewModel dtpQA = new QsAsViewModel()
                {
                    Id = data.Id,
                    Questions = data.Questions,
                    Option1 = data.Option1,
                    Option2 = data.Option2,
                    Option3 = data.Option3,
                    Option4 = data.Option4,
                    PrevQnYear = data.PrevQnYear,
                    CorrectAns = data.CorrectAns,
                    Mark = data.Mark,
                    PgmId = data.PgmId,
                    SubPgmId = data.SubPgmId,
                    CourseId = data.CourseId,
                    SubjectId = data.SubjectId,
                    ChapterId = data.ChapterId,
                    Photo = data.Photo
                };
                var pro = new SelectList(db.Programmes.Where(p => p.IsDeleted == 0), "Id", "Name", data.PgmId);
                ViewBag.PgmId = pro;
                var sub = new SelectList(db.SubPrograms.Where(p => p.IsDeleted == 0), "Id", "Name", data.SubPgmId);
                ViewBag.SubPgmId = sub;
                var cou = new SelectList(db.Courses.Where(c => c.IsDeleted == 0), "Id", "Name", data.CourseId);
                ViewBag.CourseId = cou;
                var subj = new SelectList(db.Subjects.Where(s => s.IsDeleted == 0), "Id", "Name", data.SubjectId);
                ViewBag.SubjectId = subj;
                var chap = new SelectList(db.Chapters.Where(p => p.IsDeleted == 0), "Id", "Name", data.ChapterId);
                ViewBag.ChapterId = chap;

                return View(dtpQA);
            }
        }

        [HttpGet]
        public JsonResult SubProgram(int ID)
        {
            var sub = new SelectList(db.SubPrograms.Where(s => s.PgmId == ID && s.IsDeleted == 0), "Id", "Name");
            return Json(sub, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Chapters(int ID)
        {
            var chap = new SelectList(db.Chapters.Where(c => c.SubId == ID && c.IsDeleted == 0), "Id", "Name");
            return Json(chap, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> QsAs(QsAsViewModel dtpQAView, HttpPostedFileBase fileQus)
        {
            if (dtpQAView.Questions != null || fileQus != null || dtpQAView.Photo != null)
            {
                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                var uploadPath = "";
                var oldPath = "";
                string alpha = "Question_";
                Random random = new Random();
                int unique = random.Next(10000, 99999);
                int y = DateTime.Now.Year;
                int m = DateTime.Now.Month;
                var upFileName = alpha + y + m + unique;

                if (!ModelState.IsValid)
                {
                    var pro = new SelectList(db.Programmes.Where(p => p.IsDeleted == 0), "Id", "Name", dtpQAView.PgmId);
                    ViewBag.PgmId = pro;
                    var sub = new SelectList(db.SubPrograms.Where(p => p.IsDeleted == 0), "Id", "Name", dtpQAView.SubPgmId);
                    ViewBag.SubPgmId = sub;
                    var cou = new SelectList(db.Courses.Where(c => c.IsDeleted == 0), "Id", "Name", dtpQAView.CourseId);
                    ViewBag.CourseId = cou;
                    var subj = new SelectList(db.Subjects.Where(s => s.IsDeleted == 0), "Id", "Name", dtpQAView.SubjectId);
                    ViewBag.SubjectId = subj;
                    var chap = new SelectList(db.Chapters.Where(p => p.IsDeleted == 0), "Id", "Name", dtpQAView.ChapterId);
                    ViewBag.ChapterId = chap;

                    ViewBag.ErrorMessage = "Please fill in all the required fields";
                    return View(dtpQAView);
                }

                if (dtpQAView.Id != null)
                {
                    if (fileQus != null)
                    {
                        oldPath = dtpQAView.Photo;
                        var FileExt = Path.GetExtension(fileQus.FileName);
                        if (allowedExtensions.Contains(FileExt))
                        {
                            string dtpRegId = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().UniqueID;
                            string myfile = dtpRegId + "_" + upFileName + FileExt;
                            uploadPath = Path.Combine(Server.MapPath("~/Uploads/QuestionTeacher/"), myfile);
                            dtpQAView.Photo = "../../Uploads/QuestionTeacher/" + myfile;
                        }
                    }

                    Teachers_QuestionBank data = db.Teachers_QuestionBank.Find(dtpQAView.Id);
                    if (data != null)
                    {
                        data.Questions = dtpQAView.Questions;
                        data.Option1 = dtpQAView.Option1;
                        data.Option2 = dtpQAView.Option2;
                        data.Option3 = dtpQAView.Option3;
                        data.Option4 = dtpQAView.Option4;
                        data.PrevQnYear = dtpQAView.PrevQnYear;
                        data.CorrectAns = dtpQAView.CorrectAns;
                        data.Mark = dtpQAView.Mark;
                        data.ModifiedDateTime = DateTime.Now;
                        data.ModifiedBy = dtpQAView.CuserId;
                        data.PgmId = dtpQAView.PgmId;
                        data.SubPgmId = dtpQAView.SubPgmId;
                        data.CourseId = dtpQAView.CourseId;
                        data.SubjectId = dtpQAView.SubjectId;
                        data.ChapterId = dtpQAView.ChapterId;
                        data.Photo = dtpQAView.Photo;
                        db.Entry(data).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        if (fileQus != null)
                        {
                            fileQus.SaveAs(uploadPath);

                            string path = Server.MapPath(oldPath);
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                        }

                        TempData["StatusMessage"] = "Questions Answers Edited Succesfully.";
                        return RedirectToAction("QaAsList");
                    }

                    ViewBag.ErrorMessage = "Please fill in all the required fields";
                    return View(dtpQAView);
                }
                else
                {
                    if (fileQus != null)
                    {
                        var FileExt = Path.GetExtension(fileQus.FileName);
                        if (allowedExtensions.Contains(FileExt))
                        {
                            string dtpRegId = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().UniqueID;
                            string myfile = dtpRegId + "_" + upFileName + FileExt;
                            uploadPath = Path.Combine(Server.MapPath("~/Uploads/QuestionTeacher"), myfile);
                            dtpQAView.Photo = "../../Uploads/QuestionTeacher/" + myfile;
                        }
                    }

                    Teachers_QuestionBank data = new Teachers_QuestionBank()
                    {
                        Questions = dtpQAView.Questions,
                        Option1 = dtpQAView.Option1,
                        Option2 = dtpQAView.Option2,
                        Option3 = dtpQAView.Option3,
                        Option4 = dtpQAView.Option4,
                        PrevQnYear = dtpQAView.PrevQnYear,
                        CorrectAns = dtpQAView.CorrectAns,
                        Mark = dtpQAView.Mark,
                        CreatedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                        DeletedDateTime = DateTime.Now,
                        CreatedBy = dtpQAView.CuserId,
                        PgmId = dtpQAView.PgmId,
                        SubPgmId = dtpQAView.SubPgmId,
                        CourseId = dtpQAView.CourseId,
                        SubjectId = dtpQAView.SubjectId,
                        ChapterId = dtpQAView.ChapterId,
                        Photo = dtpQAView.Photo
                    };

                    db.Teachers_QuestionBank.Add(data);
                    await db.SaveChangesAsync();

                    if (fileQus != null)
                    {
                        fileQus.SaveAs(uploadPath);
                    }

                    TempData["StatusMessage"] = "Questions Answers Created Succesfully.";
                    return RedirectToAction("QaAsList");
                }
            }
            else
            {
                var pro = new SelectList(db.Programmes.Where(p => p.IsDeleted == 0), "Id", "Name", dtpQAView.PgmId);
                ViewBag.PgmId = pro;
                var sub = new SelectList(db.SubPrograms.Where(p => p.IsDeleted == 0), "Id", "Name", dtpQAView.SubPgmId);
                ViewBag.SubPgmId = sub;
                var cou = new SelectList(db.Courses.Where(c => c.IsDeleted == 0), "Id", "Name", dtpQAView.CourseId);
                ViewBag.CourseId = cou;
                var subj = new SelectList(db.Subjects.Where(s => s.IsDeleted == 0), "Id", "Name", dtpQAView.SubjectId);
                ViewBag.SubjectId = subj;
                var chap = new SelectList(db.Chapters.Where(p => p.IsDeleted == 0), "Id", "Name", dtpQAView.ChapterId);
                ViewBag.ChapterId = chap;

                ViewBag.ErrorMessage = "Please enter Questions or Select a Image Questions";
                return View(dtpQAView);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQsAS(int? deleteQsAsId)
        {
            if (deleteQsAsId != null)
            {
                Teachers_QuestionBank dataEntry = db.Teachers_QuestionBank.Find(deleteQsAsId);
                dataEntry.IsDeleted = 1;
                dataEntry.DeletedDateTime = DateTime.Now;
                db.Entry(dataEntry).State = EntityState.Modified;
                db.SaveChanges();

                TempData["StatusMessage"] = "Questions Answers Deleted Succesfully.";
                return RedirectToAction("QaAsList");
            }

            TempData["ErrorMessage"] = "Questions Answers Not Deleted.";
            return RedirectToAction("QaAsList");
        }

        public ActionResult Exams()
        {
            if (TempData["StatusMessage"] != null)
            {
                ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
            }

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }

            int id = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id;
            var list = db.GetAllExam().Where(e => e.IsDeleted == 0).ToList();
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActiveExam(int? activeId)
        {
            if (activeId == null)
            {
                TempData["ErrorMessage"] = "Account Not Activated";
                return RedirectToAction("Exams");
            }
            else
            {
                Exam exam = await db.Exams.Where(c => c.Id == activeId).FirstOrDefaultAsync();
                if (exam != null)
                {
                    exam.IsActive = 1;
                    db.Entry(exam).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Account Activated Succesfully.";
                    return RedirectToAction("Exams");
                }
                TempData["ErrorMessage"] = "Account Not Activated";
                return RedirectToAction("Exams");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InactiveExam(int? inactiveId)
        {
            if (inactiveId == null)
            {
                TempData["ErrorMessage"] = "Account Not Inactivated";
                return RedirectToAction("Exams");
            }
            else
            {
                Exam programmes = await db.Exams.Where(c => c.Id == inactiveId).FirstOrDefaultAsync();
                if (programmes != null)
                {
                    programmes.IsActive = 0;
                    db.Entry(programmes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Account Inactivated Succesfully.";
                    return RedirectToAction("Exams");
                }
                TempData["ErrorMessage"] = "Account Not Inactivated";
                return RedirectToAction("Exams");
            }
        }

        public ActionResult DeleteExam()
        {
            return View();
        }
    }
}