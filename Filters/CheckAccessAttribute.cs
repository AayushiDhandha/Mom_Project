using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mom_Project.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CheckAccessAttribute : ActionFilterAttribute
    {
        private readonly string _sessionKey;

        // Default key based on your login module session storage
        public CheckAccessAttribute(string sessionKey = "UserID")
        {
            _sessionKey = sessionKey;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionValue = context.HttpContext.Session.GetString(_sessionKey);

            if (string.IsNullOrWhiteSpace(sessionValue))
            {
                // Not logged in -> move to Login page
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
