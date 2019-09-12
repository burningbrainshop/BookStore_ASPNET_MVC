using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class NextBuy
    {
        [Key]
        public int NextId { get; set; }
        public int MemberId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Product productInfo { get; set; }
    }
}