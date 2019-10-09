using System;
using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using Portal.DataLayer;
using Portal.Domain;

namespace Portal.Controllers
{
    public class AppController : Controller
    {
        private readonly IDataStore store;
        private readonly IPortalValidator validator;

        public AppController(IDataStore store, IPortalValidator validator)
        {
            this.store = store;
            this.validator = validator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("courses")]
        public IActionResult Plans(string type)
        {
            var mode = type.ToPlanMode();
            if (mode == PlanMode.Undefined)
            {
                return View("Error404");
            }
            else
            {
                return View(store.GetPlansVM(mode));
            }
        }

        [HttpGet("enrollees/{id}")]
        public IActionResult Enrollees(long id)
        {
            var m = store.GetPlanEnrolleesVM(id);

            if (m == null) return View("Error404");

            return View(m);
        }

        [HttpGet("application")]
        public IActionResult Application()
        {
            if (!store.IsOnlineRegistrationAvailable)
            {
                return View("Error404");
            } else
            {
                return View(store.ApplicationVM);
            }
            
        }

        [ValidateAntiForgeryToken]
        [HttpPost("application")]
        public IActionResult ApplicationPost([FromForm] NewAbitModel abit)
        {
            if (!validator.ValidatePlans(store.Plans, abit.PickedPlans, abit)) return BadRequest();

            ViewData["applicationId"] = store.CreateAbit($"{abit.LastName} {abit.FirstName} {abit.MiddleName}", abit.SerializationString);

            return View("ApplicationSuccess");
        }



        [Route("{*url}", Order = 998)]
        public IActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }


        public IActionResult Error500()
        {
            if (Response.StatusCode != 500) return Redirect("/404");
            return View();
        }
    }
}
