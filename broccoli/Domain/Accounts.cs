//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BroccoliTrade.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Accounts
    {
        public Accounts()
        {
            this.AccountPool = new HashSet<AccountPool>();
            this.TradingSystemPool = new HashSet<TradingSystemPool>();
            this.TradingSystems = new HashSet<TradingSystems>();
        }
    
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public long UserId { get; set; }
        public int StatusId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool IsNew { get; set; }
        public string Reason { get; set; }
        public int Broker { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<AccountPool> AccountPool { get; set; }
        public virtual ICollection<TradingSystemPool> TradingSystemPool { get; set; }
        public virtual ICollection<TradingSystems> TradingSystems { get; set; }
    }
}
