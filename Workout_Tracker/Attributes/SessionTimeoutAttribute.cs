using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Workout_Tracker.Attributes
{

    public class SessionTimeoutAttribute : ActionFilterAttribute
        {
            
         

            public override void OnActionExecuting(ActionExecutingContext filterContext)
               {

                var sessUserId = filterContext.HttpContext.Session.GetString("userId");
                    if (sessUserId == null)
                    {
                        
                //if no session cookie named "ID" is found, the app will redirect to the home page
                        filterContext.Result = new RedirectResult("~/Home/Index");
                        return;
                    }
                    base.OnActionExecuting(filterContext);
                }
        }
    
}
