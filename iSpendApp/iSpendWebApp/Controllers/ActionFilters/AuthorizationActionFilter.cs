using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace iSpendWebApp.Controllers.ActionFilters
{
    public class AuthorizationActionFilter : IActionFilter
    {

        public  void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("UserSession") != null) return;
            var controller = (ControllerBase) filterContext.Controller;
            filterContext.Result = controller.RedirectToAction("Login", "User");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
            return;
        }
    }
}
