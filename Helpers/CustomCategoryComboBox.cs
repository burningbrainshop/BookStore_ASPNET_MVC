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
    public static class CustomCategoryComboBox
    {
        public static MvcHtmlString CategoryComboBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            IRepository<CategorySetting> _repository = (IRepository<CategorySetting>)ServiceLocator.Resolve(typeof(Repository<CategorySetting>));
            var datas = _repository.GetAll();

            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model == null ? "" : ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model.ToString();

            TagBuilder tag = new TagBuilder("select");
            tag.MergeAttribute("data-val", "true");
            tag.MergeAttribute("data-val-required", "請選擇商品名稱");
            tag.MergeAttribute("name", fieldName);
            tag.MergeAttribute("id", fieldName);

            TagBuilder tagFirst = new TagBuilder("option");
            tagFirst.MergeAttribute("value", "");
            tagFirst.SetInnerText("---請選擇---");
            tag.InnerHtml += tagFirst.ToString();

            foreach (CategorySetting cs in datas)
            {
                TagBuilder tagOption = new TagBuilder("option");
                tagOption.MergeAttribute("value", cs.CategoryId.ToString());

                if (!string.IsNullOrEmpty(fieldValue))
                {
                    if (fieldValue == cs.CategoryId.ToString())
                        tagOption.MergeAttribute("selected", "true");
                }
                tagOption.SetInnerText(cs.CategoryName);
                tag.InnerHtml += tagOption.ToString();
            }

            return new MvcHtmlString(tag.ToString());
        }
    }
}