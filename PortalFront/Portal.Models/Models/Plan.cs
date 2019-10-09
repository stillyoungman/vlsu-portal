using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public class Plan : IPlan
    {
        private const sbyte UNSET = -9;
        private int score = UNSET;
        private readonly Enrollee owner;

        private IEnumerable<IAchievement> achievements = Enumerable.Empty<IAchievement>();

        public Plan(PlanEntry planEntry, IEnumerable<FinancingType> finTypes, long id, int prioriy, Enrollee owner)
        {
            PlanEntry = planEntry ?? throw new Exception("Required parameter PlanEntry is missed");
            FinTypes = finTypes;
            Id = id;
            Priority = prioriy;
            this.owner = owner;
        }

        public long Id { get; } //planAbit key
        public PlanEntry PlanEntry { get; }
        IPlanEntry IPlan.PlanEntry => PlanEntry;
        public int Priority { get; }
        public IEnumerable<FinancingType> FinTypes { get; }

        public int Score
        {
            get
            {
                if (score == UNSET)
                {
                    int current, sum = 0;
                    foreach (var e in PlanEntry.Exams)
                    {
                        current = owner.GetExamMark(e.Id, PlanEntry.Id);
                        if (current < e.MinMark)
                        {
                            //вернуть 0 если по одному из экзаменов меньше минимального балла
                            sum = 0;
                            break;
                        }

                        sum += current;
                    }

                    score = sum;
                }

                return score;
            }
        }

        public int AchScore => achievements.Sum(a => a.Score);
        public IEnumerable<Tuple<int, string>> AchViewModels => achievements.Select(a => new Tuple<int, string>(a.Score, a.Title));

        
        public string EnrollCategory => FinTypes.ReducedValue().Item1;
        public int FinTypeWeight => FinTypes.ReducedValue().Item2;

        public bool IsGovernmentSponsored => FinTypes.Any(ft => ft != FinancingType.Contract);

        public bool IsSelfSponsored => FinTypes.Any(ft => ft == FinancingType.Contract);

        internal void LoadAchievements() => achievements = owner.Achievements.Where(a => a.PlanEntryId == PlanEntry.Id);
    }
}
