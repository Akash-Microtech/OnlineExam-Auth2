using OnlineExam.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExam.ViewModel
{
    public class ExamViewModel
    {
        public GetAllExamById_Result GetExam { get; set; }
        public List<GetExamIdWiseQuestions_Result> GetAllQus { get; set; }
    }
}