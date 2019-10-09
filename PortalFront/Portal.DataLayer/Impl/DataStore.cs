using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Domain;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Portal.DataLayer
{
    public class DataStore : IDataStore
    {
        private IDynamicConfig dConf = null;
        private readonly IDataProvider dp;
        private IDictionary<long, IExamEntry> examsDict;
        private IDictionary<long, PlanEntry> plansDict;
        private IDictionary<long, Enrollee> enrolleesDict;
        private IDictionary<PlanMode, IPlansVM> plansViewModels;
        private string json;

        private DateTime lastUpdate;

        public DataStore(IDataProvider dp)
        {
            this.dp = dp;
            dConf = dp.GetDynamicConfig();
            examsDict = dp.GetExams().Select(e => e as IExamEntry).ToDictionary(e => e.Id);
            plansDict = dp.GetPlans().Select(p => p.ToModel(examsDict)).ToDictionary(p => p.Id);
            enrolleesDict = dp.GetEnrollees("", "").Select(e => e.ToModel(plansDict)).ToDictionary(e => e.Id);

            var achievements = dp.GetAchievements();
            ApplyAchievements(enrolleesDict, achievements);

            plansViewModels = plansDict.Values.MakeViewModels().ToDictionary(pvm => pvm.Mode);
            plansViewModels.Values.CalculatePlanEnrollees(enrolleesDict.Values);

            json = MakePlansJson(plansViewModels);

            lastUpdate = DateTime.Now;
        }


        #region IDataStore

        public ApplicationVM ApplicationVM => new ApplicationVM()
        {
            Exams = examsDict.Values.Where(e => e.IsEge).OrderBy(e => e.Name).ToArray(),
            Base64Plans = OnlineEnrollmentPlans
        };

        public IDictionary<long, IPlanEntry> Plans => plansDict.Values.Select(plan => plan as IPlanEntry).ToDictionary(p => p.Id);

        public bool IsOnlineRegistrationAvailable => dp.GetDynamicConfig().IsOnlineRegisterAvailable;

        public IPlanEnrolleesVM GetPlanEnrolleesVM(long id)
        {
            if (!plansDict.TryGetValue(id, out PlanEntry plan)) return null;

            if (ShouldUpdate) Update();

            var enrollees = enrolleesDict.Values
                    .Where(e => e != null
                    && (e.Status == EnrolleeStatus.Enrollee || e.Status == EnrolleeStatus.Recomended || e.Status == EnrolleeStatus.Student)
                    && e.Plans != null
                    && e.Plans.Any(p => p.PlanEntry.Id == id)
                    && e.GetPlan(id).FinTypes.Count() > 0)
                    .DomainSort(plan);

            return new PlanEnrolleesVM()
            {
                PlanEntry = plan,
                Enrollees = enrollees
            };
        }

        public long CreateAbit(string name, string xml)
        {
            var t = dp.CreateAbitAsync(name, xml);
            t.Wait();
            return t.Result;
        }

        public IPlansVM GetPlansVM(PlanMode mode) => plansViewModels[mode];

        public string OnlineEnrollmentPlans => json;
        #endregion


        private bool ShouldUpdate => DateTime.Now.Subtract(lastUpdate).Seconds >= dConf.UpdateIntervalSec;
        private void Update()
        {
            var from = lastUpdate;
            lastUpdate = DateTime.Now; //чтобы другие потоки не пытались выполнить аналогичное действие
            
            var uTask = dp.GetEnrolleesAsync(from.Date.ToShortDateString(), from.TimeOfDay.ToString().Split('.')[0]);
            var aTask = dp.GetAchievementsAsync();
            var dTask = dp.GetDynamicConfigAsync();
            Task.WaitAll(uTask, aTask, dTask);

            var updated = uTask.Result;
            var achivs = aTask.Result;
            dConf = dTask.Result ?? dConf;

            lock (enrolleesDict)
            {
                foreach (var enrollee in updated)
                {
                    enrolleesDict[enrollee.Id] = enrollee.ToModel(plansDict);
                }

                ApplyAchievements(enrolleesDict, achivs);
            }

            if (updated.Count() > 0) CalculatePlanEnrollees(plansViewModels.Values, enrolleesDict.Values);
        }

        private static void ApplyAchievements(IDictionary<long, Enrollee> enrollees, IEnumerable<IAchievement> achievements)
        {
            Parallel.ForEach(enrollees.Values, (e) =>
            {
                e.Achievements = Enumerable.Empty<IAchievement>();
            });

            var awarardedId = achievements.Select(a => a.OwnerId).Distinct();

            foreach (var id in awarardedId)
            {
                if (enrollees.TryGetValue(id, out Enrollee e))
                {
                    e.Achievements = achievements.Where(a => a.OwnerId == id);
                }
            }
        }
        private static string MakePlansJson(IDictionary<PlanMode, IPlansVM> plansViewModels)
        {
            var data = plansViewModels.Values
                        .Single(pvm => pvm.Mode == PlanMode.FullTime)
                        .Institutes.Where(i => i.Bachelor.Count() > 0 && i.Bachelor.Any(p => p.Office == Office.Vladimir)) //все где есть планы и есть офис Владимир
                        .Select(i =>
                        {
                            return new
                            {
                                name = i.Name,
                                plans = i.Bachelor.Select(p =>
                                {
                                    return new
                                    {
                                        id = p.Id,
                                        spots = p.GrantSpots,
                                        title = new
                                        {
                                            full = p.Title.Full,
                                            code = p.Title.Code
                                        },
                                        grantSpots = p.GrantSpots,
                                        exams = p.Exams.Select(e => e.Id)
                                    };
                                })
                            };
                        });
            //сделать json
            return JsonConvert.SerializeObject(data);

            //закодировать
            //var byteJson = Encoding.UTF8.GetBytes(json);
            //applicationPlansBase64 = Convert.ToBase64String(byteJson);
        }
        private static void CalculatePlanEnrollees(IEnumerable<IPlansVM> plansViewModels, IEnumerable<IEnrollee> enrollees)
        {
            Parallel.ForEach(plansViewModels, pvm =>
            {
                Parallel.ForEach(pvm.Institutes, i =>
                {
                    Parallel.ForEach(i.Bachelor, plan => (plan as PlanEntry)
                        .ApplicationsCount = enrollees.Where(e => e.Plans != null && e.Plans.Any(p => p.PlanEntry.Id == plan.Id)).Count());

                    Parallel.ForEach(i.Master, plan => (plan as PlanEntry)
                        .ApplicationsCount = enrollees.Where(e => e.Plans != null && e.Plans.Any(p => p.PlanEntry.Id == plan.Id)).Count());
                });
            });
        }

    }

    
}
