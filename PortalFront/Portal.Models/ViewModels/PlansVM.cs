using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{



    public class PlansVM : IPlansVM
    {
        public PlanMode Mode { get; set; }

        public IEnumerable<IInstitute> Institutes { get; set; }
    }

    public static class PlansViewModelExt
    {
        public static IEnumerable<IPlansVM> MakeViewModels(this IEnumerable<IPlanEntry> plans)
        {
            var modes = plans.Select(p => p.Mode).Distinct(); //

            var plansViewModels = new List<IPlansVM>(modes.Count());

            foreach (var mode in modes)
            {
                var pvm = plans
                    .Where(p => p.Mode == mode)
                    .ToPlansViewModel();

                plansViewModels.Add(pvm);
            }

            return plansViewModels;
        }
        public static PlansVM ToPlansViewModel(this IEnumerable<IPlanEntry> plans)
        {
            var pvm = new PlansVM
            {
                Mode = plans.First().Mode,
            };

            var distinctInstitutes = plans.Select(p => p.Institute as Institute).Distinct().ToArray();//distinct requires overriding of Equals
            var institutesVm = new List<Institute>(distinctInstitutes.Count());

            foreach (var institute in distinctInstitutes)
            {
                var instPlans = plans.Where(p => p.Institute.Id == institute.Id);
                institute.SetPlans(instPlans);
                institutesVm.Add(institute);
            }

            pvm.Institutes = institutesVm;

            return pvm;
        }
    }
}
