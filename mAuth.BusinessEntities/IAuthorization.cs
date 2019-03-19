using System;
using System.Collections.Generic;
using System.Text;

namespace mAuth.BusinessEntities
{
    public class IAttraction
    {
        public string attractioncode { get; set; }
        public string description { get; set; }
        public List<IAttractionShowTime> showTimeList { get; set; }
    }

    public class IAttractionShowTime
    {
        public string timeOfShow { get; set; }
        public string attractioncode { get; set; }
    }

    public class IAuthStation
    {
        public string posId { get; set; }
        public string posName { get; set; }
    }

    public class IAuthorizeRequest
    {
        public string code { get; set; }
        public string attraction { get; set; }
        public int posId { get; set; }
        public string updateID { get; set; }
        public string includeMemListImg { get; set; }
        public string selectedMemberLineID { get; set; }

    }
    public class ICodeValidationResult
    {
        public string type { get; set; }
        public List<ITransactionTypeResult> transactionResult { get; set; }
        public IMemberTypeResult membertypeResult { get; set; }
        public List<IMembershipAdditionalPrograms> memAdditionalPrograms { get; set; }
        public string ticketResult { get; set; }
    }
    public class MembershipList
    {
        public string membershipCode { get; set; }
        public string membershipListLineId { get; set; }
        public string name { get; set; }
        public byte[] image { get; set; }
        public string relationship { get; set; }
        public string visitorTypeDescription { get; set; }
        public int childCount { get; set; }
        public int authCount { get; set; }
        public short organizationUnitId { get; set; }
        public string ticketCode { get; set; }
        public decimal price { get; set; }
        public int paxTicket { get; set; }
        public string attractionCode { get; set; }
        public string isTaxable { get; set; }
        public string taxIncludedInPrice { get; set; }
        public string taxExempted { get; set; }
        public string taxGroupCode { get; set; }
    }
    public class TicketList
    {
        public int TicketCount { get; set; }
        public string TicketIdList { get; set; }

    }
    public class AuthorizeMemOrTransRequest
    {
        public string Attraction { get; set; }
        public int POSID { get; set; }
        public string UpdateID { get; set; }
        public List<MembershipList> MemList { get; set; }
        public List<ITransactionTypeResult> TransList { get; set; }
        public List<IMembershipAdditionalPrograms> BenefitList { get; set; }
    }
    public class IMembershipAdditionalPrograms
    {
        public int organizationUnitId { get; set; }
        public string ticketCode { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int authCount { get; set; }
        public string visitorTypeDescription { get; set; }
        public int maxTicket { get; set; }
        public string membershipCode { get; set; }
        public string attractionCode { get; set; }
        public string isTaxable { get; set; }
        public decimal taxIncludeInPrice { get; set; }
        public string taxExempted { get; set; }
        public string taxGroupCode { get; set; }
        public decimal taxAmount { get; set; }
        public decimal actualPrice { get; set; }
        public decimal retailPrice { get; set; }
        public decimal lineItemTotal { get; set; }
        public decimal taxIncludePrice { get; set; }
        public decimal taxableAmount { get; set; }
        public int taxRateValue { get; set; }
        public int rulesId { get; set; }
        public int quantity { get; set; }
    }
    public class SalesTransactionDetail
    {
        public string SalesTransactionId { get; set; }
        public string ItemNumber { get; set; }
        public string VisitorType { get; set; }
        public string UpdateId { get; set; }
        public int POSId { get; set; }
        public string CustomField1 { get; set; }
        public string TicketCode { get; set; }
        public string ScannedNumber { get; set; }
        public string ItemDescription { get; set; }
        public decimal TaxAmount { get; set; }
        public string IsOverridePrice { get; set; }
        public string TaxGroupCode { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal LineItemTotal { get; set; }
        public string RulesApplied { get; set; }
        public int RulesId { get; set; }
        public string LineTransactionDescription { get; set; }
        public string ReferenceNumber { get; set; }
        public string Taxable { get; set; }
        public string SalesStatus { get; set; }
        public string PermutationShowTime { get; set; }
        public string PermutationSeatRestrict { get; set; }
        public string PermutationAttraction { get; set; }
        public DateTime UpdateDate { get; set; }
        public int OrganizationUnitId { get; set; }
        public string TransactionMode { get; set; }
        public string TicketId { get; set; }
        public string GiftCardSwipeReq { get; set; }
        public string CategoryCode { get; set; }
        public string CustomField3 { get; set; }
        public decimal TaxIncludePrice { get; set; }
        public DateTime ActualVisitingDate { get; set; }
        public string CustomNotes { get; set; }
        public string TaxType { get; set; }
        public decimal TaxableAmount { get; set; }
        public int TaxRateValue { get; set; }
        public int Quantity { get; set; }
        public int LineItemNumber { get; set; }
        public string taxExemptSaleInd { get; set; }
        public string UpgradeFrom { get; set; }
        public string CustomField2 { get; set; }
        public int ReturnLineNo { get; set; }
        public string RegistrationType { get; set; }
        public decimal ValuePricing { get; set; }
        public string VisitingTime { get; set; }
        public string PrintFor { get; set; }
        public string TransactionStatus { get; set; }
        public int ValuePriceNo { get; set; }
        public string Instructions { get; set; }
        public decimal TaxIncludeDiscountAmt { get; set; }
        public string TicketMessage { get; set; }
        public int TicketLineId { get; set; }
        public decimal EarlyDiscountableAmount { get; set; }
        public decimal EarlyDiscountCode { get; set; }
        public decimal Baseprice { get; set; }
        public string PrintTicketBy { get; set; }
        public decimal OverrideBaseprice { get; set; }
        public int IsBuyOneFreeDis { get; set; }
        public int AssociatedLineNumber { get; set; }
        public string IsAlldiscount { get; set; }
        public string SeatingDescription { get; set; }
        public string LastName { get; set; }
        public int TotalCount { get; set; }
        public string InsertType { get; set; }
        public DateTime VisitingDate { get; set; }
        public string ContactName { get; set; }
        public string TaxIncludeInPrice { get; set; }
        public decimal EarlyDiscountAmount { get; set; }
        public string ActualTaxCode { get; set; }
        public string ActualTaxValue { get; set; }
        public string ExternalStateTaxFlag { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountableAmount { get; set; }
        public decimal ReciprocalDiscount { get; set; }
    

        public object Clone()
        {
            return this.MemberwiseClone();
        }



    }
    public class ITransactionTypeResult
    {
        public int organizationUnitId { get; set; }
        public string ticketId { get; set; }
        public string transactionId { get; set; }
        public string attractionCode { get; set; }
        public string visitorType { get; set; }
        public int ticketCount { get; set; }
        public string ticketStyleCode { get; set; }
        public int maxTicketCount { get; set; }
        public string authorizationRequired { get; set; }
        public string description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string ticketMessage { get; set; }
        public string eventTime { get; set; }
        public string ticketCode { get; set; }
        public string ticketPrintStatus { get; set; }
        public string attractionDescription { get; set; }
    }
    public class IMemberTypeResult
    {
        public int childCount { get; set; }
        public List<IMemershipList> membershipList { get; set; }

    }
    public class IMemershipList
    {
        public int organizationUnitId { get; set; }
        public string membershipcode { get; set; }
        public int membershipListLineId { get; set; }
        public string name { get; set; }
        public DateTime dob { get; set; }
        public byte[] image { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string relationShip { get; set; }
        public string visitorTypeCode { get; set; }
        public string customfield1 { get; set; }
        public string updateId { get; set; }
        public DateTime updateDate { get; set; }
        public DateTime expiryDate { get; set; }
        public string visitorTypeDescription { get; set; }
        public string status { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public decimal price { get; set; }
        public string title { get; set; }
        public int extraCardPrintCount { get; set; }
        public string email { get; set; }
        public int authCount { get; set; }

    }

    public class SalesTransaction
    {
        public short organizationUnitId { get; set; }

        public string moduleCode { get; set; }

        public string salesTransactionId { get; set; }

        public string orderId { get; set; }

        public int pOSId { get; set; }

        public short locationId { get; set; }

        public string salesTransactionType { get; set; }

        public DateTime salesTransactionDate { get; set; }

        public string salesTransactionTime { get; set; }

        public string salesTransactionDuration { get; set; }

        public bool taxExemptSaleInd { get; set; }

        public decimal totalTaxAmount { get; set; }

        public decimal totalSaleAmount { get; set; }
        public decimal totalFreightAmount { get; set; }

        public decimal totalDiscountAmount { get; set; }

        public decimal netSalesAmount { get; set; }

        public string salesTransactionStatus { get; set; }

        public string customerCode { get; set; }

        public string salesThrough { get; set; }

        public string referenceNumber { get; set; }

        public string notes { get; set; }

        public string customField1 { get; set; }

        public string customField2 { get; set; }

        public string customField3 { get; set; }

        public DateTime updateDate { get; set; }

        public string updateId { get; set; }

        public decimal totalSavingAmount { get; set; }

        public decimal totalTaxCreditableAmount { get; set; }
        

        public List<SalesTransactionDetail> salesTransactionDetail { get; set; }

        public List<SalesTransactionTax> salesTransactionTax { get; set; }

        public List<SalesTransactionDiscount> salesTransactionDiscount;

        public List<SalesTransactionPayment> salesTransactionPayment { get; set; }

        public List<SalesTransactionDonation> salesTransactionDonation { get; set; }

        public List<SalesTransactionTaxExempt> salesTransactionTaxExempt { get; set; }

        public List<SalesTransactionAdmissionTiming> salesTransactionAdmissionTiming { get; set; }

        public List<SalesTransactionAdmissionSeating> salesTransactionSeat { get; set; }

        public List<SalesTransactionCCDetail> salesTransactionCCDetail { get; set; }

        //public List<TransactionRequiredSeat> transactionRequiredSeat { get; set; }

        //public List<TransactionTimeAttraction> transactionTimeAttraction { get; set; }

        public decimal amountTended { get; set; }

        public decimal change { get; set; }

        public string transactionType { get; set; }

        public decimal donationAmount { get; set; }

        public string name { get; set; }
        public string ticketId { get; set; }
        public int lineItemNumber { get; set; }
        public decimal quantity { get; set; }
        public string visitorType { get; set; }
        public Nullable<System.DateTime> visitingDate { get; set; }
        public string itemDescrption { get; set; }
        public string registrationType { get; set; }
        public string ticketCode { get; set; }
        public string ticketStyleCode { get; set; }
        public string ticketMessage { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public int assoicatedLineItemNumber { get; set; }
        public string groupName { get; set; }
        public string visitingTime { get; set; }
        public string cashcardcustomerCode { get; set; }
        public string giftcashcardcustomerCode { get; set; }
        public string holdTransactionId { get; set; }
        public string overridePriceUser { get; set; }
        public string status { get; set; }
        public string organizationName { get; set; }
        public string customerOrganizationCode { get; set; }
        public int mailingAddressLineId { get; set; }
        public int billingAddressLineId { get; set; }
        public string giftcashcardtransactionStatuscustomerCode { get; set; }
        public string transactionStatus { get; set; }




    }

    public class SalesTransactionTax
    {
        public string salesTransactionId { get; set; }

        public short organizationUnitId { get; set; }

        public int lineItemNumber { get; set; }

        public string taxGroupCode { get; set; }

        public string tax { get; set; }

        public decimal taxableAmount { get; set; }

        public decimal taxAmount { get; set; }

        public string referenceNumber { get; set; }

        public string notes { get; set; }
        public System.DateTime updateDate { get; set; }
        public string updateId { get; set; }
    }
    public static class TrasactionTypes
    {
        public const string MEMBERWISE = "MEMBERTYPE";
        public const string TICKETWISE = "TICKETWISE";
        public const string TRANSACTIONWISE = "TRANSACTIONWISE";
    }
    public class SalesTransactionPayment
    {
        public string salesTransactionId { get; set; }

        public short organizationUnitId { get; set; }

        public string paymentTypeCode { get; set; }

        public decimal salesTransactionAmount { get; set; }

        public decimal amountTended { get; set; }

        public decimal change { get; set; }

        public string referenceNumber { get; set; }

        public string notes { get; set; }

        public System.DateTime updateDate { get; set; }

        public string updateId { get; set; }

        public string currencyCode { get; set; }

        public decimal amount { get; set; }

        public decimal conversionRate { get; set; }
    }
    public class SalesTransactionDonation
    {
        public string salesTransactionId { get; set; }

        public short organizationUnitId { get; set; }

        public int donateToOrganizationId { get; set; }

        public decimal amount { get; set; }

        public string customerCode { get; set; }

        public string referenceNumber { get; set; }

        public string roundedAmountYN { get; set; }

        public string customField1 { get; set; }

        public string customField2 { get; set; }

        public string customField3 { get; set; }

        public string notes { get; set; }

        public System.DateTime updateDate { get; set; }

        public string updateId { get; set; }

        public string displayName { get; set; }
    }
    public class SalesTransactionTaxExempt
    {
        public string salesTransactionId { get; set; }

        public short organizationUnitId { get; set; }

        public string taxExemptCode { get; set; }

        public decimal taxExemptAmount { get; set; }

        public string customerCode { get; set; }

        public string taxExemptDescription { get; set; }

        public string referenceNumber { get; set; }

        public string notes { get; set; }

        public System.DateTime updateDate { get; set; }

        public string updateId { get; set; }
    }
    public class SalesTransactionDiscount
    {
        public string salesTransactionId { get; set; }

        public int lineItemNumber { get; set; }

        public short organizationUnitId { get; set; }

        public string discountCode { get; set; }

        public string discountType { get; set; }

        public decimal discountableAmount { get; set; }

        public decimal discountAmount { get; set; }
        public string referenceNumber { get; set; }

        public string notes { get; set; }

        public System.DateTime updateDate { get; set; }

        public string updateId { get; set; }
    }
    public class SalesTransactionAdmissionTiming
    {
        public string SalesTransactionId { get; set; }
        public int OrganizationUnitId { get; set; }
        public string AttractionCode { get; set; }
        public int LineItemNumber { get; set; }
        public string TicketCode { get; set; }
        public string EventTime { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateId { get; set; }
        public string MemberSale { get; set; }
        public string PassTypeCode { get; set; }
        public string TicketDate { get; set; }

    }

    public class SalesTransactionAdmissionSeating
    {
        public string SalesTransactionId { get; set; }
        public int OrganizationUnitId { get; set; }

        public string AttractionCode { get; set; }
        public string ResourceCode { get; set; }
        public int LineItemNumber { get; set; }
        public string TicketCode { get; set; }
        public int SeatNumber { get; set; }
        public string EventTime { get; set; }
        public string Row { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateId { get; set; }
        public string MemberSale { get; set; }
        public string PassTypeCode { get; set; }
        public DateTime EventDate { get; set; }

    }

    public class SalesTransactionCCDetail
    {

        public string tranType { get; set; }
        public string refNo { get; set; }
        public string termianl { get; set; }
        public string operatorID { get; set; } //POSUser
        public int action { get; set; }
        public string command { get; set; }//Sale,VoidSale,Return,VoidReturn,Issue,VoidIssue,Sale,VoidSale,Reload,VoidReload,Return,VoidReturn,Balance
        public string amount { get; set; }
        public string billPay { get; set; } //0=Non Bill Pay, 1=Bill Payment Transaction
        public string card { get; set; }
        public string cardPresent { get; set; } //0=Card not presnent, 1=Card present
        public string custCode { get; set; }
        public string cvv { get; set; }
        public string expDate { get; set; } //mmyy
        public string mm { get; set; }
        public string yy { get; set; }
        public string gratuityAmount { get; set; }// For Restaurant Transcations Only
        public string manual { get; set; } //0=Manual Transaction, 1=Swiped Transaction
        public string trackData { get; set; } //The whole Credit Card Track
        public string track { get; set; }
        public string member { get; set; } //First Midle Last Name
        public string street { get; set; }
        public string taxAmt { get; set; }
        public Boolean taxExempt; //1= Purchase is tax emempt, 0=No tax emempt
        public string ticket { get; set; } //Trans No for refference Max length:9Char
        public string troutD { get; set; }
        public string zip { get; set; }
        public string printReceipts { get; set; }
        public Boolean xmlTrans { get; set; }
        public string memo { get; set; }
        public string autCode { get; set; }
        public Int16 organizationUnitId { get; set; }
    }


    public static class MembershipAuthorizationTypes
    {
        public const string MEMBERSHIPLISTONLY = "MEMBERSHIPLISTONLY";
        public const string ADDTIONALPROGRAMONLY = "ADDTIONALPROGRAMONLY";
        public const string BOTH = "BOTH";
    }
}
