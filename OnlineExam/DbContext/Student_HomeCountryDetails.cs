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
    
    public partial class Student_HomeCountryDetails
    {
        public int Id { get; set; }
        public string RegId { get; set; }
        public string AddressHome1 { get; set; }
        public string AddressHome2 { get; set; }
        public string AreaHome { get; set; }
        public string PincodeHome { get; set; }
        public string QuickHomeContact { get; set; }
        public string LocationHome { get; set; }
        public string StateHome { get; set; }
        public string EmaiIdHome { get; set; }
        public string QuickHomeWhatsapp { get; set; }
        public string DistrictHome { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int IsDeleted { get; set; }
    }
}