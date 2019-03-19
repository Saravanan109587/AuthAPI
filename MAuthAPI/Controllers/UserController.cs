using mAuth.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAuthAPI.Controllers
{
    public class UserController : ApiController
    {

        private readonly ImAuthUserBusiness validateBl;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// /
        /// </summary>
        /// <param name="_validateBl"></param>
        public UserController(ImAuthUserBusiness _validateBl)
        {
            log.Debug("Entered into User Controller");
            validateBl = _validateBl;
        }


        /// <summary>
        /// This function will validate the user and if valid then return the roles
        ///  
        /// </summary>
        /// <param name="user"> {username:'admin',password:'admin'}</param>
        /// <returns>{isValid:true,userRoles:[]}</returns>
        [HttpPost]
        public IValidateUserResut  ValidateUser(IUser user)
        {
            log.Debug("Entered into Validate User controller");           
            return validateBl.ValidateUser(user.userName, user.password);
        }


    }
}
