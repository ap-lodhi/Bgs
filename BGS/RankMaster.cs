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
    
    public partial class RankMaster
    {
        public int RankId { get; set; }
        public Nullable<int> BrokerNameID { get; set; }
        public Nullable<int> QuarterID { get; set; }
        public Nullable<int> VoteId { get; set; }
        public Nullable<int> MaxLimit { get; set; }
        public Nullable<int> Rank { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> DelStatus { get; set; }
        public Nullable<double> TotalScores { get; set; }
    
        public virtual BrokerNameMaster BrokerNameMaster { get; set; }
        public virtual QuarterMaster QuarterMaster { get; set; }
    }
}
