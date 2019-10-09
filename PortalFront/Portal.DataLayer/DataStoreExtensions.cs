using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.DataLayer
{
    internal static class DataStoreExt
    {
        public static void CalculatePlanEnrollees(this IEnumerable<IPlansVM> plansViewModels, IEnumerable<IEnrollee> enrollees)
        {
            Parallel.ForEach(plansViewModels, pvm =>
            {
                Parallel.ForEach(pvm.Institutes, i =>
                {
                    Parallel.ForEach(i.Bachelor, plan => (plan as PlanEntry)
                        .ApplicationsCount = enrollees.Where(e => e.Plans != null
                        && (e.Status == EnrolleeStatus.Enrollee || e.Status == EnrolleeStatus.Recomended || e.Status == EnrolleeStatus.Student)
                        && e.Plans.Any(p => p.PlanEntry.Id == plan.Id)).Count());

                    Parallel.ForEach(i.Master, plan => (plan as PlanEntry)
                        .ApplicationsCount = enrollees.Where(e => e.Plans != null
                        && (e.Status == EnrolleeStatus.Enrollee || e.Status == EnrolleeStatus.Recomended || e.Status == EnrolleeStatus.Student)
                        && e.Plans.Any(p => p.PlanEntry.Id == plan.Id)).Count());
                });
            });
        }
    }
}
