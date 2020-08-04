using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Order
{
    public enum OrderPage
    {
        All,
        Verifying,
        Factoring
    }

    public class OrderListViewModel
    {
        public OrderPage OrdersPage { get; set; }
        public IQueryable<Models.DomainModel.Order> Orders { get; set; }
        public IQueryable<Models.DomainModel.Personnel> Marketers { get; set; }
        public IQueryable<Models.DomainModel.Customer> Customers { get; set; }
    }
}