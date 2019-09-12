﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class RoleInApplication
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int AppId { get; set; }
    }
}