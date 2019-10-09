using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public enum PlanMode
    {
        Undefined = -1,
        FullTime = 0,
        Extramural = 1,
        PartTime = 2,
        Remote = 3
    }

    public static class PlanModeExt {
        public static PlanMode ToPlanMode(this string type)
        {
            switch (type)
            {
                case "full-time":
                    return PlanMode.FullTime;
                case "extramural":
                    return PlanMode.Extramural;
                case "remote":
                    return PlanMode.Remote;
                case "part-time":
                    return PlanMode.PartTime;
                default:
                    return PlanMode.Undefined;
            }
        }
    }
}
