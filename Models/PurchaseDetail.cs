using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class PurchaseDetail
    {

        [Key]
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int Cost { get; set; }
        public int Quantity { get; set; }

        public virtual Product productInfo { get; set; }
    }
}