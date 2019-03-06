using System;
using System.Collections.Generic;
using System.Text;

namespace mAuth.BusinessEntities
{
    public class IPasswordWithFormat
    {
        public string Password { get; set; }
        public string PasswordFormat { get; set; }
        public string PasswordSalt { get; set; }
        public string FailedPasswordAttemptCount { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string ChangePasswordOnNextLogin { get; set; }
        public string IsLockedOut { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public string IsMercuryValidationReq { get; set; }

    }

    public class IUserRoles
    {
        public string roleName { get; set; }
        // public string roleId { get; set; }
    }
    public class IUser
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
    public class IValidateUserResut
    {
        public bool isValid { get; set; }
        public List<IUserRoles> userRoles { get; set; }
    }




 
     
}
