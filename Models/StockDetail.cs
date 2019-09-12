using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class StockDetail
    {
        [Key]
        public int Id { get; set; }
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int Cost { get; set; }
        public int Quantity { get; set; }
        public int ReturnQuantity { get; set; }

        public virtual Product productInfo { get; set; }
    }
}