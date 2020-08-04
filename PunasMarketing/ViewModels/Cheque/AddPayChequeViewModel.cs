using PunasMarketing.Models.DomainModel;
using System.Collections.Generic;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.ViewModels.Cheque
{
    public class AddPayChequeViewModel
    {
        //public AddPayChequeMetaData AddPayChequeMetaData { get; set; }
        public IEnumerable<ChequeStatusMetaData> ChequeStatuses { get; set; }
        public IEnumerable<Bank> Banks { get; set; }
        public IEnumerable<Models.DomainModel.BankAccount> BankAccounts { get; set; }
        public IEnumerable<Models.DomainModel.Supplier> Suppliers { get; set; }
        public IEnumerable<Models.DomainModel.Cost> Costs { get; set; }
    }
}

