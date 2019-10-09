using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models.DataObjects
{
    public class ExamEntryDO : IExamEntry
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsEge { get; set; }

        public static ExamEntryDO FromDataRow(System.Data.DataRow row)
        {
            return new ExamEntryDO
            {
                Id = (long)row["nrec"],
                Name = (string)row["name"],
                IsEge = DBNull.Value != row["isEge"],
                ShortName = (DBNull.Value != row["shortName"] && !string.IsNullOrWhiteSpace((string)row["shortName"])) ? (string)row["shortName"] : null
            };
        }
    }
}
