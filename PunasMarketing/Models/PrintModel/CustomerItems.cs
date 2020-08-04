using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.PrintModel
{
    public class CustomerItems
    {
        public string Bedeh { get; set; }
        public string Bestan { get; set; }
        public System.DateTime ConfirmDate { get; set; }
        public string Description { get; set; }
        public string TafsiliName { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Name { get; set; }
        public string Coding { get; set; }
        public int SandId { get; set; }
        
    }
}