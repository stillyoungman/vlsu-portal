using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public interface IPlanEntry: ILongId
    {
        IPlanTitle Title { get; }
        IInstitute Institute { get; }
        PlanMode Mode { get; }
        Office Office { get; }
        EducationDegree Degree { get; }
        IEnumerable<PlanExam> Exams { get; }
        int GrantSpots { get; }
        int ApplicationsCount { get; }
        int Year { get; }
        IEnumerable<IPlanFinSource> FinSources { get; }
    }
}
