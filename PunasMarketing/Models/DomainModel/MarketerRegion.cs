//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PunasMarketing.Models.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class MarketerRegion
    {
        public int Id { get; set; }
        public int MarketerId { get; set; }
        public short RegionId { get; set; }
    
        public virtual Personnel Personnel { get; set; }
        public virtual Region Region { get; set; }
    }
}
