using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public interface IPlansVM
    {
        PlanMode Mode { get; }

        IEnumerable<IInstitute> Institutes { get; }
    }
}
