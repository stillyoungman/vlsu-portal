using Portal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.DataLayer
{
    public interface IDataStore
    {
        IPlansVM GetPlansVM(PlanMode mode);
        IPlanEnrolleesVM GetPlanEnrolleesVM(long id);
        ApplicationVM ApplicationVM { get; }
        IDictionary<long, IPlanEntry> Plans { get; }
        bool IsOnlineRegistrationAvailable { get; }
        long CreateAbit(string name, string xml);
    }
}
