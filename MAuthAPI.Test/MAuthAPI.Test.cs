//using System;
//using System.Collections.Generic;
//using System.Text;
//using NUnit.Framework;
//using mAuth.BusinessEntities;
//using mAuth.BusinessLogics;
//using Moq;
//using MAuthAPI.Controllers;

//namespace MAuthAPI.Test
//{
//    [TestFixture]
//    public class MAuthAPI
//    {
//        private Mock<ImAuthUserBusiness> _userBusiness;
//        private Mock<ImAuthUserDataAccess> _userData;
//        private string fakeUser = "fakeUser";
//        private string fakePassword = "fakePassword";

//        [SetUp]
//        public void setUp()
//        {
//            _userBusiness = new Mock<ImAuthUserBusiness>();
//            _userData = new Mock<ImAuthUserDataAccess>();
//            IValidateUserResut fakeValidationResult = new IValidateUserResut();
//            fakeValidationResult.isValid = true;
//            fakeValidationResult.userRoles = new List<IUserRoles>();
//            fakeValidationResult.userRoles.Add(new IUserRoles
//            {
//                roleName = "fakeRole"
//            });
//            _userBusiness.Setup(x => x.ValidateUser(fakeUser, fakePassword)).Returns(fakeValidationResult);

//        }

//        [Test]
//        public void shouldReturnTrueForValidUser()
//        {
//            UserController usercontrol = new UserController(_userBusiness.Object);
//            var result = usercontrol.ValidateUser(new IUser
//            {
//                userName = fakeUser,
//                password = fakePassword
//            }
//            );

//            Assert.AreEqual(true, result.isValid);
//        }
//    }
//}
