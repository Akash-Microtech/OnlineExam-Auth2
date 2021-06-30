using OnlineExam.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace OnlineExam.Api
{
    public class RegisterController : ApiController
    {
        private readonly Exam_DBEntities db = new Exam_DBEntities();

        [Route("api/Register/GetAcademic")]
        public IHttpActionResult GetAcademicPerformance(string regId)
        {
            var data = db.Student_AcademicPerformance.Where(e => e.RegId == regId).ToList();
            return Ok(data);
        }

        [Route("api/Register/GetEntrance")]
        public IHttpActionResult GetEntrance(string regId)
        {
            var data = db.Student_PreviousEntrance.Where(e => e.RegId == regId).ToList();
            return Ok(data);
        }


        [Route("api/Register/AcademicPerformance")]
        [ResponseType(typeof(Student_AcademicPerformance))]
        public IHttpActionResult AcademicPerformance(Student_AcademicPerformance student_AcademicPerformance)
        {
            student_AcademicPerformance.CreatedDate = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Student_AcademicPerformance.Add(student_AcademicPerformance);
            db.SaveChanges();


            return Ok(new { id = student_AcademicPerformance.Id });
        }



        // DELETE: api/Student_AcademicPerformance/5
        [Route("api/Register/DeletePerformance/{id:int}")]
        [ResponseType(typeof(Student_AcademicPerformance))]
        public IHttpActionResult DeletePerformance(int id)
        {
            Student_AcademicPerformance student_AcademicPerformance = db.Student_AcademicPerformance.Find(id);
            if (student_AcademicPerformance == null)
            {
                return NotFound();
            }

            db.Student_AcademicPerformance.Remove(student_AcademicPerformance);
            db.SaveChanges();

            return Ok(student_AcademicPerformance);
        }        

        // POST: api/Student_PreviousEntrance

        [Route("api/Register/PreviousEntrance")]
        [ResponseType(typeof(Student_PreviousEntrance))]
        public IHttpActionResult PreviousEntrance(Student_PreviousEntrance student_PreviousEntrance)
        {
            student_PreviousEntrance.CreatedDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Student_PreviousEntrance.Add(student_PreviousEntrance);
            db.SaveChanges();
            return Ok(new { id = student_PreviousEntrance.Id });
        }



        // DELETE: api/Student_PreviousEntrance/5
        [Route("api/Register/DeletePreviousEntrance/{id:int}")]
        [ResponseType(typeof(Student_PreviousEntrance))]
        public IHttpActionResult DeletePreviousEntrance(int id)
        {
            Student_PreviousEntrance student_PreviousEntrance = db.Student_PreviousEntrance.Find(id);
            if (student_PreviousEntrance == null)
            {
                return NotFound();
            }

            db.Student_PreviousEntrance.Remove(student_PreviousEntrance);
            db.SaveChanges();

            return Ok(student_PreviousEntrance);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Student_AcademicPerformanceExists(int id)
        {
            return db.Student_AcademicPerformance.Count(e => e.Id == id) > 0;
        }
    }
}
