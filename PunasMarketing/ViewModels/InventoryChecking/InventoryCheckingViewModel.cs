using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.LocalModel;

namespace PunasMarketing.ViewModels.InventoryChecking
{
    public class InventoryCheckingViewModel
    {
        public Models.DomainModel.InventoryChecking InventoryChecking { get; set; }
        public IQueryable<Models.DomainModel.Warehouse> Warehouses { get; set; }
        public IQueryable<Models.DomainModel.Product> Products { get; set; }
        public IEnumerable<SelectListItem> SelectProducts { get; set; }

        public IQueryable<JsInventoryCheckingItems> JsInventoryCheckingItemses { get; set; }

        [Display(Name = "تاریخ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string StrCreatedDate { get; set; }
    }
}