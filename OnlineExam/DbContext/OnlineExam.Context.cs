﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Exam_DBEntities : DbContext
    {
        public Exam_DBEntities()
            : base("name=Exam_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AttendExam> AttendExams { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Cours> Courses { get; set; }
        public virtual DbSet<DataEntry_QuestionBank> DataEntry_QuestionBank { get; set; }
        public virtual DbSet<DataEntry_Registration> DataEntry_Registration { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Exam_QnTable> Exam_QnTable { get; set; }
        public virtual DbSet<ExamResult> ExamResults { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Group_StudentTable> Group_StudentTable { get; set; }
        public virtual DbSet<Group_Teacher> Group_Teacher { get; set; }
        public virtual DbSet<Programme> Programmes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Student_AcademicPerformance> Student_AcademicPerformance { get; set; }
        public virtual DbSet<Student_HomeCountryDetails> Student_HomeCountryDetails { get; set; }
        public virtual DbSet<Student_Parent> Student_Parent { get; set; }
        public virtual DbSet<Student_PreviousEntrance> Student_PreviousEntrance { get; set; }
        public virtual DbSet<Student_Registration> Student_Registration { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubProgram> SubPrograms { get; set; }
        public virtual DbSet<Teachers_QuestionBank> Teachers_QuestionBank { get; set; }
        public virtual DbSet<Teachers_Registration> Teachers_Registration { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<AllStudentRegistrationDetails_Result> AllStudentRegistrationDetails()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AllStudentRegistrationDetails_Result>("AllStudentRegistrationDetails");
        }
    
        public virtual ObjectResult<GetAllDtpQusAns_Result> GetAllDtpQusAns()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllDtpQusAns_Result>("GetAllDtpQusAns");
        }
    
        public virtual ObjectResult<GetAllDtpQusAnsByUserId_Result> GetAllDtpQusAnsByUserId(Nullable<int> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("Userid", userid) :
                new ObjectParameter("Userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllDtpQusAnsByUserId_Result>("GetAllDtpQusAnsByUserId", useridParameter);
        }
    
        public virtual ObjectResult<GetAllExam_Result> GetAllExam()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllExam_Result>("GetAllExam");
        }
    
        public virtual ObjectResult<GetAllExamById_Result> GetAllExamById(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllExamById_Result>("GetAllExamById", idParameter);
        }
    
        public virtual ObjectResult<GetAllGroupLisByUserId_Result> GetAllGroupLisByUserId(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllGroupLisByUserId_Result>("GetAllGroupLisByUserId", idParameter);
        }
    
        public virtual ObjectResult<GetAllGroupList_Result> GetAllGroupList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllGroupList_Result>("GetAllGroupList");
        }
    
        public virtual ObjectResult<GetAllQusByExamId_Result> GetAllQusByExamId(Nullable<int> iD, Nullable<int> from)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllQusByExamId_Result>("GetAllQusByExamId", iDParameter, fromParameter);
        }
    
        public virtual ObjectResult<GetAllQusForEdit_Result> GetAllQusForEdit(Nullable<int> id, Nullable<int> from)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllQusForEdit_Result>("GetAllQusForEdit", idParameter, fromParameter);
        }
    
        public virtual ObjectResult<GetAllStudentRegistrationByRegId_Result> GetAllStudentRegistrationByRegId(string regId)
        {
            var regIdParameter = regId != null ?
                new ObjectParameter("RegId", regId) :
                new ObjectParameter("RegId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllStudentRegistrationByRegId_Result>("GetAllStudentRegistrationByRegId", regIdParameter);
        }
    
        public virtual ObjectResult<GetAllTeacherQusAns_Result> GetAllTeacherQusAns()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllTeacherQusAns_Result>("GetAllTeacherQusAns");
        }
    
        public virtual ObjectResult<GetAllTeacherQusAnsByUserId_Result> GetAllTeacherQusAnsByUserId(Nullable<int> userid)
        {
            var useridParameter = userid.HasValue ?
                new ObjectParameter("Userid", userid) :
                new ObjectParameter("Userid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllTeacherQusAnsByUserId_Result>("GetAllTeacherQusAnsByUserId", useridParameter);
        }
    
        public virtual ObjectResult<GetCourseDetailsByUserId_Result> GetCourseDetailsByUserId(Nullable<int> id, Nullable<int> from)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCourseDetailsByUserId_Result>("GetCourseDetailsByUserId", idParameter, fromParameter);
        }
    
        public virtual ObjectResult<GetExamByTeacherId_Result> GetExamByTeacherId(Nullable<int> userId, Nullable<System.DateTime> date)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetExamByTeacherId_Result>("GetExamByTeacherId", userIdParameter, dateParameter);
        }
    
        public virtual ObjectResult<GetExamByUserId_Result> GetExamByUserId(Nullable<int> userId, Nullable<System.DateTime> date)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetExamByUserId_Result>("GetExamByUserId", userIdParameter, dateParameter);
        }
    
        public virtual int GetExamIdWiseQuestions(Nullable<int> iD)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetExamIdWiseQuestions", iDParameter);
        }
    
        public virtual ObjectResult<GetGroupUserByTeacherId_Result> GetGroupUserByTeacherId(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetGroupUserByTeacherId_Result>("GetGroupUserByTeacherId", idParameter);
        }
    
        public virtual ObjectResult<GetStudentGroupbyGroupId_Result> GetStudentGroupbyGroupId(Nullable<int> groupid)
        {
            var groupidParameter = groupid.HasValue ?
                new ObjectParameter("Groupid", groupid) :
                new ObjectParameter("Groupid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetStudentGroupbyGroupId_Result>("GetStudentGroupbyGroupId", groupidParameter);
        }
    
        public virtual ObjectResult<GetTeacherGroupbyGroupId_Result> GetTeacherGroupbyGroupId(Nullable<int> groupid)
        {
            var groupidParameter = groupid.HasValue ?
                new ObjectParameter("Groupid", groupid) :
                new ObjectParameter("Groupid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTeacherGroupbyGroupId_Result>("GetTeacherGroupbyGroupId", groupidParameter);
        }
    
        public virtual ObjectResult<Student_AcademicPerformancebyRegid_Result> Student_AcademicPerformancebyRegid(string regId)
        {
            var regIdParameter = regId != null ?
                new ObjectParameter("RegId", regId) :
                new ObjectParameter("RegId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Student_AcademicPerformancebyRegid_Result>("Student_AcademicPerformancebyRegid", regIdParameter);
        }
    
        public virtual ObjectResult<Student_PreviousEntrancebyRegid_Result> Student_PreviousEntrancebyRegid(string regId)
        {
            var regIdParameter = regId != null ?
                new ObjectParameter("RegId", regId) :
                new ObjectParameter("RegId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Student_PreviousEntrancebyRegid_Result>("Student_PreviousEntrancebyRegid", regIdParameter);
        }
    
        public virtual ObjectResult<StudentAllDetailsByRegId_Result> StudentAllDetailsByRegId()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StudentAllDetailsByRegId_Result>("StudentAllDetailsByRegId");
        }
    }
}
