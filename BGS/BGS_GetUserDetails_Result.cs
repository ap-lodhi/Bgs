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
    
    public partial class BGS_GetUserDetails_Result
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmpID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> DeptID { get; set; }
        public string EmailID { get; set; }
        public Nullable<bool> Isactive { get; set; }
        public Nullable<bool> Isforce { get; set; }
        public Nullable<bool> IsUTI { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<bool> DelStatus { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public Nullable<int> BrokerNameID { get; set; }
        public string RoleName { get; set; }
        public string DeptName { get; set; }
        public string Status { get; set; }
        public string UserType { get; set; }
        public Nullable<double> Weight { get; set; }
        public string BrokerName { get; set; }
        public Nullable<int> MaxLimit { get; set; }
    }
}