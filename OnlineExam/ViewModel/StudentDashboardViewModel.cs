﻿using OnlineExam.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExam.ViewModel
{
    public class StudentDashboardViewModel
    {
        public List<GetExamByUserId_Result> GetExamByUserId { get; set; }

    }
}