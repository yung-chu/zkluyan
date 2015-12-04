using System;
using System.Web.Mvc;

namespace Zkly.Common.Mvc.Attribute
{
    /// <summary>
    /// Support RedirectToAction(...) operation
    /// </summary>
    public class SetViewBagAttribute : ActionFilterAttribute
    {
        private readonly string[] _keys;

        public SetViewBagAttribute(params string[] keys)
        {
            if (keys.Length == 0)
            {
                throw new ArgumentNullException("keys");
            }

            _keys = keys;
        }

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

            if (!string.IsNullOrEmpty(this.RedirectAction)
                && !string.Equals(
                    this.RedirectAction,
                    result.RouteValues["action"].ToString(),
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (!string.IsNullOrEmpty(this.RedirectController)
                && !string.Equals(
                    this.RedirectController,
                    result.RouteValues["controller"].ToString(),
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            foreach (var key in _keys)
            {
                object value;
                if (filterContext.Controller.ViewData.TryGetValue(key, out value))
                {
                    filterContext.Controller.TempData[key] = value;
                }
            }
        }
    }
}
