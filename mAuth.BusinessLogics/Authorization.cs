 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using mAuth.BusinessEntities;
using mAuth.BusinessLogics;
using mAuth.Common;
using Newtonsoft.Json;

namespace mAuth.BusinessLogics
{
    public class Authorization : IMauthBussiness
    {
        private static IMauthData authRepo;
        public Authorization(IMauthData _authRepo)
        {
            authRepo = _authRepo;
        }
        public List<IAttraction> getAttractions(int posId)
        {
            return authRepo.getAttractions(posId);
        }

        public List<IAuthStation> getAuthStations()
        {
            return authRepo.getAuthStations();
        }

        public string Validate(string ticketORMemberID, string attractionCode, string updateID, int posID, DateTime createdDate)
        {
            return authRepo.Validate(ticketORMemberID, attractionCode, updateID, posID, createdDate);

        }

        public IMemberTypeResult GetMembershipList(string membershipCode, string includeImage, string attraction)
        {
            IMemberTypeResult MemberTypeResult = authRepo.GetMembershipList(membershipCode, includeImage, attraction);
            if(MemberTypeResult.childCount > 0)
            {
                IMemershipList childMember = new IMemershipList();
                childMember.name = "Child";
                childMember.quantity = MemberTypeResult.childCount;
                MemberTypeResult.membershipList.Add(childMember);
            }
            return MemberTypeResult;
        }

        public List<IMembershipAdditionalPrograms> GetMembershipAdditionalPrograms(string membershipCode, int POSId, string updateId)
        {

            return authRepo.GetMembershipAdditionalPrograms(membershipCode, POSId, updateId);
        }

        public List<ITransactionTypeResult> GetTicketList(string transactionOrTicketID, string attractionCode, int posID, bool isGroupTicket = false)
        {
            return authRepo.GetTicketList(transactionOrTicketID, attractionCode, posID, isGroupTicket);

        }
        public string Authorize(string ticketORMemberID, string attractionCode, string updateID,
            string memberDataLineID, int posID, string salesTransactionId = "", int quantity = 0, string visitorType = null, string eventTime = null, string ticketCode = null)
        {
            return authRepo.Authorize(ticketORMemberID, attractionCode, updateID, memberDataLineID, posID, salesTransactionId, quantity, visitorType, eventTime, ticketCode);
        }


        public string UpdateMemberBenefits(SalesTransactionDetail salesTransactionDetail, int locationId, string customerCode)
        {
            return authRepo.UpdateMemberBenefits(salesTransactionDetail, locationId, customerCode);
        }

        public string GetTransactionId(int POSId, int LocationId, string UserId)
        {
            return authRepo.GetTransactionId(POSId, LocationId, UserId);
        }

        public void SaveSalesTransaction(List<IMembershipAdditionalPrograms> ticketList, int POSId, string UserId, string TransactionId, string customerCode, int LocationId)
        {
            authRepo.SaveSalesTransaction(ticketList, POSId, UserId, TransactionId, customerCode, LocationId);
        }

        private void ParseAuthResponse(string usageResponseStr)
        {

            TicketUsageCollectionResponse usageResponse = JsonConvert.DeserializeObject<TicketUsageCollectionResponse>(usageResponseStr, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });


        }


        public void InsertFailedOfflineData(IAuthorizeRequest authorizeRequest, string ticketResult)
        {
            authRepo.InsertFailedOfflineData(authorizeRequest,ticketResult);
        }

        public List<IMembershipAdditionalPrograms> BuildCityPassTicket(IAuthorizeRequest authorizeRequest, string cityPassTicketCode, string cityPassTicketDescription)
        {
            IMembershipAdditionalPrograms cityPassTicket = new IMembershipAdditionalPrograms();
            cityPassTicket.attractionCode = authorizeRequest.attraction;
            cityPassTicket.ticketCode = cityPassTicketCode;
            cityPassTicket.name = cityPassTicketDescription;
            cityPassTicket.quantity = 1;
            cityPassTicket.visitorTypeCode = authorizeRequest.visitorType;
            List<IMembershipAdditionalPrograms> cityPassTicketList = new List<IMembershipAdditionalPrograms>();
            cityPassTicketList.Add(cityPassTicket);
            return cityPassTicketList;
        }
        public void SaveCityPassTransaction(IAuthorizeRequest authorizeRequest,string cityPassTicketCode, string cityPassTicketDescription)
        {
            string TransactionId = authRepo.GetTransactionId(authorizeRequest.posId, authorizeRequest.locationId, authorizeRequest.updateID);
            List<IMembershipAdditionalPrograms> cityPassTicket = BuildCityPassTicket(authorizeRequest, cityPassTicketCode, cityPassTicketDescription);
            SalesTransaction salesTransaction = authRepo.BuildSalesTranasction(authorizeRequest.posId, authorizeRequest.updateID, TransactionId, null, authorizeRequest.locationId, authorizeRequest.code);
            salesTransaction.salesTransactionDetail = authRepo.BuildSalesTransactionDetail(cityPassTicket, authorizeRequest.posId, authorizeRequest.updateID, TransactionId, "General");
            authRepo.InsertSalesTransactionandDetail(salesTransaction);
        }

        public List<IMembershipSearchList> MembershipSearch(IMembershipSearchFilter filter)
        {
            return authRepo.MembershipSearch(filter);
        }
    }
}
