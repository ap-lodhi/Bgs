//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BGS
{
    using System;
    using System.Collections.Generic;
    
    public partial class BrokerTypeMaster
    {
        public BrokerTypeMaster()
        {
            this.BrokerMasters = new HashSet<BrokerMaster>();
        }
    
        public int BrokerTypeID { get; set; }
        public string BrokerType { get; set; }
        public Nullable<int> BrokerTypeValue { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual ICollection<BrokerMaster> BrokerMasters { get; set; }
    }
}
