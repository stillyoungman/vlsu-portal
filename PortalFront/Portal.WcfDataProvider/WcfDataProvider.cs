using System;
using Portal.DataLayer;
using Portal.Models.DataObjects;
using System.Collections.Generic;
using System.ServiceModel;
using Portal.ServiceContract;
using System.Threading.Tasks;

namespace Portal.WcfServiceDataProvider
{
    public class WcfDataProvider : IDataProvider
    {
        private readonly BasicHttpBinding binding;
        private readonly EndpointAddress endpoint;
        private ChannelFactory<IPortalDataService> ChannelFact => new ChannelFactory<IPortalDataService>(binding, endpoint);

        public WcfDataProvider(string uri)
        {
            binding = new BasicHttpBinding();
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;

            endpoint = new EndpointAddress(new Uri(uri));
        }

        public IEnumerable<AchievementDO> GetAchievements()
        {
            IEnumerable<AchievementDO> data = null;

            GetData((s) =>
            {
                data = s.GetAchievements();
            });

            return data;
        }
        public IEnumerable<EnrolleeDO> GetEnrollees(string date, string time)
        {
            IEnumerable<EnrolleeDO> data = null;

            GetData((s) => { data = s.GetEnrollees(date, time); });

            return data;
        }
        public IEnumerable<ExamEntryDO> GetExams()
        {
            IEnumerable<ExamEntryDO> data = null;

            GetData(s => data = s.GetExams());

            return data;
        }
        public IEnumerable<PlanEntryDO> GetPlans()
        {
            IEnumerable<PlanEntryDO> data = null;

            GetData(s => data = s.GetPlans());

            return data;
        }
        public DynamicConfigDO GetDynamicConfig()
        {
            DynamicConfigDO data = null;

            GetData(s => data = s.GetDynamicConfig());

            return data;
        }

        public long CreateAbit(string name, string xml)
        {
            long result = -1;

            GetData(s => result = s.CreatePortalAbit(name, xml));

            return result;
        }

        public Task<IEnumerable<EnrolleeDO>> GetEnrolleesAsync(string date, string time) => Task.Factory.StartNew(() => GetEnrollees(date, time));
        public Task<IEnumerable<AchievementDO>> GetAchievementsAsync() => Task.Factory.StartNew(() => GetAchievements());
        public Task<IEnumerable<ExamEntryDO>> GetExamsAsync() => Task.Factory.StartNew(() => GetExams());
        public Task<IEnumerable<PlanEntryDO>> GetPlansAsync() => Task.Factory.StartNew(() => GetPlans());
        public Task<DynamicConfigDO> GetDynamicConfigAsync() => Task.Factory.StartNew(() => GetDynamicConfig());
        public Task<long> CreateAbitAsync(string name, string xml) => Task.Factory.StartNew(() => CreateAbit(name, xml));

        private void GetData(Action<IPortalDataService> action)
        {
            var cf = ChannelFact;
            cf.Open();
            var channel = cf.CreateChannel();

            try
            {
                action(channel);
                cf.Close();
            }
            catch (Exception e)
            {
                cf.Abort();
            }
        }
    }
}
