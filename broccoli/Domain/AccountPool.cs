//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BroccoliTrade.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class AccountPool
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public System.DateTime ApplicationDate { get; set; }
        public string AccountNumber { get; set; }
        public int AccountId { get; set; }
    
        public virtual Accounts Accounts { get; set; }
        public virtual Users Users { get; set; }
    }
}
