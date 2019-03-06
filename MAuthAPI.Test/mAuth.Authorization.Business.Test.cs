using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using mAuth.BusinessEntities;
using mAuth.BusinessLogics;
namespace MAuthAPI.Test
{
    [TestFixture]
    public class mAuthAuthorizationTest
    {
        private Mock<IMauthData> _AuthorizationData;
        [SetUp]
        public void setup()
        {
            _AuthorizationData = new Mock<IMauthData>();

            //Attractions
            List<IAttraction> fakeAttrList = new List<IAttraction>();
            List<IAttractionShowTime> fakeTShowTimeList = new List<IAttractionShowTime>();
            fakeTShowTimeList.Add(new IAttractionShowTime
            {
                attractioncode = "fakeAttraction",
                timeOfShow = "12.00"
            });

            fakeAttrList.Add(new IAttraction
            {
                attractioncode = "fakeAttraction",
                description = "fake Attraction",
                showTimeList = fakeTShowTimeList
            });

            _AuthorizationData.Setup(x => x.getAttractions()).Returns(fakeAttrList);


            //Posstation
            List<IAuthStation> fakestationList = new List<IAuthStation>();
            fakestationList.Add(new IAuthStation
            {
                posId = "10",
                posName = "fake Station"
            });
            _AuthorizationData.Setup(x => x.getAuthStations()).Returns(fakestationList);

            //Validate code result
            string fakeId = "12345678";
            string fakeAttraction = "fake Attractions";
            string fakeUpdateId = "fake Updater";
            string fakeIncludeMembershipImage = "N";
            int fakePosId = 100;
            string fakeModulecode = "fake Module";
            _AuthorizationData.Setup(x => x.Validate(fakeId, fakeAttraction, fakeUpdateId, fakePosId)).Returns("Valid|0|0|TRANSACTIONWISE|0|N|0");


            //Membershi List
            IMemberTypeResult fakemembershipList = new IMemberTypeResult();
            fakemembershipList.childCount = 0;
            fakemembershipList.membershipList = new List<IMemershipList>();
            fakemembershipList.membershipList.Add(new IMemershipList
            {
                name = "fake Member"
            });
            _AuthorizationData.Setup(x => x.GetMembershipList(fakeId, fakeIncludeMembershipImage)).Returns(fakemembershipList);

            //Membership additional Programs
            List<IMembershipAdditionalPrograms> fakememberProgramList = new List<IMembershipAdditionalPrograms>();
            fakememberProgramList.Add(new IMembershipAdditionalPrograms
            {
                attractionCode = "fake Attraction",
                price = 10.00m
            });
            _AuthorizationData.Setup(x => x.GetMembershipAdditionalPrograms(fakeId, fakePosId, fakeUpdateId)).Returns(fakememberProgramList);

            //Ticket List
            List<ITransactionTypeResult> fakeTicketList = new List<ITransactionTypeResult>();
            fakeTicketList.Add(new ITransactionTypeResult
            {
                attractionCode = "fake Attraction",
                ticketCode = "fake Ticket"
            });
            _AuthorizationData.Setup(x => x.GetTicketList(fakeId, fakeModulecode, fakeAttraction, fakePosId)).Returns(fakeTicketList);

            //Authorize
            string faleMembeershipLineId = "0";
            _AuthorizationData.Setup(x => x.Authorize(fakeId, fakeAttraction, fakeUpdateId, faleMembeershipLineId, fakePosId, "")).Returns("success");

        }

        [Test]
        public void ShouldgetAllAttractions()
        {
            IMauthBussiness mAuthBusinesss = new mAuth.BusinessLogics.Authorization(_AuthorizationData.Object);


            var result = mAuthBusinesss.getAttractions();
            Assert.AreEqual(1, result.Count);

        }

        [Test]
        public void ShouldgetAllStations()
        {
            IMauthBussiness mAuthBusinesss = new mAuth.BusinessLogics.Authorization(_AuthorizationData.Object);


            var result = mAuthBusinesss.getAuthStations();
            Assert.AreEqual(1, result.Count);

        }



        [Test]
        public void ShouldReturnValidForValidTransactionAndAtraction()
        {
            IMauthBussiness mAuthBusinesss = new mAuth.BusinessLogics.Authorization(_AuthorizationData.Object);


            string fakeId = "12345678";
            string fakeAttraction = "fake Attractions";
            string fakeUpdateId = "fake Updater";
            int fakePosId = 100;
            var result = mAuthBusinesss.Validate(fakeId, fakeAttraction, fakeUpdateId, fakePosId);
            Assert.AreEqual("Valid", result.Split('|')[0]);

        }

        //[Ignore("Summa")]
        [Test]
        public void ShouldReturnTicketList()
        {
            IMauthBussiness mAuthBusinesss = new mAuth.BusinessLogics.Authorization(_AuthorizationData.Object);


            string fakeId = "12345678";
            string fakeAttraction = "fake Attractions";
            string fakeUpdateId = "fake Updater";
            int fakePosId = 100;
            string fakeModulecode = "fake Module";
            var result = mAuthBusinesss.GetTicketList(fakeId, fakeModulecode, fakeAttraction, fakePosId);
            Assert.AreEqual(1, result.Count);

        }


        [Test]
        public void ShouldReturnMembershipList()
        {
            IMauthBussiness mAuthBusinesss = new mAuth.BusinessLogics.Authorization(_AuthorizationData.Object);
            string fakeId = "12345678";
            int fakePosId = 100;
            string fakeIncludeMembershipImage = "N";
            var result = mAuthBusinesss.GetMembershipList(fakeId, fakeIncludeMembershipImage);
            Assert.AreEqual(1, result.membershipList.Count);

        }

        [Test]
        public void ShouldReturnMembershipProgramList()
        {
            IMauthBussiness mAuthBusinesss = new mAuth.BusinessLogics.Authorization(_AuthorizationData.Object);
            string fakeId = "12345678";
            int fakePosId = 100;
            string fakeUpdateId = "fake Updater";
            var result = mAuthBusinesss.GetMembershipAdditionalPrograms(fakeId, fakePosId, fakeUpdateId);
            Assert.AreEqual(1, result.Count);

        }


        [Test]
        public void ShouldReturnSuccessForAuthorize()
        {
            IMauthBussiness mAuthBusinesss = new mAuth.BusinessLogics.Authorization(_AuthorizationData.Object);
            string fakeId = "12345678";
            string fakeAttraction = "fake Attractions";
            int fakePosId = 100;
            string fakeUpdateId = "fake Updater";
            string faleMembeershipLineId = "0";
            var result = mAuthBusinesss.Authorize(fakeId, fakeAttraction, fakeUpdateId, faleMembeershipLineId, fakePosId, "");
            Assert.AreEqual("success", result);

        }
    }
}
