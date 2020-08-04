using System.Collections.Generic;

namespace PunasMarketing.ViewModels.Marketer
{
    public class MarketerMngtViewModel
    {
        public PunasMarketing.Models.DomainModel.Personnel personnel { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.Action> Actions { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.Section> sections { get; set; }
       
        public short[] ActionsId { get; set; }
        public short SectionId { get; set; }
    
    }
}