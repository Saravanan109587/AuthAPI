 
using System;
using System.Collections.Generic;
using System.Text;
using mAuth.BusinessEntities;
using mAuth.BusinessLogics;
 
namespace mAuth.BusinessLogics
{
  public  class Authorization: IMauthBussiness
    {
        private static IMauthData authRepo;
        public Authorization(IMauthData _authRepo)
        {
            authRepo = _authRepo;
        }
        public List<IAttraction> getAttractions()
        {
            return authRepo.getAttractions();
        }

        public List<IAuthStation> getAuthStations()
        {
            return authRepo.getAuthStations();
        }

        public string Validate(string ticketORMemberID, string attractionCode, string updateID, int posID)
        {
            return authRepo.Validate(ticketORMemberID, attractionCode, updateID, posID);

        }

        public IMemberTypeResult GetMembershipList(string membershipCode, string includeImage)
        {
            return authRepo.GetMembershipList(membershipCode, includeImage);
        }

        public List<IMembershipAdditionalPrograms> GetMembershipAdditionalPrograms(string membershipCode, int POSId, string updateId)
        {

            return authRepo.GetMembershipAdditionalPrograms(membershipCode, POSId, updateId);
        }

        public List<ITransactionTypeResult> GetTicketList(string transactionOrTicketID, string moduleCode, string attractionCode, int posID)
        {
            return authRepo.GetTicketList(transactionOrTicketID, moduleCode, attractionCode, posID);

        }
        public string Authorize(string ticketORMemberID, string attractionCode, string updateID,
            string memberDataLineID, int posID, string salesTransactionId = "")
        {
            return authRepo.Authorize(ticketORMemberID, attractionCode, updateID, memberDataLineID, posID, salesTransactionId);
        }


        public string UpdateMemberBenefits(SalesTransactionDetail salesTransactionDetail, int locationId, string customerCode)
        {
            return authRepo.UpdateMemberBenefits(salesTransactionDetail, locationId, customerCode);
        }

        public string GetTransactionId(int POSId, int LocationId, string UserId)
        {
            return authRepo.GetTransactionId(POSId, LocationId, UserId);
        }

        public void SaveSalesTransaction(List<IMembershipAdditionalPrograms> ticketList, int POSId, string UserId, string TransactionId, string customerCode)
        {
             authRepo.SaveSalesTransaction(ticketList, POSId, UserId, TransactionId, customerCode);
        }

        }
    }
