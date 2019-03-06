using System;
using System.Collections.Generic;
using System.Text;

namespace mAuth.BusinessEntities
{
    public interface ImAuthUserBusiness
    {
        IValidateUserResut ValidateUser(string UserName, string Password);
        void validateUserData(string UserName, string Password);
    }


    public interface ImAuthUserDataAccess
    {
        bool ValidateUser(string UserName, string Password);
        List<IUserRoles> GetRolesForUser(string UserName);
    }

}
