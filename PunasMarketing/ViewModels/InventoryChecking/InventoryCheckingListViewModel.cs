using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.InventoryChecking
{
    public class InventoryCheckingListViewModel
    {
        public IQueryable<Models.DomainModel.InventoryChecking> InventoryCheckings { get; set; }
        public IQueryable<Models.DomainModel.Warehouse> Warehouses { get; set; }
        public IQueryable<Models.DomainModel.InventoryCheckingItem> Items { get; set; }
    }
}