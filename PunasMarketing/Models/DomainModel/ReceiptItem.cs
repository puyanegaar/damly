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
    
    public partial class ReceiptItem
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public Nullable<int> TransactionId { get; set; }
        public Nullable<int> ChequeId { get; set; }
    
        public virtual Transaction Transaction { get; set; }
        public virtual Receipt Receipt { get; set; }
    }
}
