using NUnit.Framework;
using mAuth.BusinessEntities;
using mAuth.BusinessLogics;
using Moq;
using System.Collections.Generic;
using System;

namespace MAuthApiTest
{
    [TestFixture]
    public class Tests
    {
        private   Mock<ImAuthUserDataAccess> _MAuthUserData;
        private  Mock<IMauthData> _MAutData;
        [SetUp]
        public void Setup()
        {
            string fakeUser = "admin";
            string fakePassword = "admin";
            _MAuthUserData = new Mock<ImAuthUserDataAccess>();
            _MAuthUserData.Setup(x => x.ValidateUser(fakeUser, fakePassword)).Returns(true);


            //User Roles
            List<IUserRoles> roleList = new List<IUserRoles>();
            IUserRoles role = new IUserRoles(); role.roleName = "administrator";
            roleList.Add(role);
            _MAuthUserData.Setup(x => x.GetRolesForUser(fakeUser)).Returns(roleList);


        }

        [Test]
        public void ValidateUser_ShouldbeValidFor_CorectUser()
        {
            //Arrage
            string fakeUser = "admin";
            string fakePassword = "admin";
            ImAuthUserBusiness _mAuthBusiness = new mAuth.BusinessLogics.mAuthUser(_MAuthUserData.Object);

            //Act
            var validationresult = _mAuthBusiness.ValidateUser(fakeUser, fakePassword);

            //Assert
            Assert.AreEqual(true, validationresult.isValid);
            
        }


        [Test]
        public void ValidateUser_ShouldReturnsRoleList()
        {
            //Arrage
            string fakeUser = "admin";
            string fakePassword = "admin";
            ImAuthUserBusiness _mAuthBusiness = new mAuth.BusinessLogics.mAuthUser(_MAuthUserData.Object);

            //Act
            var validationresult = _mAuthBusiness.ValidateUser(fakeUser, fakePassword);

            //Assert
            Assert.AreEqual(1, validationresult.userRoles.Count);

        }

        [Test]  
        public void ShouldThrowExceptionForEmptyUserandEmptyPassword()
        {
            //Arrage
            string fakeUser = "";
            string fakePassword = "";
            ImAuthUserBusiness _mAuthBusiness = new mAuth.BusinessLogics.mAuthUser(_MAuthUserData.Object);
            //Act
            
            //Assert
            var ex = Assert.Throws<ArgumentNullException>(()=>_mAuthBusiness.validateUserData(fakeUser,fakePassword));

        }


        [Test]
        public void ShouldThroeError_Null_User ()
        {
            //Arrage
            string fakeUser =null;
            string fakePassword = null;
            ImAuthUserBusiness _mAuthBusiness = new mAuth.BusinessLogics.mAuthUser(_MAuthUserData.Object);
            //Act

            //Assert
            var ex = Assert.Throws<ArgumentNullException>(() => _mAuthBusiness.validateUserData(fakeUser, fakePassword));

        }
    }
}