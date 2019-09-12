using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BookStore.Repository;

namespace BookStore.Models
{
    public class ProviderSetting : Entity
    {
        [Key]
        [DisplayName("供應商類別代碼")]
        public int ProviderCodeId { get; set; }

        [DisplayName("供應商類別類別名稱")]
        [Required(ErrorMessage = "請輸入供應商類別名稱")]
        [StringLength(20, ErrorMessage = "供應商類別名稱不可超過20個字")]
        public string ProviderCodeName { get; set; }
    }
}