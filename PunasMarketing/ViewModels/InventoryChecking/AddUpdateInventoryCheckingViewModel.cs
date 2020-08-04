using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.InventoryChecking
{
    public class AddUpdateInventoryCheckingViewModel
    {
        public Models.DomainModel.InventoryChecking InventoryChecking { get; set; }
        public IQueryable<Models.DomainModel.Warehouse> Warehouses { get; set; }
        public IQueryable<Models.DomainModel.Product> Products { get; set; }

        public InventoryCheckingItem InventoryCheckingItem { get; set; }
    }
}