using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public class PlanEnrolleesVM : IPlanEnrolleesVM
    {
        public IEnumerable<IEnrollee> Enrollees { get; set; }
        public IPlanEntry PlanEntry { get; set; }
    }
}
