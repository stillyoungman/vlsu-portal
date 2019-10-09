using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public class Exam
    {
        public long Id { get; set; }
        public long ExamEntryId { get; set; }
        public long PlanEntryId { get; set; }
        public int Value { get; set; }
    }
}
