using Microsoft.AspNetCore.Mvc;
using Portal.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal
{
    public class TopButtons : ViewComponent
    {
        private IDataStore store;
        public TopButtons(IDataStore store)
        {
            this.store = store;
        }

        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shared/_TopButtonsParitalView.cshtml", store.IsOnlineRegistrationAvailable);
        }
    }
}
