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
    public static class CustomOrderStatusComboBox
    {
        public static MvcHtmlString OrderStatusComboBox(this HtmlHelper htmlHelper, int statusId)
        { 
            IRepository<OrderStatusSetting> _repository = (IRepository<OrderStatusSetting>)ServiceLocator.Resolve(typeof(Repository<OrderStatusSetting>));
            var datas = _repository.GetAll();

            TagBuilder tag = new TagBuilder("select");
            tag.MergeAttribute("name", "StatusId");
            tag.MergeAttribute("id", "StatusId");

            foreach(OrderStatusSetting os in datas)
            {
                TagBuilder tagOption = new TagBuilder("option");

                tagOption.MergeAttribute("value", os.StatusId.ToString());
                if (statusId > 0)
                {
                    if (statusId == os.StatusId)
                        tagOption.MergeAttribute("selected", "true");
                }

                tagOption.SetInnerText(os.StatusName);
                tag.InnerHtml += tagOption.ToString();
            }

            return new MvcHtmlString(tag.ToString());
        }
    }
}