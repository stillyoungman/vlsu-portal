using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models.DataObjects
{
    public class EnrolleeMarkDO
    {
        public long ExamId { get; set; }
        public int MarkValue { get; set; }
        public long PlanId { get; set; }

        public Exam ToModel() => new Exam() { ExamEntryId = ExamId, PlanEntryId = PlanId, Value = MarkValue };
    }
}
