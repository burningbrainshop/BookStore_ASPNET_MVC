using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Helpers
{
    public static class ServiceComboBox
    {
        public static MvcHtmlString ServiceOnComboBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model == null ? "" : ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model.ToString();

            TagBuilder tag = new TagBuilder("select");
            tag.MergeAttribute("name", fieldName);
            tag.MergeAttribute("id", fieldName);

            TagBuilder tagFirst = new TagBuilder("option");
            tag.MergeAttribute("data-val", "true");
            tag.MergeAttribute("data-val-required", "請選擇是或否");
            tagFirst.MergeAttribute("value", "");
            tagFirst.SetInnerText("---請選擇---");
            tag.InnerHtml += tagFirst.ToString();

            TagBuilder tagY = new TagBuilder("option");
            tagY.MergeAttribute("value", "Y");
            tagY.SetInnerText("是");
            if (!string.IsNullOrEmpty(fieldValue))
            {
                if (fieldValue.Equals("Y"))
                    tagY.MergeAttribute("selected", "true");
            }
            tag.InnerHtml += tagY.ToString();

            TagBuilder tagN = new TagBuilder("option");
            tagN.MergeAttribute("value", "N");
            tagN.SetInnerText("否");
            if (!string.IsNullOrEmpty(fieldValue))
            {
                if (fieldValue.Equals("N"))
                    tagN.MergeAttribute("selected", "true");
            }
            tag.InnerHtml += tagN.ToString();

            return new MvcHtmlString(tag.ToString());
        }
    }
}