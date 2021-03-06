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
    
    public partial class Sanad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sanad()
        {
            this.SanadItems = new HashSet<SanadItem>();
            this.Factors = new HashSet<Factor>();
        }
    
        public int Id { get; set; }
        public short PeriodId { get; set; }
        public string Description { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsAutomatic { get; set; }
        public byte SanadTypeId { get; set; }
        public int CreatedByUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ConfirmDate { get; set; }
        public decimal TotalAmount { get; set; }
    
        public virtual FiscalPeriod FiscalPeriod { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanadItem> SanadItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Factor> Factors { get; set; }
    }
}
