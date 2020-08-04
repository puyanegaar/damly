using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.ViewModels.Cheque
{
    public class ChequeDetailsViewModel
    {
        public Models.DomainModel.Cheque Cheque { get; set; }
        public IEnumerable<ChequeStatusMetaData> ChequeStatuses { get; set; }
        public List<Models.DomainModel.Sanad> ChequeSanads { get; set; }
        public string SettleAccountNum { get; set; }
        public string ChequeAccountNum { get; set; }
    }
}