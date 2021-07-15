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

        [Route("api/Exam/QsAs/{id:int}")]
        [ResponseType(typeof(GetAllQusByExamId_Result))]
        public IHttpActionResult GetExamQsAs(int id)
        {
            List<GetAllQusByExamId_Result> GetAllQusByExamId = db.GetAllQusByExamId(id).ToList();
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
    }
}
