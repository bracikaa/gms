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
    
    public partial class TrainingEnrollment
    {
        public int MemberId { get; set; }
        public System.DateTime EnrollmentDate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public decimal ExercisePrice { get; set; }
    
        public virtual Exercise Exercise { get; set; }
        public virtual Member Member { get; set; }
    }
}
