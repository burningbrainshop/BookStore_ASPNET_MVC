using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Repository
{
    public class OrderRepository : Repository<Order>
    {
        public List<OrderTitleView> GetOrderTitleList(int memberId, int peroidTimeValue)
        {
            if (memberId.Equals(0))
            {
                var startTime = GetStartTime(peroidTimeValue);
                var endTime = DateTime.Now.AddDays(1);
                var datas = _table.Where(d => d.CreatedOn >= startTime && d.CreatedOn < endTime);
                List<OrderTitleView> titleView = new List<OrderTitleView>();
                foreach (var item in datas)
                {
                    _context.Entry(item).Collection(o => o.detailsInfo);
                    titleView.Add(new OrderTitleView { OrderId = item.OrderId, MemberId = item.MemberId, StatusId = item.StatusId, Amount = item.Amount, ItemCount = item.detailsInfo.Count(), CreatedOn = item.CreatedOn });
                }
                return titleView;
            }
            else
            {
                var startTime = GetStartTime(peroidTimeValue);
                var endTime = DateTime.Now.AddDays(1);
                var datas = SearchFor(o => o.MemberId.Equals(memberId)).Where(d => d.CreatedOn >= startTime && d.CreatedOn < endTime);
                List<OrderTitleView> titleView = new List<OrderTitleView>();
                foreach (var item in datas)
                {
                    _context.Entry(item).Collection(o => o.detailsInfo);
                    titleView.Add(new OrderTitleView { OrderId = item.OrderId, MemberId = item.MemberId, StatusId = item.StatusId, Amount = item.Amount, ItemCount = item.detailsInfo.Count(), CreatedOn = item.CreatedOn });
                }
                return titleView;
            }
        }

        public OrderDetailsView GetOrderDetails(int orderId)
        {
            var data = GetById(orderId);
            _context.Entry(data).Reference(m => m.memberInfo);
            _context.Entry(data).Collection(o => o.detailsInfo);

            OrderDetailsView detailView = new OrderDetailsView();
            detailView.OrderId = data.OrderId;
            detailView.MemberDetail = data.memberInfo;
            detailView.StatusId = data.StatusId;
            detailView.Amount = data.Amount;
            detailView.ItemCount = data.detailsInfo.Count();
            detailView.CreatedOn = data.CreatedOn;
            
            foreach(var item in data.detailsInfo)
            {
                _context.Entry(item).Reference(od => od.productInfo);
                detailView.OrderItems.Add(new OrderItemsView { singleProduct = item.productInfo, Price = item.Price, Quantity = item.Quantity });
            }

            return detailView;
        }

        private DateTime GetStartTime(int periodTimeValue)
        { 
            DateTime sDate = new DateTime();
            switch(periodTimeValue)
            {
                case 1:
                    sDate = DateTime.Now.AddMonths(-3);
                    break;
                case 2:
                    sDate = DateTime.Now.AddMonths(-6);
                    break;
                case 3:
                    sDate = DateTime.Now.AddMonths(-12);
                    break;
                default:
                    sDate = DateTime.Now.AddMonths(-3);
                    break;
            }
            return sDate;
        }
    }
}