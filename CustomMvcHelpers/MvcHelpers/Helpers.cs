using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace CustomMvcHelpers.MvcHelpers
{
    public static class Helpers
    {
        /// <summary>
        ///     Create Lable with Required Hint
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="html">Helper method attach to HtmlHelper object</param>
        /// <param name="expression">Linq Expression</param>
        /// <param name="htmlAttributes">Html Attributes</param>
        /// <returns></returns>
        public static MvcHtmlString LabelRequiredFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            object htmlAttributes = null)
        {
            // Returns the metadata for the given expression from the model
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            //Gets the model name from an expression.
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("label");
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);

            if (metadata.IsRequired)
            {
                var sup = new TagBuilder("sup");
                sup.SetInnerText("*");
                tag.InnerHtml += sup;
            }

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}
