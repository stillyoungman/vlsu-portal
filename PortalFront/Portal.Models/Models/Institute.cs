using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Portal.Models
{
    public class Institute : IInstitute
    {
        public Institute(long id, string name, string abbreviation)
        {
            Id = id;
            Name = name;
            NameAbbreviation = abbreviation;
            Bachelor = Enumerable.Empty<IPlanEntry>();
            Master = Enumerable.Empty<IPlanEntry>();
        }
        public long Id { get; }

        public string Name { get; }

        public string NameAbbreviation { get; }

        public IEnumerable<IPlanEntry> Bachelor { get; internal set; }

        public IEnumerable<IPlanEntry> Master { get; internal set; }

        public bool HasBachelor => Bachelor.Count() > 0;
        public bool HasMaster => Master.Count() > 0;

        public void SetPlans(IEnumerable<IPlanEntry> plans)
        {
            if (plans.Any(p => p.Institute.Id != Id)) throw new Exception("Incorrect incoming value..");

            var bachelors = new List<IPlanEntry>();
            var masters = new List<IPlanEntry>();

            foreach (var plan in plans)
            {
                if (plan.Degree == EducationDegree.Bachelor ||
                    plan.Degree == EducationDegree.PractialBachelor ||
                    plan.Degree == EducationDegree.Spec)
                {
                    bachelors.Add(plan);
                    continue;
                }

                if (plan.Degree == EducationDegree.Master)
                {
                    masters.Add(plan);
                    continue;
                }
            }

            Bachelor = bachelors;
            Master = masters;
        }

        public override bool Equals(object i)
        {
            if (!(i is Institute)) return false;
            return ((Institute)i).Id == this.Id;
        }

        public override int GetHashCode() => (int)Id;
    }
}
