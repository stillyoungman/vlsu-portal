using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Domain
{
    public static class Sort
    {
        public static IEnumerable<IEnrollee> DomainSort(this IEnumerable<IEnrollee> enrollees, IPlanEntry planEntry)
        {
            return enrollees
                .OrderByDescending(e => e.GetPlan(planEntry.Id).FinTypeWeight) //get enrolle plan and get numeric value of aplied fin type
                .ThenByDescending(e => e.GetPlan(planEntry.Id).Score)
                .ThenByDescending(e => planEntry.Exams.Count() > 0 ? e.GetExamMark(planEntry.Exams.ElementAt(0).Id, planEntry.Id) : 0)
                .ThenByDescending(e => planEntry.Exams.Count() > 1 ? e.GetExamMark(planEntry.Exams.ElementAt(1).Id, planEntry.Id) : 0)
                .ThenByDescending(e => planEntry.Exams.Count() > 2 ? e.GetExamMark(planEntry.Exams.ElementAt(2).Id, planEntry.Id) : 0)
                .ThenByDescending(e => planEntry.Exams.Count() > 3 ? e.GetExamMark(planEntry.Exams.ElementAt(3).Id, planEntry.Id) : 0)
                .ThenByDescending(e => planEntry.Exams.Count() > 4 ? e.GetExamMark(planEntry.Exams.ElementAt(4).Id, planEntry.Id) : 0)
                .ThenByDescending(e => planEntry.Exams.Count() > 5 ? e.GetExamMark(planEntry.Exams.ElementAt(5).Id, planEntry.Id) : 0)
                .ThenByDescending(e => planEntry.Exams.Count() > 6 ? e.GetExamMark(planEntry.Exams.ElementAt(6).Id, planEntry.Id) : 0)
                .ThenByDescending(e => e.KindPrivilege)
                .ToArray();
        }
    }
}
