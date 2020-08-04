using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.BankAccount
{
    public class BankAccountViewModel
    {
        public IEnumerable<Bank> Banks { get; set; }
        public Models.DomainModel.BankAccount BankAccountModel { get; set; }
    }
}