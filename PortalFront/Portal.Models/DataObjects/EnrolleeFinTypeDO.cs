using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models.DataObjects
{
    public class EnrolleeFinTypeDO
    {
        public long FinTypeId { get; set; }
        public long PlanId { get; set; }
        public bool Enrolled { get; set; }
    }
}
