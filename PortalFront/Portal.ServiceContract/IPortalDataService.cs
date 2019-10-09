using Portal.Models.DataObjects;
using System.Collections.Generic;
using System.ServiceModel;

namespace Portal.ServiceContract
{
    [ServiceContract]
    public interface IPortalDataService
    {
        [OperationContract]
        IEnumerable<EnrolleeDO> GetEnrollees(string date, string time);
        [OperationContract]
        IEnumerable<AchievementDO> GetAchievements();
        [OperationContract]
        IEnumerable<ExamEntryDO> GetExams();
        [OperationContract]
        IEnumerable<PlanEntryDO> GetPlans();
        [OperationContract]
        DynamicConfigDO GetDynamicConfig();
        [OperationContract]
        long CreatePortalAbit(string name, string xmlString);
    }
}
