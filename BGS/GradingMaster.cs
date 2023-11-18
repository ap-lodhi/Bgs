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
    
    public partial class GradingMaster
    {
        public GradingMaster()
        {
            this.VoteMasters = new HashSet<VoteMaster>();
        }
    
        public int GradingID { get; set; }
        public Nullable<int> QuarterID { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string EmpID { get; set; }
        public Nullable<bool> IsConfirm { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> DelStatus { get; set; }
        public Nullable<bool> IsSend { get; set; }
        public Nullable<bool> IsVoterSubmit { get; set; }
    
        public virtual QuarterMaster QuarterMaster { get; set; }
        public virtual StatusMaster StatusMaster { get; set; }
        public virtual ICollection<VoteMaster> VoteMasters { get; set; }
    }
}
