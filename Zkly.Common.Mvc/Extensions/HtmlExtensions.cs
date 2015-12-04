using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Zkly.Common.Mvc.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString DisplaySuccessMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return DisplaySuccessMessageFor(htmlHelper, expression, null, new RouteValueDictionary());
        }

        public static MvcHtmlString DisplaySuccessMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage)
        {
            return DisplaySuccessMessageFor(htmlHelper, expression, validationMessage, new RouteValueDictionary());
        }

        public static MvcHtmlString DisplaySuccessMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes)
        {
            return DisplaySuccessMessageFor(htmlHelper, expression, validationMessage, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString DisplaySuccessMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, IDictionary<string, object> htmlAttributes)
        {
            return ValidationMessageHelper(htmlHelper, ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText(expression), validationMessage, htmlAttributes);
        }

        private static MvcHtmlString ValidationMessageHelper(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, string expression, string validationMessage, IDictionary<string, object> htmlAttributes)
        {
            var modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
            FormContext formContext = htmlHelper.ViewContext.ClientValidationEnabled ? htmlHelper.ViewContext.FormContext : null;

            if (!htmlHelper.ViewData.ModelState.ContainsKey(modelName) && formContext == null)
            {
                return null;
            }

            var modelState = htmlHelper.ViewData.ModelState[modelName];

            if (modelState == null || (modelState != null && (modelState.Errors == null || modelState.Errors.Count > 0)))
            {
                return null;
            }

            var builder = new TagBuilder("span");
            builder.MergeAttributes(htmlAttributes);
            builder.AddCssClass(HtmlHelper.ValidationMessageValidCssClassName);

            if (!string.IsNullOrEmpty(validationMessage))
            {
                builder.SetInnerText(validationMessage);
            }

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
    }
}
