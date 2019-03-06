using System;
using System.Collections.Generic;
using System.Text;

namespace mAuth.BusinessEntities
{
   public interface IMauthBussiness
    {

        List<IAttraction> getAttractions();
        List<IAuthStation> getAuthStations();
        string Validate(string ticketORMemberID, string attractionCode, string updateID, int posID);
        IMemberTypeResult GetMembershipList(string membershipCode, string includeImage);
        List<IMembershipAdditionalPrograms> GetMembershipAdditionalPrograms(string membershipCode, int POSId, string updateId);
        List<ITransactionTypeResult> GetTicketList(string transactionOrTicketID, string moduleCode, string attractionCode, int posID);
        string Authorize(string ticketORMemberID, string attractionCode, string updateID,
        string memberDataLineID, int posID, string salesTransactionId = "");
        string UpdateMemberBenefits(SalesTransactionDetail salesTransactionDetail, int locationId, string customerCode);
        void SaveSalesTransaction(List<IMembershipAdditionalPrograms> membershipList, int POSId, string UserId, string TransactionId,string customerCode);
        string GetTransactionId(int POSId, int LocationId, string UserId);
    }


   public interface IMauthData
    {

        List<IAttraction> getAttractions();
        List<IAuthStation> getAuthStations();
        string Validate(string ticketORMemberID, string attractionCode, string updateID, int posID);
        IMemberTypeResult GetMembershipList(string membershipCode, string includeImage);
        List<IMembershipAdditionalPrograms> GetMembershipAdditionalPrograms(string membershipCode, int POSId, string updateId);
        List<ITransactionTypeResult> GetTicketList(string transactionOrTicketID, string moduleCode, string attractionCode, int posID);
        string Authorize(string ticketORMemberID, string attractionCode, string updateID,
        string memberDataLineID, int posID, string salesTransactionId = "");
        string UpdateMemberBenefits(SalesTransactionDetail salesTransactionDetail, int locationId, string customerCode);
        string GetTransactionId(int POSId, int LocationId, string UserId);
        SalesTransaction BuildSalesTranasction(int POSId, string UserId, string TransactionId, string customerCode);
        List<SalesTransactionDetail> BuildSalesTransactionDetail(List<IMembershipAdditionalPrograms> membershipList, int POSId, string UserId, string TransactionId);
        void SaveSalesTransaction(List<IMembershipAdditionalPrograms> membershipList, int POSId, string UserId, string TransactionId, string customerCode);
        void InsertSalesTransaction(SalesTransaction salesTransaction);
        void InsertSalesTransactionDetail(SalesTransactionDetail salesTransactionDetail);
    }
}
