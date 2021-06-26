using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExam.ViewModel
{
    public class StudentRegistrationViewModel
    {
        public int UserId { get; set; }
        [Display(Name = "Group")]
        public int GroupId { get; set; }
        [Display(Name = "Batch")]
        public int BatchId { get; set; }
        [Display(Name = "Year")]
        public string ExamAttendingYear { get; set; }
        [Display(Name = "Preferred Day")]
        public string PreferredDay { get; set; }
        [Display(Name = "Application Date")]
        public DateTime ApplnDate { get; set; }
        [Display(Name = "Academic Year")]
        public string AcademicYear { get; set; }
        [Display(Name = "Admission Test Date")]
        public DateTime AdmissionTestDate { get; set; }
        [Display(Name = "Preferred Time")]
        public TimeSpan PreferredTime { get; set; }
        [Display(Name = "WhatsappNo")]
        public string WhatsappNo { get; set; }
        [Display(Name = "DOB")]
        public DateTime DOB { get; set; }
        [Display(Name = "Gender")]
        public int Gender { get; set; }
        [Display(Name = "Religion")]
        public string Religion { get; set; }

        [Display(Name = "Caste")]
        public string Caste { get; set; }
        [Display(Name = "Community")]
        public string Community { get; set; }
        [Display(Name = "BloodGroup")]
        public string BloodGroup { get; set; }
        [Display(Name = "Nationalty")]
        public string Nationalty { get; set; }
        [Display(Name = "Present Address")]
        public string PresentAddress { get; set; }
        [Display(Name = "Area")]
        public string Area { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "State")]
        public string State { get; set; }
        [Display(Name = "District")]
        public string District { get; set; }
        [Display(Name = "Pincode")]
        public string Pincode { get; set; }
        [Display(Name = "Quick ContactNo")]
        public string QuickContNo { get; set; }
        [Display(Name = "Photo")]
        public string Photo { get; set; }
        [Display(Name = "Quick WhatsaAppNo")]
        public string QuickWhatsApp { get; set; }
        [Display(Name = "Program")]
        public int PgmId { get; set; }
        [Display(Name = "Class")]
        public int ClassId { get; set; }
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        [Display(Name = "Sub Program")]
        public int SubPgmId { get; set; }

        //homedetails//
        [Display(Name = "Address1")]
        public string Address1 { get; set; }
        [Display(Name = "Address2")]
        public string Address2 { get; set; }
        [Display(Name = "Home Area")]
        public string AreaHome { get; set; }
        [Display(Name = "Pincode")]
        public string PincodeHome { get; set; }
        [Display(Name = "Quick ContactNo")]
        public string QuickContact { get; set; }
        [Display(Name = "Location")]
        public string LocationHome { get; set; }
        [Display(Name = "State")]
        public string StateHome { get; set; }
        [Display(Name = "Email Id")]
        public string EmaiId { get; set; }

        [Display(Name = "Quick WhatsappNo")]
        public string QuickHomeWhatsapp { get; set; }

        //Parentdetails//

        [Display(Name = "Father Name")]
        public string FrName { get; set; }
        [Display(Name = "Father Occupation")]
        public string FrOcc { get; set; }
        [Display(Name = "Father MobileNo")]
        public string FrMobNo { get; set; }
        [Display(Name = "Father MailId")]
        public string FrMailId { get; set; }
        [Display(Name = "Father District")]
        public string FrDistrict { get; set; }
        [Display(Name = "Father Sign")]
        public string FrSign { get; set; }
        [Display(Name = "Father State")]
        public string FrState { get; set; }
        [Display(Name = "Father WhatsAppNo")]
        public string FrWhatsApp { get; set; }
        [Display(Name = "Mother Name")]
        public string MrName { get; set; }
        [Display(Name = "Mother Occupation")]
        public string MrOcc { get; set; }
        [Display(Name = "Mother MobileNo")]
        public string MrMobNo { get; set; }
        [Display(Name = "Mother MailId")]
        public string MrMailId { get; set; }
        [Display(Name = "Mother District")]
        public string MrDistrict { get; set; }
        [Display(Name = "Mother Sign")]
        public string MrSign { get; set; }
        [Display(Name = "Mother State")]
        public string MrState { get; set; }
        [Display(Name = "Mother WhatsAppNo")]
        public string MrWhatsApp { get; set; }

        //academic performance//
        [Display(Name = "Class")]
        public string Class { get; set; }
        [Display(Name = "Passed Year")]
        public string PassYear { get; set; }
        [Display(Name = "School")]
        public string SchoolAddress { get; set; }
        [Display(Name = "Registration No")]
        public string RegNo { get; set; }
        [Display(Name = "Board")]
        public string Board { get; set; }
        [Display(Name = "Physics Mark")]
        public decimal PhyMark { get; set; }
        [Display(Name = "Chemistry Mark")]
        public decimal ChemMark { get; set; }
        [Display(Name = "Biology Mark")]
        public decimal BiologyMark { get; set; }
        [Display(Name = "Maths Mark")]
        public decimal MathsMark { get; set; }
        [Display(Name = "Percentage Of Mark")]
        public decimal PercOfMark { get; set; }

        //prev entrance//

        [Display(Name = "Previous Entrance Exam")]
        public string PrevEntranceExamName { get; set; }
        [Display(Name = "RollNo")]
        public string RollNo { get; set; }
        [Display(Name = "Attempted Year")]
        public string AttemptedYear { get; set; }
        [Display(Name = "Mark")]
        public decimal Mark { get; set; }
        [Display(Name = "Rank")]
        public int Rank { get; set; }
        [Display(Name = "No Of Attempts")]
        public int NoOfAttempts { get; set; }
    }
}