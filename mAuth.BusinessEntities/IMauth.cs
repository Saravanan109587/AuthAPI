using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mAuth.BusinessEntities
{
   public interface IMauthBussiness
    {

        List<IAttraction> getAttractions(int posId);
        List<IAuthStation> getAuthStations();
        string Validate(string ticketORMemberID, string attractionCode, string updateID, int posID, DateTime createdDate);
        IMemberTypeResult GetMembershipList(string membershipCode, string includeImage,string attraction);
        List<IMembershipAdditionalPrograms> GetMembershipAdditionalPrograms(string membershipCode, int POSId, string updateId);
        List<ITransactionTypeResult> GetTicketList(string transactionOrTicketID, string attractionCode, int posID,bool isGroupTicket);
        string Authorize(string ticketORMemberID, string attractionCode, string updateID,
             string memberDataLineID, int posID, string salesTransactionId="",int quantity=0,string visitorType="",string eventTime="",string ticketCode = "");
        string UpdateMemberBenefits(SalesTransactionDetail salesTransactionDetail, int locationId, string customerCode);
        void SaveSalesTransaction(List<IMembershipAdditionalPrograms> membershipList, int POSId, string UserId, string TransactionId,string customerCode,int LocationId);
        string GetTransactionId(int POSId, int LocationId, string UserId);
        void SaveCityPassTransaction(IAuthorizeRequest authorizeRequestData,string cityPassTicketCode, string cityPassTicketDescription);
        List<IMembershipSearchList> MembershipSearch(IMembershipSearchFilter memberFilter);
        void InsertFailedOfflineData(IAuthorizeRequest authorizeRequest, string ticketResult);
    }


   public interface IMauthData
    {

        List<IAttraction> getAttractions(int posId);
        List<IAuthStation> getAuthStations();
        string Validate(string ticketORMemberID, string attractionCode, string updateID, int posID, DateTime createdDate);
        IMemberTypeResult GetMembershipList(string membershipCode, string includeImage,string attraction);
        List<IMembershipAdditionalPrograms> GetMembershipAdditionalPrograms(string membershipCode, int POSId, string updateId);
        List<ITransactionTypeResult> GetTicketList(string transactionOrTicketID, string attractionCode, int posID,bool isGroupTicket);
        string Authorize(string ticketORMemberID, string attractionCode, string updateID,
        string memberDataLineID, int posID, string salesTransactionId, int quantity, string visitorType, string eventTime,string ticketCode);
        string UpdateMemberBenefits(SalesTransactionDetail salesTransactionDetail, int locationId, string customerCode);
        string GetTransactionId(int POSId, int LocationId, string UserId);
        SalesTransaction BuildSalesTranasction(int POSId, string UserId, string TransactionId, string customerCode,int LocationId,string cityPassCode);
        List<SalesTransactionDetail> BuildSalesTransactionDetail(List<IMembershipAdditionalPrograms> membershipList, int POSId, string UserId, string TransactionId,string TransactionMode);
        void SaveSalesTransaction(List<IMembershipAdditionalPrograms> membershipList, int POSId, string UserId, string TransactionId, string customerCode,int LocationId);
        void InsertSalesTransaction(SalesTransaction salesTransaction);
        void InsertSalesTransactionDetail(SalesTransactionDetail salesTransactionDetail);
        void InsertSalesTransactionandDetail(SalesTransaction salesTransaction);
        List<IMembershipSearchList> MembershipSearch(IMembershipSearchFilter membershipSearchFilter);
        void InsertFailedOfflineData(IAuthorizeRequest authorizeRequest, string ticketResult);
    }
}
