using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BGS
{
    public class Datalayer
    {
        private static SqlConnection conn;
        private static SqlCommand cmd;
        private string strconn = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
        public DataTable ExecuteQuery(string cQuery)
        {
            DataTable dt = new DataTable();
            string strconn = ConfigurationManager.AppSettings.Get("ConnectionString").ToString();
            String strQuery = String.Empty;
            conn = new SqlConnection(strconn);

            try
            {
                conn.Open();
                cmd = new SqlCommand(cQuery, conn);
                SqlDataAdapter SqDA = new SqlDataAdapter(cmd);
                SqDA.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                //DecisionLayer DL = new DecisionLayer();
                //DL.WriteToEventLog(ex, DL.GetErrorDetails(ex) + Environment.NewLine + " Query=" + strQuery);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataSet ExecuteProcedure(string cProcedureName, SqlParameter[] paramenters)
        {
            DataSet ds = new DataSet();
            string strconn = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            String strQuery = String.Empty;
            conn = new SqlConnection(strconn);

            try
            {
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = cProcedureName;
                if (paramenters != null)
                    cmd.Parameters.AddRange(paramenters);
                SqlDataAdapter SqDA = new SqlDataAdapter(cmd);
                SqDA.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //DecisionLayer DL = new DecisionLayer();
                //DL.WriteToEventLog(ex, DL.GetErrorDetails(ex) + Environment.NewLine + " Query=" + strQuery);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public string ExecuteSclar(string cProcedureName, SqlParameter[] paramenters)
        {
            DataSet ds = new DataSet();
            string strconn = ConfigurationManager.AppSettings.Get("ConnectionString").ToString();
            String strQuery = String.Empty;
            conn = new SqlConnection(strconn);

            try
            {
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure; ;
                cmd.CommandText = cProcedureName;
                if (paramenters != null)
                    cmd.Parameters.AddRange(paramenters);

                object obj = cmd.ExecuteScalar();

                if (obj != null)
                    return obj.ToString();
                else
                    return "";
            }

            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        #region UserModule

        public DataTable GetUsers(string empId)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetUserDetails", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@EMPID", empId);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable GetBrokerMaster(string empId, string brokerNameSearch)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetBrokerMaster", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@EMPID", empId);
                            cmd.Parameters.AddWithValue("@BrokerNameSearch", brokerNameSearch);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable GetRoles()
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetRoleDeptDetails", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable IsAuthenticateBroker(string userId, string password)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_IsAuthenticateBroker", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmpId", userId);
                            cmd.Parameters.AddWithValue("@Password", password);
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable GetDepartments()
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetRoleDeptDetails", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds.Tables[1];
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DataTable GetUserTypes(string strLoginType)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetUserTypes", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@LOGINTYPE", strLoginType);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAssignScreen(string strEmployee)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetAsssignScreens", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@EMPID", strEmployee);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AssignScreenToUser(string empId, int screenId, int moduleId, int screenAssignId, bool isActive, string createdBy)
        {
            String strQuery = String.Empty;
            DataSet ds = new DataSet();
            conn = new SqlConnection(strconn);
            conn.Open();
            try
            {
                cmd = new SqlCommand("BGS_AssignScreenToUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(empId))
                {
                    cmd.Parameters.Add("@ScreenId", SqlDbType.VarChar, 20).Value = screenId;
                    cmd.Parameters.Add("@ModuleId", SqlDbType.VarChar, 20).Value = moduleId;
                    cmd.Parameters.Add("@EmployeeId", SqlDbType.VarChar, 20).Value = empId;
                    cmd.Parameters.Add("@ScreenAssignId", SqlDbType.VarChar, 20).Value = screenAssignId;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 20).Value = createdBy;
                    cmd.Parameters.Add("@Active", SqlDbType.Bit, 4).Value = isActive;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AssignScreenLinking(string empId)
        {
            String strQuery = String.Empty;
            DataSet ds = new DataSet();
            conn = new SqlConnection(strconn);
            conn.Open();
            try
            {
                cmd = new SqlCommand("BGS_AssignScreen_linking", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(empId))
                {
                    cmd.Parameters.Add("@EmployeeId", SqlDbType.VarChar, 20).Value = empId;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void InsertUpdateUser(string EmpID, string strPwd, int intRoleID, int intDeptID, Boolean blIsActive, string strEmailID, string strName, Boolean blIsForce, Boolean blIsUTI, string strCreatedBy, float fltWeight, int intUserType, string BrokerName, int MaxLimit, int DofaCode, int BrokerNameID, int UserID)
        {
            // int intRow = 0;
            conn = new SqlConnection(strconn);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "BGS_InsertUpdateUsers";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EMPLOYEEID", SqlDbType.VarChar, 50).Value = EmpID;
            cmd.Parameters.Add("@USERPWD", SqlDbType.VarChar, 20).Value = strPwd;
            if (intRoleID > 0)
            {
                cmd.Parameters.Add("@ROLEID", SqlDbType.Int).Value = intRoleID;
            }
            if (intDeptID > 0)
            {
                cmd.Parameters.Add("@DEPTID", SqlDbType.Int).Value = intDeptID;
            }
            cmd.Parameters.Add("@ISACITVE", SqlDbType.Bit).Value = blIsActive;
            cmd.Parameters.Add("@EMAILID", SqlDbType.VarChar, 50).Value = strEmailID;
            cmd.Parameters.Add("@NAME", SqlDbType.VarChar, 50).Value = strName;
            cmd.Parameters.Add("@ISFORCE", SqlDbType.Bit).Value = blIsForce;
            cmd.Parameters.Add("@ISUTI", SqlDbType.Bit).Value = blIsUTI;
            cmd.Parameters.Add("@CREATEDBY", SqlDbType.VarChar, 20).Value = strCreatedBy;
            cmd.Parameters.Add("@WEIGHT", SqlDbType.Float).Value = fltWeight;
            cmd.Parameters.Add("@USERTYPE", SqlDbType.Int).Value = intUserType;
            cmd.Parameters.Add("@BrokerName", SqlDbType.VarChar, 100).Value = BrokerName;
            cmd.Parameters.Add("@MaxLimit", SqlDbType.Int).Value = MaxLimit;
            cmd.Parameters.Add("@DofaCode", SqlDbType.Int).Value = DofaCode;
            cmd.Parameters.Add("@BrokerNameID", SqlDbType.Int).Value = BrokerNameID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
                //DecisionLayer DL = new DecisionLayer();
                //DL.WriteToEventLog(ex, DL.GetErrorDetails(ex) + " User Active Status Record " + ex.Message + Environment.NewLine);
            }
            finally
            {
                conn.Close();
            }
        }

        public void ActiveDeactiveUser(string EmpID, Boolean blIsActive, Boolean blIsForce, string strCreatedBy)
        {
            // int intRow = 0;  (blIsActive == true) ? 1 : 0)
            conn = new SqlConnection(strconn);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "BGS_User_StatusChange";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@EMPLOYEEID", SqlDbType.VarChar, 50).Value = EmpID;
            cmd.Parameters.Add("@ISACITVE", SqlDbType.Bit).Value = Convert.ToBoolean(blIsActive);
            cmd.Parameters.Add("@ISFORCE", SqlDbType.Bit).Value = blIsForce;
            cmd.Parameters.Add("@CREATEDBY", SqlDbType.VarChar, 50).Value = strCreatedBy;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //DecisionLayer DL = new DecisionLayer();
                //DL.WriteToEventLog(ex, DL.GetErrorDetails(ex) + " User Active Status Record " + ex.Message + Environment.NewLine);
            }
            finally
            {
                conn.Close();
            }
        }

        public decimal GetWeight(string empId)
        {
            decimal weight = 0;
            DataSet ds = new DataSet();
            conn = new SqlConnection(strconn);
            conn.Open();
            try
            {
                cmd = new SqlCommand("Select dbo.GetWeight(@EmployeeId)", conn);
                cmd.CommandType = CommandType.Text;
                if (!string.IsNullOrEmpty(empId))
                {
                    cmd.Parameters.Add("@EmployeeId", SqlDbType.VarChar, 20).Value = empId;
                    weight = Convert.ToDecimal(cmd.ExecuteScalar().ToString());
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return weight;

        }

        public DataSet GetGradingDeptWise(string Dept)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetGradingMaster", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@DeptID", Dept);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetCuurentQuarterVoterStatus(int QuarterID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetCuurentQuarterVoterStatus", conn))
                    {

                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@quarterID ", QuarterID);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertRank(int BrokerID, int QuarterID, int VoteId, int MaxLimit, int Rank, string CreatedBy, double totalScore)
        {
            conn = new SqlConnection(strconn);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "BGS_InsertRank";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BrokerNameID", SqlDbType.Int).Value = BrokerID;
            cmd.Parameters.Add("@QuarterID", SqlDbType.Int).Value = QuarterID;
            cmd.Parameters.Add("@VoteID", SqlDbType.Int).Value = VoteId;
            cmd.Parameters.Add("@MaxLimit", SqlDbType.Int).Value = MaxLimit;
            cmd.Parameters.Add("@Rank", SqlDbType.Int).Value = Rank;
            cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 100).Value = CreatedBy;
            cmd.Parameters.Add("@totalScore", SqlDbType.Float).Value = totalScore;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        public void InsertBrokerConfigforBrokerEmail(int BrokerID, int QuarterID, bool isEmailSend)
        {
            conn = new SqlConnection(strconn);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "BGS_InsertBrokerConfigforBrokerEmail";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BrokerNameID", SqlDbType.Int).Value = BrokerID;
            cmd.Parameters.Add("@QuarterID", SqlDbType.Int).Value = QuarterID;
            cmd.Parameters.Add("@EmailStatus", SqlDbType.Bit).Value = isEmailSend;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        public string ChagePassword(string empId, string OldPassword, string NewPassword)
        {
            string message = string.Empty;
            DataSet ds = new DataSet();
            conn = new SqlConnection(strconn);
            try
            {
                conn.Open();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd = new SqlCommand("BGS_ChangePassword", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", empId);
                cmd.Parameters.AddWithValue("@OldPassword", OldPassword);
                cmd.Parameters.AddWithValue("@NewPassword", NewPassword);
                cmd.Parameters.Add("@Message", SqlDbType.VarChar, 100);
                cmd.Parameters["@Message"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                message = (string)cmd.Parameters["@Message"].Value;
                conn.Close();

            }
            catch (Exception)
            {
                throw;
            }
            return message;

        }
        public DataTable GetQuarterReports(int QuarterID, string voterName, string SectorName, string BrokerName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetQuarterWiseBrokerReport", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@QuarterID", QuarterID);
                            cmd.Parameters.AddWithValue("@VoterName", voterName);
                            cmd.Parameters.AddWithValue("@SectorName", SectorName);
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 500;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetBrokerAdjustedReports(int QuarterID, string BrokerName, string DepartmentName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetQuarterWiseBrokerAdjustedScore", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@QuarterID", QuarterID);
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            cmd.Parameters.AddWithValue("@DepartmentName", DepartmentName);

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 500;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetDepartmentName()
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetDepartment", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {

                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataTable GetAdjustedWeightReports(int QuarterID, string voterName, string SectorName, string BrokerName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetDepartmentWiseAdjustedWeight", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@QuarterID", QuarterID);
                            cmd.Parameters.AddWithValue("@VoterName", voterName);
                            cmd.Parameters.AddWithValue("@SectorName", SectorName);
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public int GetMaxLimit(string BrokerName)
        {
            int MaxLimit = 0;
            DataSet ds = new DataSet();
            conn = new SqlConnection(strconn);
            conn.Open();
            try
            {
                cmd = new SqlCommand("BGS_GetBrokerMaxLimit", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(BrokerName))
                {
                    cmd.Parameters.Add("@BrokerName", SqlDbType.VarChar, 150).Value = BrokerName;
                    MaxLimit = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    conn.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return MaxLimit;

        }
        public DataSet GetBrokerWiseRanKCurrentQuarter(int Quarter, string BrokerName)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetBrokerReportByCurrentQuarter", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@QuarterID", Quarter);
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            cmd.CommandTimeout = 120;
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public DataSet GetBrokerWiseRanKLastQuarter(int Quarter, string BrokerName)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetBrokerReportByLastQuarter", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@QuarterID", Quarter);
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 120;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex2)
            {
                throw;
            }
        }
        public DataTable GetVoterByCurrentQuarter(string Empid)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetVoterByCurrentQuarter", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@EmpID", Empid);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetBrokerNameID(string BrokerName)
        {
            int BrokerNameID = 0;
            DataSet ds = new DataSet();
            conn = new SqlConnection(strconn);
            conn.Open();
            try
            {
                cmd = new SqlCommand("BGS_GetBokerNameID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(BrokerName))
                {
                    cmd.Parameters.Add("@BrokerName", SqlDbType.VarChar, 150).Value = BrokerName;
                    BrokerNameID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    conn.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BrokerNameID;

        }

        public int GetReasonNameID(string ReasonName)
        {
            int ReasonNameID = 0;
            DataSet ds = new DataSet();
            conn = new SqlConnection(strconn);
            conn.Open();
            try
            {
                cmd = new SqlCommand("BGS_GetReasonNameID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(ReasonName))
                {
                    cmd.Parameters.Add("@ReasonName", SqlDbType.VarChar, 150).Value = ReasonName;
                    ReasonNameID = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    conn.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ReasonNameID;

        }
        public void Insertupdatevoter(DataTable dt1)
        {
            try
            {
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_InsertUpdateVoteMAster", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@tblVoteMaster", dt1);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable BindUserBySearch(string Search, string Type)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_BindUserMaster", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@UserName", Search);
                            cmd.Parameters.AddWithValue("@Type ", Type);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DataTable GetUSer(string Empid, string pwd)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetUser", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@EmpID", Empid);
                            cmd.Parameters.AddWithValue("@Password", pwd);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public DataSet GetPerBrokerRank(int QuarterID, int BrokerNaneID)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetPerBrokerRankStatus", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@BrokerNameID", BrokerNaneID);
                            cmd.Parameters.AddWithValue("@QuarterID ", QuarterID);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds;
            }
            catch (Exception EX)
            {
                throw;
            }

        }

        public DataTable BindAnalystName(string BrokerName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetBrokerAnalyst", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable BindReasons()
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetBrokerAnalyst", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            //cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable BindAllAnalystName(string BrokerName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetALLBrokerAnalyst", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable BindBrokerData()
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetRABrokerName", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable UploadBrokerAnalystDetails(DataTable dt, string BrokerName)
        {
            DataTable dt1 = new DataTable();
            try
            {
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_UploadBrokerAnalystDetails", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@tblBrokerDetails", dt);
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt1);
                            // cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
                throw ex;
            }
            return dt1;
        }
        public int IsUserValidForAccess(string EmpID, string PageName)
        {
            int isValid = 0;
            try
            {
                //String strQuery = String.Empty;         
                conn = new SqlConnection(strconn);
                conn.Open();
                SqlCommand cmd = null;
                cmd = new SqlCommand("IsPageValidForUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EmpID", SqlDbType.VarChar, 50).Value = EmpID;
                cmd.Parameters.Add("@PageName", SqlDbType.VarChar, 500).Value = PageName;
                isValid = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            finally
            {
                conn.Close();
            }
            return isValid;
        }
        public DataTable DownloadBroker(string BrokerName)
        {
            DataTable dt = new DataTable();
            try
            {
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GetBrokerUploadData", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BrokerName", BrokerName);
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            return dt;
        }
        public DataTable GenarateQuarterByStartEndDate(string StartDate, string EndDate)
        {
            try
            {
                DataSet ds = new DataSet();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_GenarateQuarterByStartEndDate", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@QuarterStartDate", StartDate); 
                            cmd.Parameters.AddWithValue("@QurterEndDate", EndDate); 
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            conn.Close();
                        }
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public DataTable CheckSectorName(string SectorName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (conn = new SqlConnection(strconn))
                {
                    using (SqlCommand cmd = new SqlCommand("BGS_CheckSectorName", conn))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@SectorName", SectorName);
                            cmd.CommandType = CommandType.StoredProcedure;
                            conn.Open();
                            sda.SelectCommand = cmd;
                            sda.Fill(dt);
                            conn.Close();
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void InsetUpdateSectorNameData(int SectorId, string SectorName, string CreatedBy, string UpdatedBy)
        {
            conn = new SqlConnection(strconn);
            try
            {
                conn.Open();
                cmd = new SqlCommand("BGS_InsertUpdateSectorName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SectorId", SqlDbType.Int, 100).Value = SectorId;
                cmd.Parameters.Add("@SectorName", SqlDbType.VarChar, 100).Value = SectorName;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 100).Value = CreatedBy;
                cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 100).Value = UpdatedBy;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);

            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteSectorNameData(int SectorId)
        {
            conn = new SqlConnection(strconn);
            try
            {
                conn.Open();
                cmd = new SqlCommand("BGS_DeleteSectorName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SectorId", SqlDbType.Int, 100).Value = SectorId;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);

            }
            finally
            {
                conn.Close();
            }
        }
        public void InsertupdateUserEmmailId(DataTable dt1)
        {
            try
            {
                using (conn = new SqlConnection(strconn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 500;
                    cmd.CommandText = "BGS_InsertUpdateUserMaster";
                    cmd.Parameters.AddWithValue("@tblUserMaster", dt1);
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

    }
}