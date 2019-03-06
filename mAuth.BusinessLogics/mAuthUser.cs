using System;
using System.Collections.Generic;
using mAuth.BusinessEntities;
  

namespace mAuth.BusinessLogics
{
    public class mAuthUser : ImAuthUserBusiness
    {
        private readonly ImAuthUserDataAccess dataAccess;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public mAuthUser(ImAuthUserDataAccess _dataAccess)
        {
            dataAccess = _dataAccess;
        }

        public IValidateUserResut ValidateUser(string UserName, string Password)
        {
            validateUserData(UserName, Password);

            if (dataAccess.ValidateUser(UserName, Password))
            {
                List<IUserRoles> userRoleList = dataAccess.GetRolesForUser(UserName);
                return new IValidateUserResut { isValid = true, userRoles = userRoleList };
            }
            else
                return new IValidateUserResut { isValid = false, userRoles = null };


            
        }

        public void validateUserData(string UserName, string Password)
        {
            if (string.IsNullOrEmpty(UserName))
                throw new ArgumentNullException("User name is Required", UserName);
            if(string.IsNullOrEmpty(Password))
                throw new ArgumentNullException("Password is Required", Password);
        }
    }
}
