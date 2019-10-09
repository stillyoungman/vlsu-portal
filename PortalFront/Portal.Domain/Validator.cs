using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Domain
{
    /// <summary>
    /// на фронте есть валидация, но данный валидатор предназначен для того чтобы защитить страничку от разного рода атак.
    /// </summary>
    public class Validator : IPortalValidator
    {
        private const byte UNIQUE_CODES_COUNT = 3;
        private const byte MIN_RAW_FIN_TYPES_LENGHT = 3; //с-0

        public bool ValidatePlans(IDictionary<long,IPlanEntry> plans, IEnumerable<long> plansId, NewAbitModel abit)
        {
            if (abit.RawFinTypes.Length < MIN_RAW_FIN_TYPES_LENGHT 
                || abit.FinTypes == null) return false;

            IPlanEntry plan;
            var selectedPlans = new List<IPlanEntry>(plansId.Count());

            foreach (var id in plansId)
            {
                if (!plans.TryGetValue(id, out plan)) return false;

                selectedPlans.Add(plan);
            }

            if (!ValidatePlanSpots(plans, abit.FinTypes, plansId)) return false;

            if (selectedPlans.Select(p => p.Title.Code).Distinct().Count() > UNIQUE_CODES_COUNT) return false;

            return true;
        }

        private bool ValidatePlanSpots(IDictionary<long, IPlanEntry> plans, IEnumerable<Tuple<int, FinancingType>> finTypes, IEnumerable<long> plansId)
        {
            //тут только проверка есть ли бюджетные места
            try
            {
                if (!finTypes.Any(f => f.Item2 == FinancingType.Government)) return true;

                foreach(var i in finTypes.Where(f => f.Item2 == FinancingType.Government))
                {
                    if (plans[plansId.ElementAt(i.Item1)].GrantSpots == 0) return false;
                }

                return true;
            } catch
            {
                return false;
            }
            
        }
    }

    public interface IPortalValidator
    {
        bool ValidatePlans(IDictionary<long, IPlanEntry> plans, IEnumerable<long> plansId, NewAbitModel abit);
    }
}
