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
    public class ExamController : ApiController
    {
        private readonly Exam_DBEntities db = new Exam_DBEntities();

        [Route("api/Exam/QsAs/{id:int}/{fromId:int}")]
        [ResponseType(typeof(GetAllQusByExamId_Result))]
        public IHttpActionResult GetExamQsAs(int id, int fromId)
        {
            List<GetAllQusByExamId_Result> GetAllQusByExamId = db.GetAllQusByExamId(id, fromId).ToList();
            return Ok(GetAllQusByExamId);
        }

        [Route("api/Exam/DeleteExam/{id:int}")]
        [ResponseType(typeof(Exam))]
        public IHttpActionResult DeletePerformance(int id)
        {
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return NotFound();
            }

            List<Exam_QnTable> exam_Qn = db.Exam_QnTable.Where(e => e.ExamId == exam.Id).ToList();
            foreach (var item in exam_Qn)
            {
                db.Exam_QnTable.Remove(item);
            }
            db.Exams.Remove(exam);
            db.SaveChanges();

            return Ok(exam);
        }

        [Route("api/Exam/DeleteExamQsAs/{id}/{examId:int}")]
        [ResponseType(typeof(Exam_QnTable))]
        public IHttpActionResult DeletePerformance(string id, int examId)
        {
            Exam_QnTable exam_Qn = db.Exam_QnTable.Where(e => e.QnId == id && e.ExamId == examId).FirstOrDefault();
            if (exam_Qn == null)
            {
                return NotFound();
            }

            db.Exam_QnTable.Remove(exam_Qn);
            db.SaveChanges();

            return Ok(exam_Qn);
        }

        [HttpPost]
        [Route("api/Exam/GetQsAsBank")]
        [ResponseType(typeof(Student_AcademicPerformance))]
        public IHttpActionResult GetQsAsFromQnBank(Exam exam)
        {
            List<DataEntry_QuestionBank> result = db.DataEntry_QuestionBank
                .Where(d => d.IsDeleted == 0 && d.PgmId == exam.PgmId && d.CourseId == exam.CourseId && d.SubjectId == exam.SubjectId).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Route("api/Exam/GetQsAsManual")]
        [ResponseType(typeof(Student_AcademicPerformance))]
        public IHttpActionResult GetQsAsFromManual(Exam exam)
        {
            List<Teachers_QuestionBank> result = db.Teachers_QuestionBank.Where(d => d.IsDeleted == 0 && d.PgmId == exam.PgmId && d.CourseId == exam.CourseId && d.SubjectId == exam.SubjectId && d.CreatedBy == exam.CreatedBy).ToList();
            return Ok(result);
        }
    }
}
