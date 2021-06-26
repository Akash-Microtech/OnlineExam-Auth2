using OnlineExam.DbContext;
using OnlineExam.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly Exam_DBEntities db = new Exam_DBEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult StudentRegister()
        {
            return View();
        }

        public async Task<ActionResult> TeacherRegister(int? id)
        {
            if (id == null)
            {
                var sub = new SelectList(db.Subjects.Where(p => p.IsDeleted == 0), "Id", "Name");
                var clas = new SelectList(db.Classes.Where(p => p.IsDeleted == 0), "Id", "Name");

                ViewBag.PrimarySubject = sub;
                ViewBag.SecondarySubject = sub;
                ViewBag.classes = clas;

                return View();
            }
            else
            {
                var data = await db.Teachers_Registration.Where(d => d.Id == id).FirstOrDefaultAsync();
                TeacherRegViewModel teacher = new TeacherRegViewModel()
                {
                    Id = data.Id,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Address = data.Address,
                    WhatsApp = data.WhatsApp,
                    District = data.District,
                    Location = data.Location,
                    Street = data.Street,
                    PO = data.PO,
                    State = data.State
                };
                var sub = new SelectList(db.Subjects.Where(p => p.IsDeleted == 0), "Id", "Name", data.PrimarySubject);
                ViewBag.PrimarySubject = sub;
                ViewBag.SecondarySubject = sub;
                return View(teacher);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TeacherRegister(TeacherRegViewModel teacherRegView)
        {
            if (ModelState.IsValid)
            {
                if (teacherRegView.Id != null)
                {
                    Teachers_Registration data = db.Teachers_Registration.Find(teacherRegView.Id);
                    if (data != null)
                    {
                        if (ModelState.IsValid)
                        {
                            data.FirstName = teacherRegView.FirstName;
                            data.LastName = teacherRegView.LastName;
                            data.WhatsApp = teacherRegView.WhatsApp;
                            data.PrimarySubject = teacherRegView.PrimarySubject;
                            data.SecondarySubject = teacherRegView.SecondarySubject;
                            data.Location = teacherRegView.Location;
                            data.Street = teacherRegView.Street;
                            data.Address = teacherRegView.Address;
                            data.PO = teacherRegView.PO;
                            data.District = teacherRegView.District;
                            data.State = teacherRegView.State;
                            db.Entry(data).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }

                    }
                    return View(teacherRegView);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Teachers_Registration teachers_Registration = new Teachers_Registration();
                        teachers_Registration.FirstName = teacherRegView.FirstName;
                        teachers_Registration.LastName = teacherRegView.LastName;
                        teachers_Registration.WhatsApp = teacherRegView.WhatsApp;
                        teachers_Registration.PrimarySubject = teacherRegView.PrimarySubject;
                        teachers_Registration.SecondarySubject = teacherRegView.SecondarySubject;
                        teachers_Registration.Location = teacherRegView.Location;
                        teachers_Registration.Street = teacherRegView.Street;
                        teachers_Registration.Address = teacherRegView.Address;
                        teachers_Registration.PO = teacherRegView.PO;
                        teachers_Registration.District = teacherRegView.District;
                        teachers_Registration.State = teacherRegView.State;
                        teachers_Registration.DeletedDateTime = DateTime.Now;
                        db.Teachers_Registration.Add(teachers_Registration);
                        await db.SaveChangesAsync();


                        return RedirectToAction("Index");
                    }
                }

            }

            var sub = new SelectList(db.Subjects.Where(p => p.IsDeleted == 0), "Id", "Name");
            ViewBag.PrimarySubject = sub;
            ViewBag.SecondarySubject = sub;
            return View(teacherRegView);
            
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}