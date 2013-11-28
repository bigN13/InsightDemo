using System;
using System.Collections.Generic;



namespace InsightDemoWPF.Models {
    public class Comment {
        // A property named ID is treated as a PK by default
        public int ID { get; set; }

        // Naming it PostID makes it a foreign key into the Posts table
        public int PostID { get; set; }

        
        public string Text { get; set; }

        public string Author { get; set; }
    }
}
