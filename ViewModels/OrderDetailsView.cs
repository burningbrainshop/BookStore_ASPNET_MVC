using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Models;

namespace BookStore.ViewModels
{
    public class OrderDetailsView
    {
        public OrderDetailsView()
        {
            OrderItems = new List<OrderItemsView>();
        }

        public int OrderId { get; set; }
        public Member MemberDetail { get; set; }
        public int StatusId { get; set; }
        public int Amount { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<OrderItemsView> OrderItems { get; set; }
    }
}