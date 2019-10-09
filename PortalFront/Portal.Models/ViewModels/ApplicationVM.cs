using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public class ApplicationVM 
    {

        public IEnumerable<IExamEntry> Exams { get; set; }

        public string Base64Plans { get; set; }
    }
}
