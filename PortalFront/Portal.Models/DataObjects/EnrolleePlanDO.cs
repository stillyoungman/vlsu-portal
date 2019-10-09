using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models.DataObjects
{
    public class EnrolleePlanDO
    {
        public long PlanId { get; set; }
        public int Priority { get; set; }
        public long AbitPlanId { get; set; }

        public Plan ToModel(PlanEntry planEntry, IEnumerable<EnrolleeFinTypeDO> finTypes, Enrollee owner)
        {
            if (planEntry.Id != PlanId) throw new Exception("Incorect PlanEntry instance");
            if (finTypes.Any(f => f.PlanId != PlanId)) throw new Exception("Incorect FinTypes");

            return new Plan(planEntry, finTypes.Select(f => (FinancingType)f.FinTypeId), AbitPlanId, Priority, owner);            
        }
    }
}
