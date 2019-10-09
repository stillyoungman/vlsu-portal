using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;

namespace Portal.Models.DataObjects
{
    public class EnrolleeDO
    {
        public EnrolleeDO()
        {
            Plans = new EnrolleePlanDO[0];
            Marks = new EnrolleeMarkDO[0];
            FinTypes = new EnrolleeFinTypeDO[0];
        }
        public long Id { get; set; }
        public string FullName { get; set; }
        public int Year { get; set; }
        public string KindPrivilege { get; set; }
        public DocumentType Doc { get; set; }
        public EnrolleeStatus Status { get; set; }

        public ConsentRecordDO ConsentRecord { get; set; }
        public EnrolleePlanDO[] Plans { get; set; }
        public EnrolleeMarkDO[] Marks { get; set; }
        public EnrolleeFinTypeDO[] FinTypes { get; set; }

        public static EnrolleeDO FromDataRow(DataRow row)
        {

            var e = new EnrolleeDO
            {
                Id = (long)row["nrec"],
                FullName = (string)row["fullName"],
                Year = (int)row["year"],
                KindPrivilege = (string)row["KindPrivilege"],
                Doc = (DocumentType)row["isOrigin"],
                Status = (EnrolleeStatus)row["status"],
            };

            if (DBNull.Value != row["plans"])
            {
                e.ParsePlans((string)row["plans"]);
            }
            if (DBNull.Value != row["marks"])
            {
                e.ParseMarks((string)row["marks"]);
            }
            if (DBNull.Value != row["fin_pass"])
            {
                e.ParseFins((string)row["fin_pass"]);
            }
            if (DBNull.Value != row["approvedTo"])
            {
                e.ParseConsentRecord((string)row["approvedTo"]);
            }

            return e;
        }

        public Enrollee ToModel(IDictionary<long,PlanEntry> plansDict)
        {
            Enrollee enrollee = new Enrollee()
            {
                Id = Id,
                FullName = FullName,
                Year = Year,
                KindPrivilege = KindPrivilege,
                Doc = Doc,
                Status = Status,
                EnrolledTo = FinTypes.SingleOrDefault(f => f.Enrolled)?.PlanId ?? 0,
                ApprovedTo = ConsentRecord?.ToModel()
            };

            enrollee.Plans = Plans.Select(p => p.ToModel(plansDict[p.PlanId], FinTypes.Where(f => f.PlanId == p.PlanId), enrollee)).ToArray();
            enrollee.Exams = Marks.Select(m => m.ToModel()).ToArray();
            
            return enrollee;
        }

        private void ParsePlans(string source) //source looks like planId:priority:abitPlanNrec "81474976714273:1:31474976714273,%repeat%"
        {
            var plans = source.Split(',');
            var result = new EnrolleePlanDO[plans.Length];

            string[] rawPlan;
            long planId;
            int priority;
            long id;

            for (int i = 0; i < plans.Length; i++)
            {
                rawPlan = plans[i].Split(':');
                planId = long.Parse(rawPlan[0]);
                priority = int.Parse(rawPlan[1]);
                id = long.Parse(rawPlan[2]);

                result[i] = new EnrolleePlanDO
                {
                    PlanId = planId,
                    Priority = priority,
                    AbitPlanId = id
                };
            }

            Plans = result;
        }
        private void ParseMarks(string source) // examId:mark:planId
        {
            var exams = source.Split(',');
            var result = new EnrolleeMarkDO[exams.Length];

            string[] rawExam;
            long examId;
            int value;
            long planEntryId;

            for (var i = 0; i < exams.Length; i++)
            {
                rawExam = exams[i].Split(':');
                examId = long.Parse(rawExam[0]);
                value = int.Parse(rawExam[1]);
                planEntryId = long.Parse(rawExam[2]);

                result[i] = new EnrolleeMarkDO
                {
                    MarkValue = value,
                    ExamId = examId,
                    PlanId = planEntryId
                };
            }

            Marks = result;

        }
        private void ParseFins(string source) //finId:planId:enrolled
        {
            var items = source.Split(',');
            var result = new EnrolleeFinTypeDO[items.Length];

            string[] item;
            long finId;
            long planId;
            int enrolled;

            for (var i = 0; i < items.Length; i++)
            {
                item = items[i].Split(':');
                finId = long.Parse(item[0]);
                planId = long.Parse(item[1]);
                enrolled = int.Parse(item[2]);

                result[i] = new EnrolleeFinTypeDO
                {
                    FinTypeId = finId,
                    PlanId = planId,
                    Enrolled = enrolled == 1
                };

                //if (enrolled == 1)
                //{
                //    EnrolledTo = planId;
                //}
            }

            FinTypes = result;
        }

        private void ParseConsentRecord(string source) //planId:finId
        {
            var parts = source.Split(':');
            var planId = long.Parse(parts[0]);
            var finType = (FinancingType)long.Parse(parts[1]);

            ConsentRecord = new ConsentRecordDO
            {
                PlanId = planId,
                FinType = finType
            };
        }



        public class ConsentRecordDO 
        {
            public long PlanId { get; set; }
            public FinancingType FinType { get; set; }

            public Tuple<long, FinancingType> ToModel() => new Tuple<long, FinancingType>(PlanId, FinType);
        }
    }
}
