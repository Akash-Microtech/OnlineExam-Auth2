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
    
    public partial class GetAllExam_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ExGroupId { get; set; }
        public int PgmId { get; set; }
        public int ClassId { get; set; }
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public System.DateTime DeletedDateTime { get; set; }
        public string ExamTime { get; set; }
        public int TotalMark { get; set; }
        public int QsAsFrom { get; set; }
        public string PName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string CourseName { get; set; }
    }
}
