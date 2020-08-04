using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Order
{
    public class OrderDetailsViewModel
    {
        public Models.DomainModel.Order Order { get; set; }
        public IQueryable<OrderItem> Items { get; set; }
        public IEnumerable<KeyValuePair<byte, string>> ProductionStatuses { get; set; }
    }
}