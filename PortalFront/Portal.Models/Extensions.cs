using Portal.Models.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace Portal.Models
{
    public static class Extensions
    {
        public static string ToYesNo(this bool value) => value ? "Да" : "Нет";

        public static int GetSpotsByFin(this IEnumerable<IPlanFinSource> source, FinancingType fin) => source.SingleOrDefault(fs => fs.FinSource == fin)?.NumPlaces ?? 0;
    }
}
