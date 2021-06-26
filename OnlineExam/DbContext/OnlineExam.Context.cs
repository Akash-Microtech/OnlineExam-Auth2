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
    using OnlineExam.Models;

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
    
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Cours> Courses { get; set; }
        public virtual DbSet<DataEntry_QuestionBank> DataEntry_QuestionBank { get; set; }
        public virtual DbSet<DataEntry_Registration> DataEntry_Registration { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Exam_QnTable> Exam_QnTable { get; set; }
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

        public virtual ObjectResult<GETCHAPTERBYSUB_Result> GETCHAPTERBYSUB()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GETCHAPTERBYSUB_Result>("GETCHAPTERBYSUB");
        }
    
        public virtual int GETCOURSEBYCLASS()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GETCOURSEBYCLASS");
        }
    
        public virtual ObjectResult<GETPRGRMBYSUBPRGM_Result> GETPRGRMBYSUBPRGM()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GETPRGRMBYSUBPRGM_Result>("GETPRGRMBYSUBPRGM");
        }
    
        public virtual int GETUSERROLEBYID()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GETUSERROLEBYID");
        }
    }
}
