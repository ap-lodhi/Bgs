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
    
    public partial class StatusMaster
    {
        public StatusMaster()
        {
            this.BrokerAnalysts = new HashSet<BrokerAnalyst>();
            this.GradingMasters = new HashSet<GradingMaster>();
        }
    
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> DelStatus { get; set; }
    
        public virtual ICollection<BrokerAnalyst> BrokerAnalysts { get; set; }
        public virtual ICollection<GradingMaster> GradingMasters { get; set; }
    }
}