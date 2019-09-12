using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Repository;
using BookStore.Models;
using BookStore.Services;

namespace BookStore.Helpers
{
    public static class DisplayOrderStatucLabel
    {
        public static MvcHtmlString DisplayOrderStatus(this HtmlHelper htmlHelper, int statusId)
        {
            IRepository<OrderStatusSetting> _repository = (IRepository<OrderStatusSetting>)ServiceLocator.Resolve(typeof(Repository<OrderStatusSetting>));
            var datas = _repository.GetAll();

            TagBuilder tag = new TagBuilder("span");
            foreach (OrderStatusSetting os in datas)
            {
                if (statusId > 0)
                {
                    if (statusId == os.StatusId)
                        tag.SetInnerText(os.StatusName);
                }
            }
            return new MvcHtmlString(tag.ToString());
        }
    }
}