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
    public static class ProvidersComboBox
    {
        public static MvcHtmlString ProvidersListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string check)
        {
            IRepository<Provider> _repository = (IRepository<Provider>)ServiceLocator.Resolve(typeof(Repository<Provider>));
            var datas = _repository.GetAll();

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fieldValue = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model == null ? "" : ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model.ToString();

            TagBuilder tag = new TagBuilder("select");

            switch(check)
            {
                case "N":
                    string providerName = "";

                    tag.MergeAttribute("name", fieldName);
                    tag.MergeAttribute("id", fieldName);

                    foreach(Provider p in datas)
                    {
                        if (p.ProviderId.Equals(int.Parse(fieldValue)))
                        {
                            providerName = p.Name;                            
                            break;
                        }
                    }

                    TagBuilder singleOption = new TagBuilder("option");
                    singleOption.MergeAttribute("value", fieldValue);
                    singleOption.SetInnerText(providerName);
                    tag.InnerHtml += singleOption.ToString();
                    break;
                default:
                    tag.MergeAttribute("data-val", "true");
                    tag.MergeAttribute("data-val-required", "請選擇供應商名稱");
                    tag.MergeAttribute("name", fieldName);
                    tag.MergeAttribute("id", fieldName);

                    TagBuilder tagFirst = new TagBuilder("option");
                    tagFirst.MergeAttribute("value", "");
                    tagFirst.SetInnerText("---請選擇---");
                    tag.InnerHtml += tagFirst.ToString();

                    foreach (Provider p in datas)
                    {
                        if (p.Service.Equals("Y"))
                        {
                            TagBuilder tagOption = new TagBuilder("option");
                            tagOption.MergeAttribute("value", p.ProviderId.ToString());

                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                if (fieldValue == p.ProviderId.ToString())
                                    tagOption.MergeAttribute("selected", "true");
                            }
                            tagOption.SetInnerText(p.Name);
                            tag.InnerHtml += tagOption.ToString();
                        }
                    }
                    break;
            }
            return new MvcHtmlString(tag.ToString());
        }
    }
}