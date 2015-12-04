using System;
using System.Web.Mvc;

namespace Zkly.Common.Mvc.Attribute
{
    public class RestoreViewBagAttribute : ActionFilterAttribute
    {
         private readonly string[] _keys;

         public RestoreViewBagAttribute(params string[] keys)
        {
            if (keys.Length == 0)
            {
                throw new ArgumentNullException("keys");
            }

             this._keys = keys;
        }

       public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            foreach (var key in _keys)
            {
                object value;
                if (filterContext.Controller.TempData.TryGetValue(key, out value))
                {
                    filterContext.Controller.ViewData[key] = value;
                }
            }
        }
    }
}
