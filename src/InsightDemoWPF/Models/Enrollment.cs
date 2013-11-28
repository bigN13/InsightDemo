﻿
namespace InsightDemoWPF.Models
{
   public enum Grade
   {
      A, B, C, D, F
   }

   public class Enrollment
   {
       public int EnrollmentID { get; set; }
       public int CourseID { get; set; }
       public int PersonID { get; set; }
       public Grade? Grade { get; set; }

       public virtual Course Course { get; set; }
       public virtual Person Person { get; set; }
   }
}