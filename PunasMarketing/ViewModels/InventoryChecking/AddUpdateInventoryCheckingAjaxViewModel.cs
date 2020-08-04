using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.InventoryChecking
{
    public class AddUpdateInventoryCheckingAjaxViewModel
    {
        public Models.DomainModel.InventoryChecking InventoryChecking { get; set; }
        public List<InventoryCheckingItem> InventoryCheckingItems { get; set; }

        [Display(Name = "تاریخ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string StrCreatedDate { get; set; }
    }
}