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
    
    public partial class Exercise
    {
        public Exercise()
        {
            this.TrainingEnrollments = new HashSet<TrainingEnrollment>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public int Enrolled { get; set; }
        public int Price { get; set; }
    
        public virtual ICollection<TrainingEnrollment> TrainingEnrollments { get; set; }
    }
}
