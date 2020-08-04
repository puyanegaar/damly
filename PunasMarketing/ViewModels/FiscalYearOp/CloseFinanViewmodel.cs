using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.FiscalYearOp
{
    public class CloseFinanViewmodel
    {
      
        public PunasMarketing.Models.DomainModel.FiscalPeriod Fiscal { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

       
        public string DateToClose { get; set; }
    }
}