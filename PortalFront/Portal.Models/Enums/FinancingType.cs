using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Portal.Models
{
    public enum FinancingType : Int64
    {
        None = 0,

        [Description("Бюджет")]
        Government = 281474976710657,

        [Description("Внебюджет")]
        Contract = 281474976710658,

        [Description("Целевой договор")]
        Enterprise = 281474976710661,

        [Description("Целевой договор (Моногорода)")]
        EnterpriseMono = 281474976710671,

        [Description("Целевой договор(ОПК)")]
        EnterpriseDIC = 281474976710666,//defense industry complex

        [Description("Особая квота")]
        Special = 281474976710667,

        [Description("Бюджет-иностранцы по направлению Мин.Обр.")]
        GovernmentForeign = 281474976710669,

        [Description("Целевой договор (в рамках интересов безопасности государства)")]
        EnterpriseNS = 281474976710672, //national security

        [Description("Без вступителных испытаний")]
        WithoutCompetition = 281474976710668,

        [Description("Внебюджет иностранцы")]
        ContractForeign = 281474976710665
    }

    public static class FinTypeExt
    {
        public static Tuple<string, int> ReducedValue(this IEnumerable<FinancingType> types)
        {
            if (types.Any(ft => ft == FinancingType.Enterprise ||
                ft == FinancingType.EnterpriseDIC ||
                ft == FinancingType.EnterpriseMono ||
                ft == FinancingType.EnterpriseNS)) return new Tuple<string, int>("Целевой договор", 6);

            if (types.Any(ft => ft == FinancingType.WithoutCompetition)) return new Tuple<string, int>("Особая квота", 5);

            if (types.Any(ft => ft == FinancingType.Special)) return new Tuple<string, int>("Особая квота", 4);

            if (types.Any(ft => ft == FinancingType.Government ||
                ft == FinancingType.GovernmentForeign ||
                ft == FinancingType.ContractForeign ||
                ft == FinancingType.Contract)) return new Tuple<string, int>("Общий конкурс", 2);

            throw new Exception("Unknown financing type");
        }
    }
}
