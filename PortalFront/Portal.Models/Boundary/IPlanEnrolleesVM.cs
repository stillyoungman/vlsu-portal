using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public interface IPlanEnrolleesVM
    {
        IEnumerable<IEnrollee> Enrollees { get; }
        IPlanEntry PlanEntry { get; }
    }
}
