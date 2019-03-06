using System;
 
using mAuth.BusinessEntities;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

using System.Collections.Generic;
using mAuth.Common;

namespace mAuth.DataAccess
{
    
    public class mAuthUser : ImAuthUserDataAccess
    {
        private   readonly string connection;
 
        public mAuthUser(string _connection)
        {
            connection = _connection;          
        }
        public bool ValidateUser(string UserName, string Password)
        {
            IPasswordWithFormat passwordwithFormat;
            CommonLogics co = new CommonLogics();

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@ApplicationName","KMware");
            parameter.Add("@OrganizationUnitId",1);
            parameter.Add("@UserName",UserName);
            parameter.Add("@UpdateLastLoginActivityDate",1);
           
            try
            {
                using (var con = new SqlConnection(connection))
                {

                    passwordwithFormat = con.Query<IPasswordWithFormat>("usp_IOS_GetPasswordWithFormat", parameter, commandType: CommandType.StoredProcedure)
                        .SingleOrDefault();
                }

                if (passwordwithFormat != null && !string.IsNullOrEmpty(passwordwithFormat.PasswordFormat))
                {
                    if (Password.Trim().Equals(co.EncodePassword(passwordwithFormat.Password, passwordwithFormat.PasswordSalt,
                          co.ConvertIntToMembershipPasswordFormat(Convert.ToInt32(passwordwithFormat.PasswordFormat))).Trim()))
                    {


                        if (Convert.ToInt16(passwordwithFormat.FailedPasswordAttemptCount.Trim()) > 0)
                        {
                            UnlockUser(UserName, 1);
                        }
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
               
            }
            catch (Exception e)
            {

                throw;
            }
            
        }
        public  bool UnlockUser(string userName, short organizationUnitId)
        {

            using (SqlConnection conn = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.aspnet_Membership_UnlockUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = "KMWare";
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 256).Value = userName;
                    cmd.Parameters.Add("@OrganizationUnitId", SqlDbType.SmallInt).Value = organizationUnitId;

                    conn.Open();
                    int recCount = cmd.ExecuteNonQuery();
                    if (recCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
        }
        public List<IUserRoles> GetRolesForUser(string UserName)
        {
            
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@ApplicationName", "KMware");
            parameter.Add("@OrganizationUnitId", 1);
            parameter.Add("@UserName", UserName); 

            try
            {
                using (var con = new SqlConnection(connection))
                {

                   return con.Query<IUserRoles>("Usp_IOS_UsersInRoles_GetRolesForUser", parameter, commandType: CommandType.StoredProcedure)
                        .ToList<IUserRoles>();
                } 
            }
            catch (Exception e)
            {

                throw;
            }

        }

    }
}
