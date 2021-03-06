//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GymMembershipSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class Member
    {
        public Member()
        {
            this.Accounts = new HashSet<Account>();
            this.Measurements = new HashSet<Measurement>();
            this.Reports = new HashSet<Report>();
            this.ShopPayments = new HashSet<ShopPayment>();
            this.TrainingEnrollments = new HashSet<TrainingEnrollment>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public long CardId { get; set; }
        public string TypeId { get; set; }
        public int NumOfEntrances { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> LastEntrance { get; set; }
    
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ShopPayment> ShopPayments { get; set; }
        public virtual ICollection<TrainingEnrollment> TrainingEnrollments { get; set; }
    }
}
