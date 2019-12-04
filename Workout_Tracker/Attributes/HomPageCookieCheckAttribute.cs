using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Workout_Tracker.Attributes
{
    public class HomPageCookieCheckAttribute:ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var sessUserId = filterContext.HttpContext.Session.GetString("userId");
            if (sessUserId != null)
            {

                //if no session cookie named "ID" is found, the app will redirect to the home page
                filterContext.Result = new RedirectResult("~/Home/HomePage");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }

}

