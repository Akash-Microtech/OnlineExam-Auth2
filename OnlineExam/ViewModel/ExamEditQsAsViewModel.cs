using OnlineExam.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExam.ViewModel
{
    public class ExamEditQsAsViewModel
    {
        public List<DataEntry_QuestionBank> QuestionBank { get; set; }
        public List<Teachers_QuestionBank> ManualQuestionBank { get; set; }
    }
}