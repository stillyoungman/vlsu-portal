using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public class PlanExam
    {
        public long Id => ExamEntry.Id;
        public string ShortName => ExamEntry.ShortName;
        public string Name => ExamEntry.Name;

        public IExamEntry ExamEntry { get; }
        public int MinMark { get; }
        public int Priority { get; }

        public PlanExam(IExamEntry e, int priority, int minMark)
        {
            ExamEntry = e;
            Priority = priority;
            MinMark = minMark;
        }
    }
}
