using System.Collections.Generic;
using System.ServiceModel;
using Portal.Models.DataObjects;
using Portal.DBLayer;
using Portal.ServiceContract;
using System.Web.Configuration;

namespace Portal.WCF
{
    /// <summary>
    /// Если в какой-то enum попадет значение которого там нет, то будет ошибка сериализации!
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class PortalDataService : IPortalDataService
    {
        private readonly IPortalDB db;

        public PortalDataService()
        {
            db = new PortalDB(new DataConfig()
            {
                ConnectionString = WebConfigurationManager.AppSettings["connectionString"],
                Year = int.Parse(WebConfigurationManager.AppSettings["year"])
            });
        }

        public IEnumerable<AchievementDO> GetAchievements() => db.Achievements;

        public IEnumerable<ExamEntryDO> GetExams() => db.Exams;

        public IEnumerable<EnrolleeDO> GetEnrollees(string date, string time) => db.Enrollees(date, time);

        public IEnumerable<PlanEntryDO> GetPlans() => db.Plans;

        public DynamicConfigDO GetDynamicConfig() => db.DynamicConfig;

        public long CreatePortalAbit(string name, string data) => db.CreatePortalEnrollee(name, data);
    }
}
