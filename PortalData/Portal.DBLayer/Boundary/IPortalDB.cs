using System;
using System.Collections.Generic;
using System.Text;
using Portal.Models.DataObjects;

namespace Portal.DBLayer
{
    public interface IPortalDB
    {
        IEnumerable<EnrolleeDO> Enrollees(string date, string time);
        IEnumerable<AchievementDO> Achievements { get; }
        IEnumerable<ExamEntryDO> Exams { get; }
        IEnumerable<PlanEntryDO> Plans { get; }
        DynamicConfigDO DynamicConfig { get; }
        long CreatePortalEnrollee(string name, string data);
    }
}
