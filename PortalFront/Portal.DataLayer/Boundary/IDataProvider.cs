using Portal.Models.DataObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portal.DataLayer
{
    public interface IDataProvider
    {
        IEnumerable<EnrolleeDO> GetEnrollees(string date, string time);
        IEnumerable<AchievementDO> GetAchievements();
        IEnumerable<ExamEntryDO> GetExams();
        IEnumerable<PlanEntryDO> GetPlans();
        DynamicConfigDO GetDynamicConfig();

        Task<IEnumerable<EnrolleeDO>> GetEnrolleesAsync(string date, string time);
        Task<IEnumerable<AchievementDO>> GetAchievementsAsync();
        Task<IEnumerable<ExamEntryDO>> GetExamsAsync();
        Task<IEnumerable<PlanEntryDO>> GetPlansAsync();
        Task<DynamicConfigDO> GetDynamicConfigAsync();
        Task<long> CreateAbitAsync(string name, string xmlString);
    }
}
