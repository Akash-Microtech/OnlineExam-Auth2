//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineExam.DbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student_PreviousEntrance
    {
        public int Id { get; set; }
        public int StudRegId { get; set; }
        public string PrevEntranceExamName { get; set; }
        public string RollNo { get; set; }
        public string AttemptedYear { get; set; }
        public Nullable<decimal> Mark { get; set; }
        public Nullable<int> Rank { get; set; }
        public Nullable<int> NoOfAttempts { get; set; }
    
        public virtual Student_Registration Student_Registration { get; set; }
    }
}
