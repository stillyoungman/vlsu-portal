using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models
{
    public interface IEnrollee
    {
        long Id { get; }
        string FullName { get; }
        DocumentType Doc { get; }
        string KindPrivilege { get; }
        EnrolleeStatus Status { get; }

        Tuple<long, FinancingType> ApprovedTo { get; }
        long EnrolledTo { get; }

        int GetExamMark(long examId, long planEntryId);
        IPlan GetPlan(long planEntryId);

        IEnumerable<IPlan> Plans { get; }
    }
}
