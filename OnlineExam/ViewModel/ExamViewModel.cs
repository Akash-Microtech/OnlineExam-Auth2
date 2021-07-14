﻿using OnlineExam.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExam.ViewModel
{
    public class ExamViewModel
    {
        public int ExamAttend { get; set; }
        public GetAllExamById_Result GetExam { get; set; }
        public List<GetAllQusByExamId_Result> GetAllQus { get; set; }
    }
}