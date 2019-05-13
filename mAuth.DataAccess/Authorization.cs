using Dapper;
using mAuth.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace mAuth.DataAccess
{
    public class Authorization : IMauthData
    {
        private string connection;
        public Authorization(string _connection)
        {
            connection = _connection;
        }


        public List<IAttraction> getAttractions(int posId)
        {
            List<IAttraction> attractionList;

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@OrganizationUnitId", 1);
            parameter.Add("@PosId", posId);

            try
            {
                using (var con = new SqlConnection(connection))
                {

                    var result = con.QueryMultiple("usp_IOS_GetAttraction",
                         parameter,
                         commandType: CommandType.StoredProcedure);
                    attractionList = result.Read<IAttraction>().ToList();
                    var showTimes = result.Read<IAttractionShowTime>().ToList();
                    attractionList.ForEach(x =>
                    {
                        x.showTimeList = new List<IAttractionShowTime>();
                        x.showTimeList.AddRange(showTimes.FindAll(y => y.attractioncode.Trim() == x.attractioncode.Trim()));

                    });
                }

                return attractionList;
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public List<IAuthStation> getAuthStations()
        {
            List<IAuthStation> authStationList;

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@OrganizationUnitId", 1);
            parameter.Add("@StationType", "IOSAuth");

            try
            {
                using (var con = new SqlConnection(connection))
                {

                    authStationList = con.Query<IAuthStation>("usp_IOS_GetStationList", parameter, commandType: CommandType.StoredProcedure)
                        .ToList<IAuthStation>();

                }

                return authStationList;
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public string Validate(string ticketORMemberID, string attractionCode, string updateID, int posID,DateTime createdDate)
        {
            string authStatus = string.Empty;
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@OrganizationUnitId", 1);
            parameter.Add("@pOSId", posID);
            parameter.Add("@ticketId", ticketORMemberID);
            parameter.Add("@attractionCode", attractionCode);
            parameter.Add("@eventDate", createdDate != DateTime.MinValue ? createdDate :DateTime.Now);
            parameter.Add("@updateId", updateID);


            try
            {
                using (var con = new SqlConnection(connection))
                {

                    authStatus = con.Query<string>("usp_IOS_TicketAuditValidate", parameter, commandType: CommandType.StoredProcedure)
                      .SingleOrDefault<string>();
                }

                return authStatus;
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public List<IMembershipAdditionalPrograms> GetMembershipAdditionalPrograms(string membershipCode, int POSId, string updateId)
        {
            string membershipList = string.Empty;
            List<IMembershipAdditionalPrograms> memProgramList;
            string authStatus = string.Empty;
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@OrganizationUnitId", 1);
            parameter.Add("@POSId", POSId);
            parameter.Add("@membershipCode", membershipCode);
            parameter.Add("@UpdateId", updateId);
            parameter.Add("@AUTHORIZEDDATE", DateTime.Now);
            try
            {
                using (var con = new SqlConnection(connection))
                {

                    memProgramList = con.Query<IMembershipAdditionalPrograms>("usp_IOS_MemeberProgramsFind", parameter,
                        commandType: CommandType.StoredProcedure).ToList();

                }
                if (memProgramList.Count > 0)
                    memProgramList.ForEach(x => x.quantity = x.maxTicket);

                return memProgramList;
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public List<ITransactionTypeResult> GetTicketList(string transactionOrTicketID,string attractionCode, int posID,bool isGroupTicket=false)
        {
            string[] selectedAttractions = attractionCode.Split('|');
            char isGroupTicketType = isGroupTicket ? 'Y' : 'N';
            DynamicParameters parameter = new DynamicParameters();
            List<ITransactionTypeResult> TransactionList = new List<ITransactionTypeResult>();
            parameter.Add("@OrganizationUnitId", 1);
            parameter.Add("@POSId", posID);
            parameter.Add("@transactionId", transactionOrTicketID);
            parameter.Add("@isGroupTicket", isGroupTicketType);
            parameter.Add("@eventDate", DateTime.Now);
            try
            {
                using (var con = new SqlConnection(connection))
                {
                    foreach(string attraction in selectedAttractions)
                    {
                        parameter.Add("@attractionCode", attraction);
                        List<ITransactionTypeResult> result = con.Query<ITransactionTypeResult>("usp_IOS_TicketByAttraction_Find", parameter,
                        commandType: CommandType.StoredProcedure).ToList();
                        TransactionList = TransactionList.Concat(result).ToList();
                        // result= JsonConvert.SerializeObject(result1);
                    }
                }

                return TransactionList;
            }
            catch (Exception e)
            {

                throw;
            }


        }


        public string Authorize(string ticketORMemberID, string attractionCode, string updateID,
            string memberDataLineID, int posID, string salesTransactionId = "",int quantity = 0,string visitorType = null, string eventTime = null,string ticketCode = null)
        {
            DynamicParameters parameter = new DynamicParameters();
            string result = string.Empty;
            parameter.Add("@OrganizationUnitId", 1);
            parameter.Add("@ticketId", ticketORMemberID);
            parameter.Add("@authorizedSystemId", posID);
            parameter.Add("@membershipListLineId", memberDataLineID);
            parameter.Add("@attractionCode", attractionCode);
            parameter.Add("@utilizedDate", DateTime.Now);
            parameter.Add("@utilizedTime", DateTime.Now.ToString("HH:mm:ss"));
            parameter.Add("@updateId", updateID);
            parameter.Add("@salesTransactionId", salesTransactionId);
            parameter.Add("@quantity", quantity);
            parameter.Add("@visitorType", visitorType);
            parameter.Add("@time", eventTime);
            parameter.Add("@ticketCode", ticketCode);
            parameter.Add("@returnValue", result, DbType.String, direction: ParameterDirection.Output);


            try
            {
                using (var con = new SqlConnection(connection))
                {

                    con.Execute("usp_IOS_TicketAuthorization_Update", parameter,
                        commandType: CommandType.StoredProcedure);
                    result = parameter.Get<string>("@returnValue");

                }

                return result;
            }
            catch (Exception e)
            {

                throw;
            }
        }


        public IMemberTypeResult GetMembershipList(string membershipCode, string includeImage,string attraction)
        {

            DynamicParameters parameter = new DynamicParameters();
            IMemberTypeResult result = new IMemberTypeResult();
            parameter.Add("@OrganizationUnitId", 1);
            parameter.Add("@customerCode", membershipCode);
            parameter.Add("@includeImage", includeImage);
            parameter.Add("@authorizedDate", DateTime.Now);
            parameter.Add("@attractionCode", attraction);
            try
            {
                using (var con = new SqlConnection(connection))
                {

                    var temp = con.QueryMultiple("usp_IOS_GetMemberForAuthorizationDetails", parameter,
                         commandType: CommandType.StoredProcedure);

                    result.childCount = temp.ReadFirst<int>();
                    result.validAttractions = temp.Read<string>().ToList();
                     
                    if(!temp.IsConsumed)
                        result.membershipList = temp.Read<IMemershipList>().ToList();
                    else
                        result.membershipList = new List<IMemershipList>();
 
                }

                return result;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public void InsertFailedOfflineData(IAuthorizeRequest authorizeRequest, string ticketResult)
        {
            string result = string.Empty;
            DynamicParameters param = new DynamicParameters();
            param.Add("@code", authorizeRequest.code);
            param.Add("@failureReason", ticketResult);
            param.Add("@attraction", authorizeRequest.attraction);
            param.Add("@authorizedDate", authorizeRequest.createdDate);
            param.Add("@userId", authorizeRequest.updateID );
            param.Add("@thirdPartyType", authorizeRequest.thirdPartyType);
            param.Add("@thirdPartyVerificationStatus", authorizeRequest.thirdPartyVerificationStatus);
            param.Add("@posId", authorizeRequest.posId);

            try
            {

                using (var con = new SqlConnection(connection))
                {

                    result = con.Query<string>("usp_IOS_Add_FailedOfflineData", param, commandType: CommandType.StoredProcedure).SingleOrDefault<string>();

                }
            }
            catch (Exception e)
            {

                throw;
            }
        }


        #region Member Benefit Insertion

        public string GetTransactionId(int POSId, int LocationId, string UserId)
        {
            string SalesTransactionId = string.Empty;
            DynamicParameters parameter = new DynamicParameters();
            using (var con = new SqlConnection(connection))
            {
                parameter.Add("@organizationUnitId", 1);
                parameter.Add("@posId", POSId);
                parameter.Add("@locationId", LocationId);
                parameter.Add("@entityName", "sm_SalesTransaction");
                parameter.Add("@modeOfOperation", "");
                parameter.Add("@updateId", UserId);
                SalesTransactionId = con.Query<string>("usp_GetNewTransactionId", parameter, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }

            return SalesTransactionId;
        }

        public SalesTransaction BuildSalesTranasction(int POSId, string UserId, string TransactionId, string customerCode,int LocationId,string cityPassCode = null)
        {
            SalesTransaction salesTransaction = new SalesTransaction();
            salesTransaction.customerCode = customerCode;
            salesTransaction.customField1 = "";
            salesTransaction.customField2 = "";
            salesTransaction.customField3 = "";
            salesTransaction.locationId = (Int16)LocationId;
            salesTransaction.moduleCode = "Admission";
            salesTransaction.netSalesAmount = 0;
            salesTransaction.notes = "";
            salesTransaction.organizationUnitId = 1;
            salesTransaction.pOSId = POSId;
            salesTransaction.salesThrough = "General";
            salesTransaction.salesTransactionDate = DateTime.Now.Date;
            salesTransaction.salesTransactionDuration = "00:30";
            salesTransaction.salesTransactionId = TransactionId;
            salesTransaction.salesTransactionStatus = "Final";
            salesTransaction.salesTransactionTime = DateTime.Now.TimeOfDay.ToString();
            salesTransaction.salesTransactionType = "New";
            //salesTransaction.TaxExemptSaleInd = true ;
            salesTransaction.totalDiscountAmount = 0;
            salesTransaction.totalFreightAmount = 0.0m;
            salesTransaction.totalSaleAmount = 0;
            salesTransaction.referenceNumber = "";
            // salesTransaction.salesTransactionDetail = BuildSalesTransactionDetail(membershipList, POSId, UserId, TransactionId);
            salesTransaction.totalSavingAmount = 0;
            salesTransaction.totalTaxCreditableAmount = 0;
            salesTransaction.updateId = UserId;
            salesTransaction.salesTransactionTax = new List<SalesTransactionTax>();
            salesTransaction.salesTransactionDiscount = new List<SalesTransactionDiscount>();
            salesTransaction.salesTransactionPayment = new List<SalesTransactionPayment>();
            salesTransaction.salesTransactionDonation = new List<SalesTransactionDonation>();

            if(!string.IsNullOrEmpty(cityPassCode))
                salesTransaction.customField3 = cityPassCode;

            return salesTransaction;
        }

        public List<SalesTransactionDetail> BuildSalesTransactionDetail(List<IMembershipAdditionalPrograms> ticketList, int POSId, string UserId, string TransactionId,string TransactionMode)
        {
            List<SalesTransactionDetail> salesTransactionDetailList = new List<SalesTransactionDetail>();
            int ix = 1;
            foreach (IMembershipAdditionalPrograms ticket in ticketList)
            {
                SalesTransactionDetail salesTransactionDetail = new SalesTransactionDetail();
                salesTransactionDetail.CustomField1 = "WINCEPOSADMIS";
                salesTransactionDetail.ItemNumber = ticket.ticketCode;
                salesTransactionDetail.TicketCode = ticket.ticketCode;
                salesTransactionDetail.ScannedNumber = "";
                salesTransactionDetail.ItemDescription = ticket.name;
                //salesTransactionDetail.Quantity = member.MaxTicket;
                salesTransactionDetail.TaxAmount = ticket.taxAmount;
                salesTransactionDetail.IsOverridePrice = "N";
                salesTransactionDetail.TaxGroupCode = null;
                salesTransactionDetail.ActualPrice = ticket.actualPrice;
                salesTransactionDetail.RetailPrice = ticket.retailPrice;
                salesTransactionDetail.LineItemTotal = ticket.lineItemTotal;
                salesTransactionDetail.RulesApplied = "N";
                salesTransactionDetail.RulesId = ticket.rulesId;
                salesTransactionDetail.VisitorType = ticket.visitorTypeCode; //to be changed to code
                salesTransactionDetail.LineTransactionDescription = string.Empty;
                salesTransactionDetail.ReferenceNumber = string.Empty; //to be changed to code
                salesTransactionDetail.Taxable = "N";
                salesTransactionDetail.SalesStatus = "Sale";
                salesTransactionDetail.PermutationShowTime = "";
                salesTransactionDetail.PermutationSeatRestrict = "";
                salesTransactionDetail.PermutationAttraction = "";
                salesTransactionDetail.UpdateId = UserId;
                salesTransactionDetail.UpdateDate = DateTime.Now.Date;
                salesTransactionDetail.SalesTransactionId = TransactionId;
                salesTransactionDetail.OrganizationUnitId = 1;
                salesTransactionDetail.TransactionMode = TransactionMode;
                salesTransactionDetail.TicketId = null;
                salesTransactionDetail.GiftCardSwipeReq = "N";
                salesTransactionDetail.CategoryCode = string.Empty;
                salesTransactionDetail.CustomField3 = DateTime.Now.ToString("yyyy-MM-dd");
                salesTransactionDetail.TaxIncludePrice = ticket.taxIncludePrice;
                salesTransactionDetail.ActualVisitingDate = DateTime.Now;
                salesTransactionDetail.CustomNotes = "";
                salesTransactionDetail.TaxType = string.Empty;
                salesTransactionDetail.TaxableAmount = ticket.taxableAmount;
                salesTransactionDetail.TaxRateValue = ticket.taxRateValue;
                salesTransactionDetail.POSId = POSId;
                salesTransactionDetail.VisitingDate = DateTime.Now;
                for (int jx = 1; jx <= ticket.quantity; jx++)
                {
                    SalesTransactionDetail detail = (SalesTransactionDetail)salesTransactionDetail.Clone();
                    detail.Quantity = 1;
                    detail.LineItemNumber = ix;
                    salesTransactionDetailList.Add(detail);
                    ix = ix + 1;
                }

            }

            return salesTransactionDetailList;
        }

        public void InsertSalesTransactionandDetail(SalesTransaction salesTransaction)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                InsertSalesTransaction(salesTransaction);

                foreach (SalesTransactionDetail detail in salesTransaction.salesTransactionDetail)
                {
                    InsertSalesTransactionDetail(detail);
                }
                scope.Complete();
            }
        }


        public void SaveSalesTransaction(List<IMembershipAdditionalPrograms> ticketList, int POSId, string UserId, string TransactionId,string customerCode,int LocationId)
        {

            SalesTransaction salesTransaction = BuildSalesTranasction(POSId, UserId, TransactionId, customerCode, LocationId);
            salesTransaction.salesTransactionDetail = BuildSalesTransactionDetail(ticketList, POSId, UserId, TransactionId,"Guest");

            InsertSalesTransactionandDetail(salesTransaction);

                //scope.Complete();
            //}
        }

        

        public void InsertSalesTransaction(SalesTransaction salesTransaction)
        {

            DynamicParameters parameter = new DynamicParameters();
            using (var con = new SqlConnection(connection))
            {

                parameter.Add("@customerCode", salesTransaction.customerCode);
                parameter.Add("@customField1", salesTransaction.customField1);
                parameter.Add("@customField2", salesTransaction.customField2);
                parameter.Add("@customField3", salesTransaction.customField3);
                parameter.Add("locationId", salesTransaction.locationId);
                parameter.Add("netSalesAmount", salesTransaction.netSalesAmount);
                parameter.Add("notes", salesTransaction.notes);
                parameter.Add("organizationUnitId", salesTransaction.organizationUnitId);
                parameter.Add("moduleCode", salesTransaction.moduleCode);
                parameter.Add("pOSId", salesTransaction.pOSId);
                parameter.Add("referenceNumber", salesTransaction.referenceNumber);
                parameter.Add("salesThrough", salesTransaction.salesThrough);
                parameter.Add("salesTransactionDate", salesTransaction.salesTransactionDate);
                parameter.Add("salesTransactionTime", salesTransaction.salesTransactionTime);
                parameter.Add("salesTransactionDuration", salesTransaction.salesTransactionDuration);
                parameter.Add("salesTransactionStatus", salesTransaction.salesTransactionStatus);
                parameter.Add("salesTransactionType", salesTransaction.salesTransactionType);
                parameter.Add("taxExemptSaleInd", salesTransaction.taxExemptSaleInd);
                parameter.Add("totalDiscountAmount", salesTransaction.totalDiscountAmount);
                parameter.Add("totalFreightAmount", salesTransaction.totalFreightAmount);
                parameter.Add("totalSaleAmount", salesTransaction.totalSaleAmount);
                parameter.Add("totalTaxAmount", salesTransaction.totalTaxAmount);
                parameter.Add("updateId", salesTransaction.updateId);
                parameter.Add("holdSalesTransactionId", salesTransaction.holdTransactionId);
                parameter.Add("overridePriceUser", salesTransaction.overridePriceUser);
                parameter.Add("status", salesTransaction.status);
                parameter.Add("salesTransactionId", salesTransaction.salesTransactionId);
                parameter.Add("groupName", salesTransaction.groupName);
                parameter.Add("organizationName", salesTransaction.organizationName);
                parameter.Add("customerOrganizationCode", salesTransaction.customerOrganizationCode);
                parameter.Add("mailingAddressLineId", salesTransaction.mailingAddressLineId);
                parameter.Add("billingAddressLineId", salesTransaction.billingAddressLineId);
                parameter.Add("transactionStatus", salesTransaction.transactionStatus);
                parameter.Add("@depositeAmount", 0);
                parameter.Add("@returnValue", 50, direction: ParameterDirection.Output);


             var result=   con.Execute("usp_SalesTransaction_Insert", parameter,
                       commandType: CommandType.StoredProcedure);
                // result= JsonConvert.SerializeObject(result1);
            }
        }

        public void InsertSalesTransactionDetail(SalesTransactionDetail salesTransactionDetail)
        {
            DynamicParameters parameter = new DynamicParameters();
            using (var con = new SqlConnection(connection))
            {
                parameter.Add("itemDescription", salesTransactionDetail.ItemDescription);
                parameter.Add("scannedNumber", salesTransactionDetail.ScannedNumber);
                parameter.Add("itemNumber", salesTransactionDetail.ItemNumber);
                parameter.Add("lineItemNumber", salesTransactionDetail.LineItemNumber);
                parameter.Add("lineItemTotal", salesTransactionDetail.LineItemTotal);
                parameter.Add("lineTransactionDescription", salesTransactionDetail.LineTransactionDescription);
                parameter.Add("organizationUnitId", salesTransactionDetail.OrganizationUnitId);
                parameter.Add("quantity", salesTransactionDetail.Quantity);
                parameter.Add("referenceNumber", salesTransactionDetail.ReferenceNumber);
                parameter.Add("retailPrice", salesTransactionDetail.RetailPrice);
                parameter.Add("adjustedPrice", salesTransactionDetail.ActualPrice);
                parameter.Add("salesTransactionId", salesTransactionDetail.SalesTransactionId);
                parameter.Add("taxable", salesTransactionDetail.Taxable);
                parameter.Add("taxExemptSaleInd", salesTransactionDetail.taxExemptSaleInd);
                parameter.Add("taxGroupCode", salesTransactionDetail.TaxGroupCode);
                parameter.Add("visitorType", salesTransactionDetail.VisitorType);
                parameter.Add("customNotes", salesTransactionDetail.CustomNotes);
                parameter.Add("updateId", salesTransactionDetail.UpdateId);
                parameter.Add("upgradeFrom", salesTransactionDetail.UpgradeFrom);
                parameter.Add("ticketId", salesTransactionDetail.TicketId);
                parameter.Add("transactionMode ", salesTransactionDetail.TransactionMode);
                parameter.Add("foodInventoryStatus", "");
                parameter.Add("ruleId", salesTransactionDetail.RulesId);
                parameter.Add("ruleApplied", salesTransactionDetail.RulesApplied);
                parameter.Add("taxAmount", salesTransactionDetail.TaxAmount);
                parameter.Add("isOverridePrice", salesTransactionDetail.IsOverridePrice);
                parameter.Add("customField1", salesTransactionDetail.CustomField1);
                parameter.Add("customField2", salesTransactionDetail.CustomField2);
                parameter.Add("customField3", salesTransactionDetail.CustomField3);
                parameter.Add("returnLineNo", salesTransactionDetail.ReturnLineNo);
                parameter.Add("registrationType", salesTransactionDetail.RegistrationType);
                parameter.Add("valuePricing", salesTransactionDetail.ValuePricing);
                parameter.Add("visitingTime", salesTransactionDetail.VisitingTime);
                parameter.Add("visitingDate", salesTransactionDetail.VisitingDate);
                parameter.Add("instructions", salesTransactionDetail.Instructions);
                parameter.Add("contactName", salesTransactionDetail.ContactName);
                parameter.Add("transactionStatus", salesTransactionDetail.TransactionStatus);
                parameter.Add("isTaxIncludePrice", salesTransactionDetail.TaxIncludeInPrice);
                parameter.Add("taxIncludePrice", salesTransactionDetail.TaxIncludePrice);
                parameter.Add("basePrice", salesTransactionDetail.Baseprice);
                parameter.Add("earlyDiscountCode", salesTransactionDetail.EarlyDiscountCode);
                parameter.Add("earlyDiscountAmount", salesTransactionDetail.EarlyDiscountAmount);
                parameter.Add("earlyDiscountableAmount", salesTransactionDetail.EarlyDiscountableAmount);
                parameter.Add("overrideBasePrice", salesTransactionDetail.OverrideBaseprice);
                parameter.Add("salesStatus", salesTransactionDetail.SalesStatus);
                parameter.Add("actualVisitingDate", salesTransactionDetail.ActualVisitingDate);
                parameter.Add("actualTaxCode", salesTransactionDetail.ActualTaxCode);
                parameter.Add("actualTaxValue", salesTransactionDetail.ActualTaxValue);
                parameter.Add("externalStateFlag", salesTransactionDetail.ExternalStateTaxFlag);
                parameter.Add("taxRateValue", salesTransactionDetail.TaxRateValue);
                parameter.Add("insertType", salesTransactionDetail.InsertType);
                parameter.Add("totalCount", salesTransactionDetail.TotalCount);
                parameter.Add("taxableAmount", salesTransactionDetail.TaxableAmount);
                parameter.Add("taxType", salesTransactionDetail.TaxType);
                parameter.Add("pOSId", salesTransactionDetail.POSId);
                parameter.Add("discountCode", salesTransactionDetail.DiscountCode);
                parameter.Add("discountType", salesTransactionDetail.DiscountType);
                parameter.Add("discountableAmount", salesTransactionDetail.DiscountableAmount);
                parameter.Add("discountAmount", salesTransactionDetail.DiscountAmount);
                parameter.Add("reciprocalDiscount", salesTransactionDetail.ReciprocalDiscount);
                parameter.Add("@returnValue", ParameterDirection.Output);

                

                var result = con.Execute("usp_SalesTransactionDetail_Insert", parameter, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion

        public string UpdateMemberBenefits(SalesTransactionDetail salesTransactionDetail, int locationId, string customerCode)
        {


            DynamicParameters parameter = new DynamicParameters();
            string result = string.Empty;
            parameter.Add("@organizationUnitId", 1);
            parameter.Add("@salesTransactionId", salesTransactionDetail.SalesTransactionId);
            parameter.Add("@lineItemNumber", 1);
            parameter.Add("@itemNumber", salesTransactionDetail.ItemNumber);
            parameter.Add("@visitorType", salesTransactionDetail.VisitorType);
            parameter.Add("@updateId", salesTransactionDetail.UpdateId);
            parameter.Add("@pOSId", salesTransactionDetail.POSId);
            parameter.Add("@locationId", locationId);
            parameter.Add("@customerCode", customerCode);
            parameter.Add("@transactionMode", "Guest");
            parameter.Add("@customField3", DateTime.Now.Date);
            parameter.Add("@visitingDate", DateTime.Now);
            parameter.Add("@registrationType", "Admission");
            parameter.Add("@transactionStatus", "Sale");
            parameter.Add("@returnValue", result, DbType.String, direction: ParameterDirection.ReturnValue);


            try
            {
                using (var con = new SqlConnection(connection))
                {

                    con.Execute("usp_SalesTransactionDetailGRPTicketId_Insert", parameter,
                        commandType: CommandType.StoredProcedure);
                    result = parameter.Get<string>("@returnValue");

                }

                return result;
            }
            catch (Exception e)
            {

                throw;
            }


        }

        public List<IMembershipSearchList> MembershipSearch(IMembershipSearchFilter Filter)
        {
            DynamicParameters parameter = new DynamicParameters();
            string result = string.Empty;
            parameter.Add("@OrganizationUnit", 1);
            parameter.Add("@Name", Filter.name);
            parameter.Add("@ContactNumber", Filter.phoneNumber);
            parameter.Add("@EmailAddress", Filter.email);


            try
            {
                using (var con = new SqlConnection(connection))
                {
                    List<IMembershipSearchList> membershipList = con.Query<IMembershipSearchList>("IOS_Usp_CustomerMembershipFindAll", parameter,
                        commandType: CommandType.StoredProcedure).ToList();
                    return membershipList;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }


    }
}
