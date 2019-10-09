using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Portal.Models.DataObjects
{
    public class AchievementDO : IAchievement
    {
        public long AchievementId { get; set; }
        public long OwnerId { get; set; }
        public int Score => (int)score;
        public long PlanEntryId { get; set; }
        public string Title { get; set; }

        public decimal score { get; set; }

        public static AchievementDO FromDataRow(DataRow row) => new AchievementDO
        {
            AchievementId = (long)row["id"],
            OwnerId = (long)row["ownerId"],
            PlanEntryId = (long)row["planEntryId"],
            score = (decimal)row["score"],
            Title = (string)row["title"]
        };

        
    }
}
