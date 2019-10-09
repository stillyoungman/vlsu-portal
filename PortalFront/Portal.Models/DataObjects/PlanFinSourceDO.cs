using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Models.DataObjects
{
    public class PlanFinSourceDO : IPlanFinSource
    {
        public PlanFinSourceDO() { }
        public FinancingType FinSource { get; set; }
        public int NumPlaces { get; set; }

        public PlanFinSourceDO(FinancingType fin, int num)
        {
            this.FinSource = fin;
            this.NumPlaces = num;
        }
    }
}
