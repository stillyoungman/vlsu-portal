using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using System.IO;
using System.Xml;
using System.Data;

namespace Portal.Models.DataObjects
{
    public class PlanEntryDO
    {
        public long Id { get; set; }
        public TitleEntry Title { get; set; }

        public string CodeLong { get; set; }
        public EducationDegree Degree { get; set; }

        public Office Office { get; set; }
        public PlanMode Mode { get; set; }

        public int Year { get; set; }

        public int GrantSpots { get; set; }
        public InstituteDO Institute { get; set; }
        //serialization unexpected beheivor
        public PlanExamDO[] Exams { get; set; }
        public IEnumerable<PlanFinSourceDO> FinSources { get; set; }

        public PlanEntry ToModel(IDictionary<long, IExamEntry> examsDict)
        {
            var exams = Exams?.Select(e => e.ToModel(examsDict[e.ExamId])) ?? Enumerable.Empty<PlanExam>();

            var result = new PlanEntry(exams, this.FinSources)
            {
                Id = Id,
                Title = Title,
                CodeLong = CodeLong,
                Degree = Degree,
                Office = Office,
                Mode = Mode,
                Year = Year,
                GrantSpots = GrantSpots,
                Institute = Institute.ToModel()
            };

            return result;
        }

        public static PlanEntryDO FromDataRow(System.Data.DataRow row)
        {
            PlanEntryDO plan = new PlanEntryDO
            {
                Id = (long)row["nrec_pl"],
                Title = new TitleEntry(
                    (string)row["code_sp_only"],
                    (string)row["name_sp_only"],
                    (string)row["name_spec_only"],
                    (string)row["term_txt"],
                    (EducationDegree)int.Parse((string)row["code_qu_only"])
                ),
                Institute = new InstituteDO(
                    (long)row["nrec_fak"],
                    (string)row["name_fak_full"],
                    (string)row["name_fak"]
                ),
                CodeLong = (string)row["code_spec_only"],
                Degree = (EducationDegree)int.Parse((string)row["code_qu_only"]),
                Office = (Office)row["filial"],
                Mode = (PlanMode)row["wformed"],
                Year = (int)row["yeared"],
                GrantSpots = (int)row["num_abit_budj_plan"]
            };

            plan.ParsePlanExams((string)row["examines"]);
            plan.ParsePlanFinsSources((string)row["finsources"]);

            return plan;
        }

        private void ParsePlanExams(string source) //examId:priotiry:minMark separated by ','
        {
            if (source?.Length == 0) { return; }

            using (DataSet ds = new DataSet())
            {
                using (StringReader stringReader = new StringReader(source))
                {
                    ds.ReadXml(stringReader);

                    if (ds.Tables.Count == 1)
                    {
                       this.Exams = new PlanExamDO[ds.Tables[0].Rows.Count];
                        for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            long id = long.Parse(ds.Tables[0].Rows[i]["nrec_dis"].ToString());
                            int priority = int.Parse(ds.Tables[0].Rows[i]["is_prof"].ToString());
                            int minMark = int.Parse(ds.Tables[0].Rows[i]["min_ball"].ToString());

                            this.Exams[i] = new PlanExamDO
                            {
                                ExamId = id,
                                Priority = priority,
                                MinMark = minMark
                            };
                        }
                    }
                }
            }

            //var items = source.Split(',');

            //var exams = new PlanExamDO[items.Length];

            //for (var i = 0; i < items.Length; i++)
            //{
            //    var parts = items[i].Split(':');
            //    var id = long.Parse(parts[0]);
            //    var priority = int.Parse(parts[1]);
            //    var minMark = int.Parse(parts[2]);

            //    exams[i] = new PlanExamDO
            //    {
            //        ExamId = id,
            //        Priority = priority,
            //        MinMark = minMark
            //    };
            //}

            //Exams = exams;
        }
        private void ParsePlanFinsSources(string source)
        {
            if (source?.Length == 0) { return; }

            using (DataSet ds = new DataSet())
            {
                using (StringReader stringReader = new StringReader(source))
                {
                    ds.ReadXml(stringReader);

                    if (ds.Tables.Count == 1)
                    {
                        PlanFinSourceDO[] fins = new PlanFinSourceDO[ds.Tables[0].Rows.Count];
                        for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            long nrec_fin = long.Parse(ds.Tables[0].Rows[i]["nrec_fin"].ToString());
                            int num_stud_plan = int.Parse(ds.Tables[0].Rows[i]["num_stud_plan"].ToString());

                            fins[i] = new PlanFinSourceDO((FinancingType)nrec_fin, num_stud_plan);
                        }
                        this.FinSources = fins;
                    }
                }
            }
        }
    }
}
