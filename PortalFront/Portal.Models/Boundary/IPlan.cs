using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public interface IPlan : ILongId
    {
        IPlanEntry PlanEntry { get; }
        int Priority { get; }
        int Score { get; }
        int AchScore { get; }
        int FinTypeWeight { get; }
        string EnrollCategory { get; }
        IEnumerable<Tuple<int, string>> AchViewModels { get; }

        bool IsGovernmentSponsored { get; }
        bool IsSelfSponsored { get; }
    }
}
