using System;
using System.Web.Mvc;

namespace Zkly.Common.Mvc.Attribute
{
    /// <summary>
    /// Support RedirectToAction(...) operation
    /// </summary>
    public class SetModelStateAttribute : ActionFilterAttribute
    {
        public string RedirectAction { get; set; }

        public string RedirectController { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var result = filterContext.Result as RedirectToRouteResult;
            if (result == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(RedirectAction)
                && !string.Equals(
                    this.RedirectAction,
                    result.RouteValues["action"].ToString(),
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (!string.IsNullOrEmpty(RedirectController)
                && !string.Equals(
                    this.RedirectController,
                    result.RouteValues["controller"].ToString(),
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            filterContext.Controller.TempData["ModelState"] = filterContext.Controller.ViewData.ModelState;
        }
    }
}
