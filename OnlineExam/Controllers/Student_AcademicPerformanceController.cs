using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using OnlineExam.DbContext;

namespace OnlineExam.Controllers
{
    public class Student_AcademicPerformanceController : ApiController
    {
        private Exam_DBEntities db = new Exam_DBEntities();

        // GET: api/Student_AcademicPerformance
        public IQueryable<Student_AcademicPerformance> GetStudent_AcademicPerformance()
        {
            return db.Student_AcademicPerformance;
        }

        // GET: api/Student_AcademicPerformance/5
        [ResponseType(typeof(Student_AcademicPerformance))]
        public async Task<IHttpActionResult> GetStudent_AcademicPerformance(int id)
        {
            Student_AcademicPerformance student_AcademicPerformance = await db.Student_AcademicPerformance.FindAsync(id);
            if (student_AcademicPerformance == null)
            {
                return NotFound();
            }

            return Ok(student_AcademicPerformance);
        }

        // PUT: api/Student_AcademicPerformance/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudent_AcademicPerformance(int id, Student_AcademicPerformance student_AcademicPerformance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student_AcademicPerformance.Id)
            {
                return BadRequest();
            }

            db.Entry(student_AcademicPerformance).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Student_AcademicPerformanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Student_AcademicPerformance
        [ResponseType(typeof(Student_AcademicPerformance))]
        public async Task<IHttpActionResult> PostStudent_AcademicPerformance(Student_AcademicPerformance student_AcademicPerformance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Student_AcademicPerformance.Add(student_AcademicPerformance);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = student_AcademicPerformance.Id }, student_AcademicPerformance);
        }

        // DELETE: api/Student_AcademicPerformance/5
        [ResponseType(typeof(Student_AcademicPerformance))]
        public async Task<IHttpActionResult> DeleteStudent_AcademicPerformance(int id)
        {
            Student_AcademicPerformance student_AcademicPerformance = await db.Student_AcademicPerformance.FindAsync(id);
            if (student_AcademicPerformance == null)
            {
                return NotFound();
            }

            db.Student_AcademicPerformance.Remove(student_AcademicPerformance);
            await db.SaveChangesAsync();

            return Ok(student_AcademicPerformance);
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