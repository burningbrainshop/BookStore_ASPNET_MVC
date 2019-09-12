using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace BookStore.Models
{
    public class Cart
    {
        [Required]
        public Product productCart { get; set; }

        [Required]
        public int quantity { get; set; }
    }
}