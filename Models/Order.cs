using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int StatusId { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Member memberInfo { get; set; }
        public virtual List<OrderDetail> detailsInfo { get; set; }
    }
}