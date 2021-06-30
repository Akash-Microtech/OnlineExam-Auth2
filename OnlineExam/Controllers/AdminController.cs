﻿using OnlineExam.Authentication;
using OnlineExam.DbContext;
using OnlineExam.Models;
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
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Exam_DBEntities db = new Exam_DBEntities();
        private readonly Exam_DBrole DbRole = new Exam_DBrole();

        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public new ActionResult Profile()
        {
            return View();
        }

        public async Task<ActionResult> Role(int? id)
        {
            var allRoleData = await db.Roles.ToListAsync();

            if (id != null)
            {
                var oneRoleData = await db.Roles.Where(r => r.RoleId == id).FirstOrDefaultAsync();
                RoleViewModel roleView = new RoleViewModel()
                {
                    RoleName = oneRoleData.RoleName,
                    RoleId = oneRoleData.RoleId,
                    Roles = allRoleData
                };

                return View(roleView);
            }
            else
            {
                RoleViewModel roleView = new RoleViewModel()
                {
                    Roles = allRoleData
                };
                if (TempData["StatusMessage"] != null)
                {
                    ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
                }
                return View(roleView);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Role(RoleViewModel role)
        {
            var allRoleData = await db.Roles.ToListAsync();

            if (role.RoleId != null)
            {
                role.Roles = allRoleData;
                ViewBag.ErrorMessage = "Please Enter Role Name";
                return View(role);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var exists = await db.Roles.Where(r => r.RoleName == role.RoleName).FirstOrDefaultAsync();
                    if (exists != null)
                    {
                        role.Roles = allRoleData;
                        ViewBag.ErrorMessage = "Role Name already exists";
                        return View(role);
                    }
                    else
                    {
                        Role userRole = new Role()
                        {
                            RoleName = role.RoleName
                        };

                        db.Roles.Add(userRole);
                        await db.SaveChangesAsync();
                        TempData["StatusMessage"] = "Role Created Succesfully";
                        return RedirectToAction("Role");
                    }

                }

                role.Roles = allRoleData;
                ViewBag.ErrorMessage = "Please Enter Role Name";
                return View(role);
            }
        }

        public async Task<ActionResult> UserAccounts()
        {
            if (TempData["StatusMessage"] != null)
            {
                ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
            }

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
            }

            var users = db.Users.Include(u => u.Roles);
            return View(await users.Where(u => u.IsDeleted == 0).ToListAsync());
        }

        public async Task<ActionResult> UserAccount(int? id)
        {
            if(id != null)
            {
                User data = await db.Users.Where(d => d.Id == id).FirstOrDefaultAsync();
                if (data != null)
                {
                    AccountViewModel user = new AccountViewModel
                    {
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        Email = data.Email,
                        MobileNo = data.MobileNo,
                        UserName = data.UserName,
                        RoleId = data.RoleId,
                        Password = data.Password,
                        ConfirmPassword = data.Password,
                        Roles = await db.Roles.ToListAsync()
                    };
                    return View(user);
                }                
            }

            AccountViewModel model = new AccountViewModel
            {
                Roles = await db.Roles.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserAccount(AccountViewModel model)
        {
            if (model.Id != null)
            {
                User data = db.Users.Where(d => d.Id == model.Id).FirstOrDefault();
                if(data != null)
                {
                    if(data.Email != model.Email)
                    {
                        var check = db.Users.Where(d => d.Email == model.Email).FirstOrDefault();

                        if (check != null)
                        {
                            ViewBag.ErrorMessage = "Email already exists";
                            model.Roles = await db.Roles.ToListAsync();
                            return View(model);
                        }
                    }

                    if (data.UserName != model.UserName)
                    {
                        var check = db.Users.Where(d => d.UserName == model.UserName).FirstOrDefault();

                        if (check != null)
                        {
                            ViewBag.ErrorMessage = "Username already exists";
                            model.Roles = await db.Roles.ToListAsync();
                            return View(model);
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        data.FirstName = model.FirstName;
                        data.LastName = model.LastName;
                        data.Email = model.Email;
                        data.UserName = model.UserName;
                        data.Password = model.Password;
                        data.RoleId = model.RoleId;

                        db.Entry(data).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        AddRole(model);

                        TempData["StatusMessage"] = "Account Edited Succesfully.";
                        return RedirectToAction("UserAccounts");
                    }


                }

                model.Roles = await db.Roles.ToListAsync();
                ViewBag.ErrorMessage = "Please fill in all the required fields";
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var data = db.Users.Where(d => d.Email == model.Email || d.UserName == model.UserName).FirstOrDefault();

                    if (data != null)
                    {
                        ViewBag.ErrorMessage = "Email or Username already exists";
                        model.Roles = await db.Roles.ToListAsync();
                        return View(model);
                    }

                    string alpha;
                    if (model.RoleId == 1)
                    {
                        alpha = "ECA";
                    }
                    else if (model.RoleId == 2)
                    {
                        alpha = "ECS";
                    }
                    else if (model.RoleId == 3)
                    {
                        alpha = "ECT";
                    }
                    else if (model.RoleId == 4)
                    {
                        alpha = "ECD";
                    }
                    else
                    {
                        alpha = "ECC";
                    }

                    Random random = new Random();
                    int unique = random.Next(10000, 99999);
                    int y = DateTime.Now.Year;
                    int m = DateTime.Now.Month;
                    var uniqueID = alpha + y + m + unique;

                    var user = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        MobileNo = model.MobileNo,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Password = model.Password,
                        RoleId = model.RoleId,
                        UniqueID = uniqueID,
                        CreatedDate = DateTime.Now,
                        DeletedDate = DateTime.Now,
                        CreatedBy = model.CuserId,
                        ActivationCode = Guid.NewGuid().ToString()
                };                    

                    if (ModelState.IsValid)
                    {
                        db.Users.Add(user);
                        await db.SaveChangesAsync();

                        AddRole(model);

                        TempData["StatusMessage"] = "Account Created Succesfully";
                        return RedirectToAction("UserAccounts");
                    }

                    return View(model); ;
                }

                model.Roles = await db.Roles.ToListAsync();
                ViewBag.ErrorMessage = "Please fill in all the required fields";
                return View(model);
            }
        }

        public ViewResult AddRole(AccountViewModel model)
        {

            if (model.Id != null)
            {
                UserRole userR = (UserRole)DbRole.UserRoles.Where(u => u.UserId == model.Id).FirstOrDefault();
                DbRole.UserRoles.Remove(userR);
                DbRole.SaveChanges();

                UserRole userRole = new UserRole()
                {
                    RoleId = model.RoleId,
                    UserId = (int)model.Id
                };
                DbRole.UserRoles.Add(userRole);
                DbRole.SaveChangesAsync();
            }
            else
            {
                var user = db.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();
                UserRole userRole = new UserRole()
                {
                    RoleId = model.RoleId,
                    UserId = user.Id
                };

                DbRole.UserRoles.Add(userRole);
                DbRole.SaveChangesAsync();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActiveAccount(int? activeId)
        {
            if (activeId == null)
            {
                TempData["ErrorMessage"] = "Account Not Activated";
                return RedirectToAction("UserAccounts");
            }
            else
            {
                User programmes = await db.Users.Where(c => c.Id == activeId).FirstOrDefaultAsync();
                if (programmes != null)
                {
                    programmes.IsActive = 1;
                    db.Entry(programmes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Account Activated Succesfully.";
                    return RedirectToAction("UserAccounts");
                }
                TempData["ErrorMessage"] = "Account Not Activated";
                return RedirectToAction("UserAccounts");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InactiveAccount(int? inactiveId)
        {
            if (inactiveId == null)
            {
                TempData["ErrorMessage"] = "Account Not Inactivated";
                return RedirectToAction("UserAccounts");
            }
            else
            {
                User programmes = await db.Users.Where(c => c.Id == inactiveId).FirstOrDefaultAsync();
                if (programmes != null)
                {
                    programmes.IsActive = 0;
                    db.Entry(programmes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Account Inactivated Succesfully.";
                    return RedirectToAction("UserAccounts");
                }
                TempData["ErrorMessage"] = "Account Not Inactivated";
                return RedirectToAction("UserAccounts");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAccount(int? deleteUserId)
        {
            if (deleteUserId == null)
            {
                TempData["ErrorMessage"] = "Account Not Deleted";
                return RedirectToAction("UserAccounts");
            }
            else
            {
                User programmes = await db.Users.Where(c => c.Id == deleteUserId).FirstOrDefaultAsync();
                if (programmes != null)
                {
                    programmes.IsDeleted = 1;
                    db.Entry(programmes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Account Deleted Succesfully.";
                    return RedirectToAction("UserAccounts");
                }
                TempData["ErrorMessage"] = "Account Not Deleted";
                return RedirectToAction("UserAccounts");
            }
        }

        public async Task<ActionResult> Programmes(int? id)
        {

            if (id == null)
            {
                ProgrammeViewModel viewModel = new ProgrammeViewModel()
                {
                    programmes = await db.Programmes.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                if (TempData["StatusMessage"] != null)
                {
                    ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
                }

                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                return View(viewModel);
            }
            else
            {
                var data = await db.Programmes.Where(d => d.Id == id).FirstOrDefaultAsync();
                ProgrammeViewModel viewModel = new ProgrammeViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate,
                    programmes = await db.Programmes.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Programmes(ProgrammeViewModel programmes)
        {
            var userId = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;

            if (programmes.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Programme model = await db.Programmes.Where(c => c.Id == programmes.Id).FirstOrDefaultAsync();
                    model.Name = programmes.Name;
                    model.ModifiedTime = DateTime.Now;
                    model.ModifiedBy = userId;

                    db.Entry(model).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Programme Edited Succesfully.";
                    return RedirectToAction("ProgrammesEdit");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                programmes.programmes = await db.Programmes.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(programmes);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Programme model = new Programme()
                    {
                        Name = programmes.Name,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        DeletedDate = DateTime.Now
                    };

                    db.Programmes.Add(model);
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Programme Created Succesfully.";
                    return RedirectToAction("Programmes");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                programmes.programmes = await db.Programmes.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(programmes);
            }
        }

        public ActionResult ProgrammesEdit()
        {
            return RedirectToAction("Programmes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProgrammeDelete(int? programmeId)
        {
            if (programmeId == null)
            {
                TempData["ErrorMessage"] = "Account Not Deleted";
                return RedirectToAction("Programmes");
            }
            else
            {
                Programme programmes = await db.Programmes.Where(c => c.Id == programmeId).FirstOrDefaultAsync();

                if (programmes != null)
                {
                    programmes.IsDeleted = 1;
                    programmes.DeletedDate = DateTime.Now;
                    db.Entry(programmes).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Account Deleted Succesfully.";
                    return RedirectToAction("Programmes");
                }

                TempData["ErrorMessage"] = "Account Not Deleted";
                return RedirectToAction("Programmes");
            }
        }

        public async Task<ActionResult> SubProgramme(int? id)
        {
            var data1 = db.SubPrograms.Include(d => d.Programme);
            var subProg = await data1.Where(p => p.IsDeleted == 0).ToListAsync();

            if (id == null)
            {
                SubProgramViewModel viewModel = new SubProgramViewModel()
                {
                    SubPrograms = subProg,
                    Programmes = await db.Programmes.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                if (TempData["StatusMessage"] != null)
                {
                    ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
                }

                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                return View(viewModel);
            }
            else
            {
                var data = await db.SubPrograms.Where(d => d.Id == id).FirstOrDefaultAsync();
                SubProgramViewModel viewModel = new SubProgramViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate,
                    PgmId = data.PgmId,
                    SubPrograms = subProg,
                    Programmes = await db.Programmes.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubProgramme(SubProgramViewModel subProgram)
        {
            var userId = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;

            if (subProgram.Id != null)
            {
                if (ModelState.IsValid)
                {
                    SubProgram model = await db.SubPrograms.Where(c => c.Id == subProgram.Id).FirstOrDefaultAsync();
                    model.Name = subProgram.Name;
                    model.ModifiedTime = DateTime.Now;
                    model.ModifiedBy = userId;
                    model.PgmId = subProgram.PgmId;

                    db.Entry(model).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Sub Programme Edited Succesfully.";
                    return RedirectToAction("EditSubProgramme");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";

                var data = db.SubPrograms.Include(d => d.Programme);
                subProgram.SubPrograms = await data.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(subProgram);
            }
            else
            {
                SubProgram model = new SubProgram()
                {
                    Name = subProgram.Name,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifiedTime = DateTime.Now,
                    DeletedDate = DateTime.Now,
                    PgmId = subProgram.PgmId
                };

                if (ModelState.IsValid)
                {
                    db.SubPrograms.Add(model);
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Sub Programme Created Succesfully.";
                    return RedirectToAction("SubProgramme");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields"; 
                var data = db.SubPrograms.Include(d => d.Programme);
                subProgram.SubPrograms = await data.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(subProgram);
            }
        }

        public ActionResult EditSubProgramme()
        {
            return RedirectToAction("SubProgramme");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubProgrammeDelete(int? subProgrammeId)
        {
            if (subProgrammeId == null)
            {
                TempData["ErrorMessage"] = "sub Programme Not Deleted";
                return RedirectToAction("SubProgramme");
            }
            else
            {
                SubProgram subProgramme = await db.SubPrograms.Where(c => c.Id == subProgrammeId).FirstOrDefaultAsync();

                if (subProgramme != null)
                {
                    subProgramme.IsDeleted = 1;
                    db.Entry(subProgramme).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "sub Programme Deleted Succesfully.";
                    return RedirectToAction("SubProgramme");
                }

                TempData["ErrorMessage"] = "sub Programme Not Deleted";
                return RedirectToAction("SubProgramme");
            }
        }


        public async Task<ActionResult> StdClass(int? id)
        {

            if (id == null)
            {
                StdClassViewModel viewModel = new StdClassViewModel()
                {
                    Classes = await db.Classes.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                if (TempData["StatusMessage"] != null)
                {
                    ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
                }

                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                return View(viewModel);
            }
            else
            {
                var data = await db.Classes.Where(d => d.Id == id).FirstOrDefaultAsync();
                StdClassViewModel viewModel = new StdClassViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate,
                    Classes = await db.Classes.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StdClass(StdClassViewModel stdClassView)
        {
            var userId = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;

            if (stdClassView.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Class model = await db.Classes.Where(c => c.Id == stdClassView.Id).FirstOrDefaultAsync();
                    model.Name = stdClassView.Name;
                    model.ModifiedTime = DateTime.Now;
                    model.ModifiedBy = userId;

                    db.Entry(model).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Class Edited Succesfully.";
                    return RedirectToAction("StdClassEdit");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                stdClassView.Classes = await db.Classes.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(stdClassView);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Class model = new Class()
                    {
                        Name = stdClassView.Name,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        DeletedDate = DateTime.Now
                    };

                    db.Classes.Add(model);
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Class Created Succesfully.";
                    return RedirectToAction("StdClass");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                stdClassView.Classes = await db.Classes.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(stdClassView);
            }
        }

        public ActionResult StdClassEdit()
        {
            return RedirectToAction("StdClass");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StdClassDelete(int? classId)
        {
            if (classId == null)
            {
                TempData["ErrorMessage"] = "Class Not Deleted";
                return RedirectToAction("StdClass");
            }
            else
            {
                Class stdClass = await db.Classes.Where(c => c.Id == classId).FirstOrDefaultAsync();

                if (classId != null)
                {
                    stdClass.IsDeleted = 1;
                    stdClass.DeletedDate = DateTime.Now;
                    db.Entry(stdClass).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Class Deleted Succesfully.";
                    return RedirectToAction("StdClass");
                }

                TempData["ErrorMessage"] = "Class Not Deleted";
                return RedirectToAction("StdClass");
            }
        }

        public async Task<ActionResult> Course(int? id)
        {
            var subProg = await db.Courses.Where(p => p.IsDeleted == 0).ToListAsync();


            if (id == null)
            {
                CourseViewModel viewModel = new CourseViewModel()
                {
                    Courses = subProg,
                    Classes = await db.Classes.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                if (TempData["StatusMessage"] != null)
                {
                    ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
                }

                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                return View(viewModel);
            }
            else
            {
                var data = await db.Courses.Where(d => d.Id == id).FirstOrDefaultAsync();
                CourseViewModel viewModel = new CourseViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate,
                    ClassId = data.ClassId,
                    Courses = subProg,
                    Classes = await db.Classes.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Course(CourseViewModel courseView)
        {
            var userId = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;

            if (courseView.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Cours model = await db.Courses.Where(c => c.Id == courseView.Id).FirstOrDefaultAsync();
                    model.Name = courseView.Name;
                    model.ModifiedTime = DateTime.Now;
                    model.ModifiedBy = userId;
                    model.ClassId = courseView.ClassId;

                    db.Entry(model).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Course Edited Succesfully.";
                    return RedirectToAction("EditCourse");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                courseView.Courses = await db.Courses.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(courseView);
            }
            else
            {
                Cours model = new Cours()
                {
                    Name = courseView.Name,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifiedTime = DateTime.Now,
                    DeletedDate = DateTime.Now,
                    ClassId = courseView.ClassId
                };

                if (ModelState.IsValid)
                {
                    db.Courses.Add(model);
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Course Created Succesfully.";
                    return RedirectToAction("Course");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                courseView.Courses = await db.Courses.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(courseView);
            }
        }

        public ActionResult EditCourse()
        {
            return RedirectToAction("Course");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CourseDelete(int? CourseId)
        {
            if (CourseId == null)
            {
                TempData["ErrorMessage"] = "Course Not Deleted";
                return RedirectToAction("Course");
            }
            else
            {
                Cours course = await db.Courses.Where(c => c.Id == CourseId).FirstOrDefaultAsync();

                if (course != null)
                {
                    course.IsDeleted = 1;
                    db.Entry(course).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Course Deleted Succesfully.";
                    return RedirectToAction("Course");
                }

                TempData["ErrorMessage"] = "Course Not Deleted";
                return RedirectToAction("Course");
            }
        }

        public async Task<ActionResult> Subjects(int? id)
        {

            if (id == null)
            {
                SubjectViewModel viewModel = new SubjectViewModel()
                {
                    Subjects = await db.Subjects.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                if (TempData["StatusMessage"] != null)
                {
                    ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
                }

                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                return View(viewModel);
            }
            else
            {
                var data = await db.Subjects.Where(d => d.Id == id).FirstOrDefaultAsync();
                SubjectViewModel viewModel = new SubjectViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate,
                    Subjects = await db.Subjects.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Subjects(SubjectViewModel subjectView)
        {
            var userId = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;

            if (subjectView.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Subject model = await db.Subjects.Where(c => c.Id == subjectView.Id).FirstOrDefaultAsync();
                    model.Name = subjectView.Name;
                    model.ModifiedTime = DateTime.Now;
                    model.ModifiedBy = userId;

                    db.Entry(model).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Subject Edited Succesfully.";
                    return RedirectToAction("SubjectEdit");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                subjectView.Subjects = await db.Subjects.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(subjectView);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Subject model = new Subject()
                    {
                        Name = subjectView.Name,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        DeletedDate = DateTime.Now
                    };

                    db.Subjects.Add(model);
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Subject Created Succesfully.";
                    return RedirectToAction("Subjects");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                subjectView.Subjects = await db.Subjects.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(subjectView);
            }
        }

        public ActionResult SubjectEdit()
        {
            return RedirectToAction("Subjects");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubjectDelete(int? subjectId)
        {
            if (subjectId == null)
            {
                TempData["ErrorMessage"] = "Subject Not Deleted";
                return RedirectToAction("Subjects");
            }
            else
            {
                Subject subject = await db.Subjects.Where(c => c.Id == subjectId).FirstOrDefaultAsync();

                if (subjectId != null)
                {
                    subject.IsDeleted = 1;
                    subject.DeletedDate = DateTime.Now;
                    db.Entry(subject).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Subject Deleted Succesfully.";
                    return RedirectToAction("Subjects");
                }

                TempData["ErrorMessage"] = "Subject Not Deleted";
                return RedirectToAction("Subjects");
            }
        }

        public async Task<ActionResult> Chapters(int? id)
        {
            var Subjects =  db.Subjects.Where(p => p.IsDeleted == 0);
            var chapter = db.Chapters.Include(c=>c.Subject);

            if (id == null)
            {

                ChapterViewModel viewModel = new ChapterViewModel()
                {
                    Subjects = await Subjects.ToListAsync(),
                    Chapters = await chapter.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                if (TempData["StatusMessage"] != null)
                {
                    ViewBag.StatusMessage = TempData["StatusMessage"].ToString();
                }

                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                return View(viewModel);
            }
            else
            {
                var data = await db.Chapters.Where(d => d.Id == id).FirstOrDefaultAsync();
                ChapterViewModel viewModel = new ChapterViewModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    IsDeleted = data.IsDeleted,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedTime = data.ModifiedTime,
                    DeletedDate = data.DeletedDate,
                    SubId = data.SubId,
                    Subjects = await Subjects.ToListAsync(),
                    Chapters = await chapter.Where(p => p.IsDeleted == 0).ToListAsync()
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Chapters(ChapterViewModel chapter)
        {
            var userId = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
            var chapterList = db.Chapters.Include(c => c.Subject);

            if (chapter.Id != null)
            {
                if (ModelState.IsValid)
                {
                    Chapter model = await db.Chapters.Where(c => c.Id == chapter.Id).FirstOrDefaultAsync();
                    model.Name = chapter.Name;
                    model.ModifiedTime = DateTime.Now;
                    model.ModifiedBy = userId;
                    model.SubId = chapter.SubId;

                    db.Entry(model).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Chapter Edited Succesfully.";
                    return RedirectToAction("EditCourse");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                chapter.Chapters = await chapterList.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(chapter);
            }
            else
            {
                Chapter model = new Chapter()
                {
                    Name = chapter.Name,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifiedTime = DateTime.Now,
                    DeletedDate = DateTime.Now,
                    SubId = chapter.SubId
                };

                if (ModelState.IsValid)
                {
                    db.Chapters.Add(model);
                    await db.SaveChangesAsync();

                    TempData["StatusMessage"] = "Chapter Created Succesfully.";
                    return RedirectToAction("Chapters");
                }

                ViewBag.ErrorMessage = "Please fill in all the required fields";
                chapter.Chapters = await chapterList.Where(p => p.IsDeleted == 0).ToListAsync();
                return View(chapter);
            }
        }

        public ActionResult EditChapter()
        {
            return RedirectToAction("Chapters");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChapterDelete(int? chapterId)
        {
            if (chapterId == null)
            {
                TempData["ErrorMessage"] = "Chapter Not Deleted";
                return RedirectToAction("Chapters");
            }
            else
            {
                Chapter chapter = await db.Chapters.Where(c => c.Id == chapterId).FirstOrDefaultAsync();

                if (chapterId != null)
                {
                    chapter.IsDeleted = 1;
                    db.Entry(chapter).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    TempData["StatusMessage"] = "Chapter Deleted Succesfully.";
                    return RedirectToAction("Chapters");
                }

                TempData["ErrorMessage"] = "Chapter Not Deleted";
                return RedirectToAction("Chapters");
            }
        }


        public ActionResult StudentRegistration()
        {
            return View();
        }
    }
}