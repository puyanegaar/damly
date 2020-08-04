using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.ViewModels.Cheque
{
    public class ChequeListViewModel
    {
        public IEnumerable<Models.DomainModel.Cheque> Cheques { get; set; }
        public IEnumerable<ChequeStatusMetaData> ChequeStatuses { get; set; }
        public IEnumerable<Bank> Banks { get; set; }
        public IEnumerable<Models.DomainModel.BankAccount> Accounts  { get; set; }
    }
}