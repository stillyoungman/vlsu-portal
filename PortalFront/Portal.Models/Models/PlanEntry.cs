using Portal.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public class PlanEntry : IPlanEntry
    {
        private readonly IEnumerable<PlanExam> exams;

        public PlanEntry(IEnumerable<PlanExam> exams, IEnumerable<IPlanFinSource> fins)
        {
            this.exams = exams ?? Enumerable.Empty<PlanExam>();
            this.FinSources = fins ?? Enumerable.Empty<IPlanFinSource>();
        }

        public long Id { get; internal set; }
        public IPlanTitle Title { get; internal set; }
        public string CodeLong { get; internal set; }
        public EducationDegree Degree { get; internal set; }
        public Office Office { get; internal set; }
        public PlanMode Mode { get; internal set; }
        public int Year { get; internal set; }

        [DisplayName("Бюджетные места")]
        public int GrantSpots { get; internal set; }
        [DisplayName("Кол-во заявлений")]
        public int ApplicationsCount { get; set; }

        public IEnumerable<PlanExam> Exams => exams?.OrderBy(e => e.Priority);
        public IInstitute Institute { get; internal set; }
        public IEnumerable<IPlanFinSource> FinSources { get; }

    }
}
