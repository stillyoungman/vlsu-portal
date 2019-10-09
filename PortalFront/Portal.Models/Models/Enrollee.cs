using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public class Enrollee : IEnrollee
    {
        private IEnumerable<IAchievement> achievements = Enumerable.Empty<IAchievement>();

        public long Id { get; internal set; }
        public string FullName { get; internal set; }
        public int Year { get; internal set; }
        public EnrolleeStatus Status { get; internal set; }

        public DocumentType Doc { get; internal set; }
        public long EnrolledTo { get; internal set; }

        public IEnumerable<IAchievement> Achievements
        {
            get => achievements;
            set
            {
                if (value.Any(ach => ach.OwnerId != Id)) throw new Exception("Enumerable contains element with invalid OwnerId.");

                achievements = value;
                foreach (var p in Plans) p.LoadAchievements();
            }
        }
        public IEnumerable<Exam> Exams { get; internal set; }
        public IEnumerable<Plan> Plans { get; internal set; }
        IEnumerable<IPlan> IEnrollee.Plans => Plans;

        public string KindPrivilege { get; internal set; }
        public Tuple<long, FinancingType> ApprovedTo { get; internal set; }

        public int GetExamMark(long examEntryId, long planEntryId) => Exams?.SingleOrDefault(e => e.ExamEntryId == examEntryId && e.PlanEntryId == planEntryId).Value ?? 0;

        public Plan GetPlan(long planEntryId) => Plans?.SingleOrDefault(p => p.PlanEntry.Id == planEntryId);
        IPlan IEnrollee.GetPlan(long planEntryId) => GetPlan(planEntryId);
    }
}
