//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdventureWorks.Services.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vendor
    {
        public int BusinessEntityId { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public byte CreditRating { get; set; }
        public bool PreferredVendorStatus { get; set; }
        public bool ActiveFlag { get; set; }
        public string PurchasingWebServiceUrl { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        public virtual BusinessEntity BusinessEntityIdObject { get; set; }
    }
}