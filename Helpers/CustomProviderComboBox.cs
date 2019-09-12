using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BookStore.Repository;
using BookStore.Models;
using BookStore.Services;

namespace BookStore.Helpers
{
    public static class CustomProviderComboBox
    {
        public static MvcHtmlString ProviderComboBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            IRepository<ProviderSetting> _reposiroty = (IRepository<ProviderSetting>)ServiceLocator.Resolve(typeof(Repository<ProviderSetting>));
            var datas = _reposiroty.GetAll();

            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model == null ? "" : ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model.ToString();

            TagBuilder tag = new TagBuilder("select");
            tag.MergeAttribute("data-val", "true");
            tag.MergeAttribute("data-val-required", "請選擇供應商類別");
            tag.MergeAttribute("name", fieldName);
            tag.MergeAttribute("id", fieldName);

            TagBuilder tagFirst = new TagBuilder("option");
            tagFirst.MergeAttribute("value", "");
            tagFirst.SetInnerText("---請選擇---");
            tag.InnerHtml += tagFirst.ToString();

            foreach (ProviderSetting ps in datas)
            {
                TagBuilder tagOption = new TagBuilder("option");

                tagOption.MergeAttribute("value", ps.ProviderCodeId.ToString());
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    if (fieldValue == ps.ProviderCodeId.ToString())
                        tagOption.MergeAttribute("selected", "true");
                }

                tagOption.SetInnerText(ps.ProviderCodeName);
                tag.InnerHtml += tagOption.ToString();
            }

            return new MvcHtmlString(tag.ToString());
        }
    }
}