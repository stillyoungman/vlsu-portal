using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models.DataObjects
{
    public class PlanExamDO
    {
        public long ExamId { get; set; }
        public int Priority { get; set; }
        public int MinMark { get; set; }

        public PlanExam ToModel(IExamEntry ex)
        {
            if (ex.Id != ExamId) throw new Exception("Incorrect exam.");

            return new PlanExam(ex, Priority, MinMark);
        }
    }
}
