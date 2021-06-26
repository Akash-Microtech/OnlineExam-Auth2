using OnlineExam.DbContext;
using OnlineExam.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineExam.Controllers
{
    public class RegistrationController : Controller
    {

        private readonly Exam_DBEntities db = new Exam_DBEntities();

        // GET: Registration
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Teacher()
        {

            if (TempData["StatusMessage"] != null)
            {
                ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
                ViewBag.ApplicationName = TempData["ApplicationName"].ToString();
            }

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Teacher(TeacherRegViewModel teacherRegView)
        {
            if (ModelState.IsValid)
            {
                Teachers_Registration teachers_Registration = new Teachers_Registration
                {
                    FirstName = teacherRegView.FirstName,
                    MiddleName = teacherRegView.MiddleName,
                    LastName = teacherRegView.LastName,
                    WhatsApp = teacherRegView.WhatsApp,
                    PrimarySubject = teacherRegView.PrimarySubject,
                    SecondarySubject = teacherRegView.SecondarySubject,
                    Location = teacherRegView.Location,
                    Street = teacherRegView.Street,
                    Address = teacherRegView.Address,
                    PO = teacherRegView.PO,
                    District = teacherRegView.District,
                    State = teacherRegView.State,
                    Country = teacherRegView.Country,
                    Time = String.Join(",", teacherRegView.Time),
                    Weekends = teacherRegView.Weekends,
                    Weekdays = teacherRegView.Weekdays,
                    StudentGrade = String.Join(",", teacherRegView.StudentGrade),
                    DeletedDateTime = DateTime.Now
                };

                db.Teachers_Registration.Add(teachers_Registration);
                await db.SaveChangesAsync();

                TempData["ApplicationName"] = teacherRegView.FirstName + " " + teacherRegView.MiddleName + " " + teacherRegView.LastName;
                TempData["StatusMessage"] = "Thank You for your registration we will reach you soon.";
                return RedirectToAction("TeacherRegisterSuccess");

            }

            ViewBag.ErrorMessage = "Please fill in all the required fields";
            return View(teacherRegView);

        }

        public ActionResult TeacherRegisterSuccess()
        {
            return RedirectToAction("TeacherRegister");
        }

        public ActionResult Student()
        {
            ViewBag.ClassID = new SelectList(db.Classes, "Id", "Name");
            ViewBag.ProgramID = new SelectList(db.Programmes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Student(StudentRegistrationViewModel model, HttpPostedFileBase file, HttpPostedFileBase filefr, HttpPostedFileBase filemr)
        {

            ////IMAGE OF STUDENT
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };

            var pathphoto = "";
            var pathfrsign = "";
            var pathmrsign = "";



            var fileName = Path.GetFileName(file.FileName);
            var filefrsign = Path.GetFileName(filefr.FileName);
            var filemrsign = Path.GetFileName(filemr.FileName);


            var ext = Path.GetExtension(file.FileName);
            var extfr = Path.GetExtension(filefr.FileName);
            var extmr = Path.GetExtension(filemr.FileName);

            if (allowedExtensions.Contains(ext))
            {
                string name = Path.GetFileNameWithoutExtension(fileName);
                string myfile = name + "_" + ext;

                pathphoto = Path.Combine(Server.MapPath("~/Uploads/StudentRegistration/Image/"), myfile);
            }

            if (allowedExtensions.Contains(extfr))
            {
                string name = Path.GetFileNameWithoutExtension(filefrsign);
                string myfile = name + "_" + extfr;

                pathfrsign = Path.Combine(Server.MapPath("~/Uploads/StudentRegistration/Sign/"), myfile);
            }

            if (allowedExtensions.Contains(extmr))
            {
                string name = Path.GetFileNameWithoutExtension(filemrsign);
                string myfile = name + "_" + extmr;

                pathmrsign = Path.Combine(Server.MapPath("~/Uploads/StudentRegistration/Sign/"), myfile);
            }




            //BASIC REGISTRATION//

            var StudentRegistration = new Student_Registration()
            {

                UserId = 0,  // model.UserId,
                GroupId = 0,
                BatchId = 0,
                ExamAttendingYear = model.ExamAttendingYear,
                PreferredDay = model.PreferredDay,
                ApplnDate = model.ApplnDate,
                AcademicYear = model.AcademicYear,
                AdmissionTestDate = model.AdmissionTestDate,
                PreferredTime = model.PreferredTime,
                WhatsappNo = model.WhatsappNo,
                DOB = model.DOB,
                Gender = model.Gender,
                Religion = model.Religion,
                Caste = model.Caste,
                Community = model.Community,
                BloodGroup = model.BloodGroup,
                Nationalty = model.Nationalty,
                PresentAddress = model.PresentAddress,
                Area = model.Area,
                Location = model.Location,
                State = model.State,
                District = model.District,
                Pincode = model.Pincode,
                QuickContNo = model.QuickContNo,
                Photo = pathphoto.ToString(),
                QuickWhatsApp = model.QuickWhatsApp,
                PgmId = model.PgmId,
                ClassId = model.ClassId,
                CourseId = model.CourseId,
                SubPgmId = model.SubPgmId
            };


            //PARENT REGISTRATION//

            var StudentParent = new Student_Parent()
            {
                StudRegId = 3,
                FrName = model.FrName,
                FrOcc = model.FrOcc,
                FrMobNo = model.FrMobNo,
                FrMailid = model.FrMailId,
                FrDistrict = model.FrDistrict,
                FrSign = pathfrsign.ToString(),
                FrState = model.FrState,
                FrWhatsapp = model.FrWhatsApp,
                MrName = model.MrName,
                MrOcc = model.MrOcc,
                MrMobNo = model.MrMobNo,
                MrMailid = model.MrMailId,
                MrDistrict = model.MrDistrict,
                MrSign = pathmrsign.ToString(),
                MrState = model.MrState,
                MrWhatsapp = model.MrWhatsApp,
            };

            //HOME DETAILS//

            var StudentHomeCountryDetails = new Student_HomeCountryDetails()
            {
                StudRegId = 3,
                Address1 = model.Address1,
                Address2 = model.Address2,
                AreaHome = model.AreaHome,
                PincodeHome = model.PincodeHome,
                QuickContact = model.QuickContact,
                LocationHome = model.LocationHome,
                StateHome = model.StateHome,
                EmaiId = model.EmaiId,
                QuickHomeWhatsapp = model.QuickHomeWhatsapp,

            };


            //ACADEMIC//
            var StudentAcademicPerformance = new Student_AcademicPerformance()
            {
                StudRegId = 3,
                Class = model.Class,
                PassYear = model.PassYear,
                SchoolAddress = model.SchoolAddress,
                RegNo = model.RegNo,
                Board = model.Board,
                PhyMark = model.PhyMark,
                ChemMark = model.ChemMark,
                BiologyMark = model.BiologyMark,
                MathsMark = model.MathsMark,
                PercOfMark = model.PercOfMark,

            };

            //PREV ENTRANCE//
            var StudentPreviousEntrance = new Student_PreviousEntrance()
            {
                StudRegId = 3,
                PrevEntranceExamName = model.PrevEntranceExamName,
                RollNo = model.RollNo,
                AttemptedYear = model.AttemptedYear,
                Mark = model.Mark,
                Rank = model.Rank,
                NoOfAttempts = model.NoOfAttempts,


            };

            //---------------------//
            if (ModelState.IsValid)
            {
                db.Student_Registration.Add(StudentRegistration);
                db.Student_Parent.Add(StudentParent);
                db.Student_HomeCountryDetails.Add(StudentHomeCountryDetails);
                db.Student_PreviousEntrance.Add(StudentPreviousEntrance);
                db.Student_AcademicPerformance.Add(StudentAcademicPerformance);

                db.SaveChangesAsync();
                file.SaveAs(pathphoto);
                filefr.SaveAs(pathfrsign);
                filefr.SaveAs(pathmrsign);
                ViewBag.StatusMessage = "Registration Succesfully Completed";
            }

            return View(model);
        }
    }
}