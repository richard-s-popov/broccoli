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
    
    public partial class TradingSystems
    {
        public TradingSystems()
        {
            this.TradingSystemPool = new HashSet<TradingSystemPool>();
        }
    
        public int Id { get; set; }
        public int SystemId { get; set; }
        public long UserId { get; set; }
        public int StatusId { get; set; }
        public int AccountId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> ModificationDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }
    
        public virtual Accounts Accounts { get; set; }
        public virtual Status Status { get; set; }
        public virtual Systems Systems { get; set; }
        public virtual ICollection<TradingSystemPool> TradingSystemPool { get; set; }
        public virtual Users Users { get; set; }
    }
}
