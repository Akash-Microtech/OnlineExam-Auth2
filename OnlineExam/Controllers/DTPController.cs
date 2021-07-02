using OnlineExam.Authentication;
using OnlineExam.DbContext;
using OnlineExam.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineExam.Controllers
{
    [CustomAuthorize(Roles = "DTP")]
    public class DTPController : Controller
    {
        private readonly Exam_DBEntities db = new Exam_DBEntities();

        // GET: DTP
        public async Task<ActionResult> Index()
        {
            string dtpRegId = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().UniqueID;
            var data = await db.DataEntry_Registration.Where(d => d.DtpRegId == dtpRegId).FirstOrDefaultAsync();

            if(data != null)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("QaAsAccount");
            }            
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

            int id = db.Users.Where(u=>u.UserName == User.Identity.Name).FirstOrDefault().Id;
            var list = db.GetAllDtpQusAnsByUserId(id).ToList();
            return View(list);
        }

        public async Task<ActionResult> QaAsAccount(int? id)
        {
            if (id == null)
            {
                string dtpRegId = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().UniqueID;
                DtpAccountViewModel dtpView = new DtpAccountViewModel()
                {
                    DtpRegId = dtpRegId
                };
                return View(dtpView);
            }
            else
            {
                var data = await db.DataEntry_Registration.Where(d => d.Id == id).FirstOrDefaultAsync();
                DtpAccountViewModel dtpView = new DtpAccountViewModel()
                {
                    Id = data.Id,
                    DtpRegId = data.DtpRegId,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Mobile = data.Mobile,
                    WhatsApp = data.WhatsApp,
                    Email = data.Email,
                    Location = data.Location,
                    Place = data.Place
                };

                return View(dtpView);

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> QaAsAccount(DtpAccountViewModel dtpViewModel)
        {
            if (dtpViewModel.Id != null)
            {
                DataEntry_Registration data = db.DataEntry_Registration.Find(dtpViewModel.Id);
                if (data != null)
                {
                    if (ModelState.IsValid)
                    {
                        data.FirstName = dtpViewModel.FirstName;
                        data.LastName = dtpViewModel.LastName;
                        data.Email = dtpViewModel.Email;
                        data.Location = dtpViewModel.Location;
                        data.Place = dtpViewModel.Place;
                        data.WhatsApp = dtpViewModel.WhatsApp;
                        data.Mobile = dtpViewModel.Mobile;
                        data.ModifiedDateTime = DateTime.Now;
                        data.ModifiedBy = dtpViewModel.CuserId; 
                        db.Entry(data).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Profile");
                    }

                }
                ViewBag.ErrorMessage = "Please fill in all the required fields";
                return View(dtpViewModel);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ErrorMessage = "Please fill in all the required fields";
                    return View(dtpViewModel);
                }



                DataEntry_Registration dataEntry_Registration = new DataEntry_Registration
                {
                    DtpRegId = dtpViewModel.DtpRegId,
                    FirstName = dtpViewModel.FirstName,
                    LastName = dtpViewModel.LastName,
                    Email = dtpViewModel.Email,
                    Location = dtpViewModel.Location,
                    Place = dtpViewModel.Place,
                    WhatsApp = dtpViewModel.WhatsApp,
                    Mobile = dtpViewModel.Mobile,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    DeletedDateTime = DateTime.Now,
                    CreatedBy = dtpViewModel.CuserId
                };
                db.DataEntry_Registration.Add(dataEntry_Registration);
                await db.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }

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

            }
            else
            {
                var data = await db.DataEntry_QuestionBank.Where(d => d.Id == id).FirstOrDefaultAsync();
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
            return View();
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
            if(dtpQAView.Questions != null || fileQus.FileName != null)
            {
                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                var uploadPath = "";
                var FileExt = Path.GetExtension(fileQus.FileName);
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
                    if(fileQus.FileName != null)
                    {
                        if (allowedExtensions.Contains(FileExt))
                        {
                            string dtpRegId = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().UniqueID;
                            string myfile = dtpRegId + "_" + upFileName + FileExt;
                            uploadPath = Path.Combine(Server.MapPath("~/Uploads/QuestionDtp/"), myfile);
                            dtpQAView.Photo = "../../Uploads/QuestionDtp/" + myfile;
                        }
                    }                    

                    DataEntry_QuestionBank data = db.DataEntry_QuestionBank.Find(dtpQAView.Id);
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
                        data.ModifiedBy = dtpQAView.ChapterId;
                        data.PgmId = dtpQAView.PgmId;
                        data.SubPgmId = dtpQAView.SubPgmId;
                        data.CourseId = dtpQAView.CourseId;
                        data.SubjectId = dtpQAView.SubjectId;
                        data.ChapterId = dtpQAView.ChapterId;
                        data.Photo = dtpQAView.Photo;
                        db.Entry(data).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        fileQus.SaveAs(uploadPath);

                        TempData["StatusMessage"] = "Questions Answers Edited Succesfully.";
                        return RedirectToAction("QaAsList");
                    }

                    ViewBag.ErrorMessage = "Please fill in all the required fields";
                    return View(dtpQAView);
                }
                else
                {
                    if (allowedExtensions.Contains(FileExt))
                    {
                        string dtpRegId = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().UniqueID;
                        string myfile = dtpRegId + "_" + upFileName + FileExt;
                        uploadPath = Path.Combine(Server.MapPath("~/Uploads/QuestionDtp"), myfile);
                        dtpQAView.Photo = "../../Uploads/QuestionDtp/" + myfile;
                    }

                    DataEntry_QuestionBank data = new DataEntry_QuestionBank
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
                        CreatedBy = dtpQAView.ChapterId,
                        PgmId = dtpQAView.PgmId,
                        SubPgmId = dtpQAView.SubPgmId,
                        CourseId = dtpQAView.CourseId,
                        SubjectId = dtpQAView.SubjectId,
                        ChapterId = dtpQAView.ChapterId,
                        Photo = dtpQAView.Photo
                    };

                    db.DataEntry_QuestionBank.Add(data);
                    await db.SaveChangesAsync();
                    fileQus.SaveAs(uploadPath);

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

    }
}