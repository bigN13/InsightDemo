
namespace InsightDemoWPF.Models
{
   public class OfficeAssignment
   {
      public int PersonID  { get; set; }
      public string Location { get; set; }

      public virtual Instructor Instructor { get; set; }
   }
}