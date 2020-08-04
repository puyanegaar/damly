using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.InventoryChecking
{
    public class AddInventoryCheckingAjaxViewModel
    {
        public Models.DomainModel.InventoryChecking InventoryChecking { get; set; }
        public List<InventoryCheckingItem> InventoryCheckingItems { get; set; }
    }
}