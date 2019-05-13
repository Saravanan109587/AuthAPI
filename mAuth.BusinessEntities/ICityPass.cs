using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mAuth.BusinessEntities
{
    // City Pass Related

    public interface ICityPass
    {
        Task<TicketUsageCollectionResponse> Use(IAuthorizeRequest authorizeRequest);
        //Task<TicketUsageCollectionResponse> Use(TicketUsageCollectionRequest ticketUsageCollectionRequest);
        //Task<TicketUsageNotificationResponse> NotifyUsage(TicketUsageCollectionRequest ticketUsageCollectionRequest);
    }

    public class TicketUsageNotificationResponse
    {
        public bool ReceivedSuccess { get; set; }
        public string Message { get; set; }
        public int ReceivedCount { get; set; }
    }

    public class TicketUsageRequest
    {
        public string TicketBarcode { get; set; }
        public string TicketUsageDate { get; set; }
        public string UserID { get; set; }
        public int BatchID { get; set; }
        public string InputMethod { get; set; }
        public string InputString { get; set; }
        public int AttractionID { get; set; }
        public bool IsAutoScan { get; set; }
        public string AttractionProductCode { get; set; }
    }

    public class TicketUsageCollectionRequest
    {
        public List<TicketUsageRequest> TicketUsageRequests { get; set; }
    }

    public class SplitTicket
    {
        public int TicketID { get; set; }
        public string TicketBarcode { get; set; }
        public int TicketStatusID { get; set; }
        public string TicketStatusDescription { get; set; }
        public int AttractionID { get; set; }
        public string AttractionName { get; set; }
    }

    public class TicketComment
    {
        public int CommentID { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUserID { get; set; }
    }

    public class TicketProblem
    {
        public int TicketUsageProblemID { get; set; }
        public string WarningMessage { get; set; }
        public bool IsFailure { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class City
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public string BookletCityCode { get; set; }
    }

    public class ParentProduct
    {
        public string Name { get; set; }
        public int ProductParentID { get; set; }
    }

    public class AttractionAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string StateProvince { get; set; }
        public string ZipCode { get; set; }
    }

    public class CityCollection
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public string BookletCityCode { get; set; }
    }

    public class GatePriceCollection
    {
        public DateTime AttractionEffectiveDate { get; set; }
        public DateTime AttractionEffectiveEndDate { get; set; }
        public int AttractionGatePriceID { get; set; }
        public int CommissionBasis { get; set; }
        public int CommissionBasisAffiliateVisit { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public int ModelID { get; set; }
        public int Price { get; set; }
        public int TicketClassID { get; set; }
        public string TicketClassName { get; set; }
        public int ProductFamilyID { get; set; }
        public bool ApplyToAllCTicketProductTypes { get; set; }
        public DateTime EarliestEditDate { get; set; }
    }

    public class Configuration
    {
        public bool AllowSoftRedemptions { get; set; }
        public bool AllowManualVoucherPoe { get; set; }
        public bool AllowTicketScanningPoe { get; set; }
        public string PassExchangeScanMethod { get; set; }
        public bool DisplayPointOfEntryHistory { get; set; }
        public bool IsAttraction { get; set; }
        public bool IsRedemptionAttraction { get; set; }
        public bool SoftRedemptionZipCodeChallenge { get; set; }
    }

    public class DailyData
    {
        public DateTime ActivityDate { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public bool HasExplicitZeroEntry { get; set; }
        public bool IsOpen { get; set; }
        public bool IsValid { get; set; }
    }

    public class ValidationRule
    {
        public int RuleID { get; set; }
        public string RuleName { get; set; }
        public int ProcessOrder { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; }
        public List<DailyData> DailyData { get; set; }
        public string CancelValidationMessage { get; set; }
    }

    public class ValidAttractionCollection
    {
        public AttractionAddress AttractionAddress { get; set; }
        public int AttractionID { get; set; }
        public List<CityCollection> CityCollection { get; set; }
        public List<GatePriceCollection> GatePriceCollection { get; set; }
        public List<int> CityProductFamilies { get; set; }
        public Configuration Configuration { get; set; }
        public bool IsPrimaryAttraction { get; set; }
        public string Name { get; set; }
        public int TimeZoneOffset { get; set; }
        public List<ValidationRule> ValidationRules { get; set; }
        public bool IsOpenMonday { get; set; }
        public bool IsOpenTuesday { get; set; }
        public bool IsOpenWednesday { get; set; }
        public bool IsOpenThursday { get; set; }
        public bool IsOpenFriday { get; set; }
        public bool IsOpenSaturday { get; set; }
        public bool IsOpenSunday { get; set; }
        public bool IsAttraction { get; set; }
        public string LatestAttractionBookletCode { get; set; }
        public bool IsMediaCardProduct { get; set; }
    }

    public class ProductPrice
    {
        public int ProductID { get; set; }
        public int ProductPriceID { get; set; }
        public int PassOnsiteSaleCommission { get; set; }
        public int VoucherRedemptionCommission { get; set; }
        public int Price { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyID { get; set; }
        public int Tax { get; set; }
        public int TaxTypeID { get; set; }
        public string TaxTypeName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int Revision { get; set; }
    }

    public class Product
    {
        public City City { get; set; }
        public bool IsAvailableForOrder { get; set; }
        public bool IsCityPassOnly { get; set; }
        public string Name { get; set; }
        public ParentProduct ParentProduct { get; set; }
        public int ProductID { get; set; }
        public string ProgramYear { get; set; }
        public int ProgramYearID { get; set; }
        public int TicketClassId { get; set; }
        public List<ValidAttractionCollection> ValidAttractionCollection { get; set; }
        public List<ProductPrice> ProductPrices { get; set; }
        public int CityProgramYearID { get; set; }
        public int TimeZoneOffset { get; set; }
        public int UsageModelID { get; set; }
        public int ModelID { get; set; }
        public int ProductFamilyID { get; set; }
        public int CityID { get; set; }
        public string AttractionProductCode { get; set; }
        public int CityModelProgramYearID { get; set; }
        public int ProductTypeID { get; set; }
    }

    public class City2
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public string BookletCityCode { get; set; }
    }

    public class ParentProduct2
    {
        public string Name { get; set; }
        public int ProductParentID { get; set; }
    }

    public class AttractionAddress2
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string StateProvince { get; set; }
        public string ZipCode { get; set; }
    }

    public class CityCollection2
    {
        public int CityID { get; set; }
        public string Name { get; set; }
        public string BookletCityCode { get; set; }
    }

    public class GatePriceCollection2
    {
        public DateTime AttractionEffectiveDate { get; set; }
        public DateTime AttractionEffectiveEndDate { get; set; }
        public int AttractionGatePriceID { get; set; }
        public int CommissionBasis { get; set; }
        public int CommissionBasisAffiliateVisit { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public int ModelID { get; set; }
        public int Price { get; set; }
        public int TicketClassID { get; set; }
        public string TicketClassName { get; set; }
        public int ProductFamilyID { get; set; }
        public bool ApplyToAllCTicketProductTypes { get; set; }
        public DateTime EarliestEditDate { get; set; }
    }

    public class Configuration2
    {
        public bool AllowSoftRedemptions { get; set; }
        public bool AllowManualVoucherPoe { get; set; }
        public bool AllowTicketScanningPoe { get; set; }
        public string PassExchangeScanMethod { get; set; }
        public bool DisplayPointOfEntryHistory { get; set; }
        public bool IsAttraction { get; set; }
        public bool IsRedemptionAttraction { get; set; }
        public bool SoftRedemptionZipCodeChallenge { get; set; }
    }

    public class DailyData2
    {
        public DateTime ActivityDate { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public bool HasExplicitZeroEntry { get; set; }
        public bool IsOpen { get; set; }
        public bool IsValid { get; set; }
    }

    public class ValidationRule2
    {
        public int RuleID { get; set; }
        public string RuleName { get; set; }
        public int ProcessOrder { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; }
        public List<DailyData2> DailyData { get; set; }
        public string CancelValidationMessage { get; set; }
    }

    public class ValidAttractionCollection2
    {
        public AttractionAddress2 AttractionAddress { get; set; }
        public int AttractionID { get; set; }
        public List<CityCollection2> CityCollection { get; set; }
        public List<GatePriceCollection2> GatePriceCollection { get; set; }
        public List<int> CityProductFamilies { get; set; }
        public Configuration2 Configuration { get; set; }
        public bool IsPrimaryAttraction { get; set; }
        public string Name { get; set; }
        public int TimeZoneOffset { get; set; }
        public List<ValidationRule2> ValidationRules { get; set; }
        public bool IsOpenMonday { get; set; }
        public bool IsOpenTuesday { get; set; }
        public bool IsOpenWednesday { get; set; }
        public bool IsOpenThursday { get; set; }
        public bool IsOpenFriday { get; set; }
        public bool IsOpenSaturday { get; set; }
        public bool IsOpenSunday { get; set; }
        public bool IsAttraction { get; set; }
        public string LatestAttractionBookletCode { get; set; }
        public bool IsMediaCardProduct { get; set; }
    }

    public class ProductPrice2
    {
        public int ProductID { get; set; }
        public int ProductPriceID { get; set; }
        public int PassOnsiteSaleCommission { get; set; }
        public int VoucherRedemptionCommission { get; set; }
        public int Price { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyID { get; set; }
        public int Tax { get; set; }
        public int TaxTypeID { get; set; }
        public string TaxTypeName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int Revision { get; set; }
    }

    public class Product2
    {
        public City2 City { get; set; }
        public bool IsAvailableForOrder { get; set; }
        public bool IsCityPassOnly { get; set; }
        public string Name { get; set; }
        public ParentProduct2 ParentProduct { get; set; }
        public int ProductID { get; set; }
        public string ProgramYear { get; set; }
        public int ProgramYearID { get; set; }
        public int TicketClassId { get; set; }
        public List<ValidAttractionCollection2> ValidAttractionCollection { get; set; }
        public List<ProductPrice2> ProductPrices { get; set; }
        public int CityProgramYearID { get; set; }
        public int TimeZoneOffset { get; set; }
        public int UsageModelID { get; set; }
        public int ModelID { get; set; }
        public int ProductFamilyID { get; set; }
        public int CityID { get; set; }
        public string AttractionProductCode { get; set; }
        public int CityModelProgramYearID { get; set; }
        public int ProductTypeID { get; set; }
    }

    public class AttractionProduct
    {
        public int AttractionID { get; set; }
        public Product2 Product { get; set; }
        public int ProductID { get; set; }
    }

    public class Ticket
    {
        public int TicketID { get; set; }
        public string TicketBarcode { get; set; }
        public string ScannedBarcode { get; set; }
        public int TicketStatusID { get; set; }
        public string TicketStatusDescription { get; set; }
        public int AttractionID { get; set; }
        public string AttractionName { get; set; }
        public int AttractionMonthStatusID { get; set; }
        public SplitTicket SplitTicket { get; set; }
        public List<TicketComment> TicketComments { get; set; }
        public List<TicketProblem> TicketProblems { get; set; }
        public DateTime UsageDate { get; set; }
        public DateTime ScanDate { get; set; }
        public string UsageUserID { get; set; }
        public int BatchRedemptionLogID { get; set; }
        public string BatchRedemptionLogType { get; set; }
        public string BatchRedemptionLogTypeName { get; set; }
        public string TicketClass { get; set; }
        public string TicketClassName { get; set; }
        public Product Product { get; set; }
        public AttractionProduct AttractionProduct { get; set; }
        public string AttractionProductName { get; set; }
        public DateTime FirstUsageBeforeDate { get; set; }
        public DateTime ExpiresAfter { get; set; }
        public bool IsExpired { get; set; }
        public DateTime CalculatedExpirationDate { get; set; }
        public bool IsMobileTicketBarcode { get; set; }
    }

    public class ReturnData
    {
        public string FailureReason { get; set; }
        public bool IsAutoScanFailure { get; set; }
        public string Message { get; set; }
        public string ReturnValue { get; set; }
        public Ticket Ticket { get; set; }
        public int PassID { get; set; }
        public string TicketBarcodeRequested { get; set; }
        public bool IsAttractionInventory { get; set; }
        public DateTime UsageDate { get; set; }
        public string ExternalProductCode { get; set; }
    }

    public class TicketUsageCollectionResponse
    {
        public string Message { get; set; }
        public List<ReturnData> ReturnData { get; set; }
        public string ReturnValue { get; set; }
        public string ReturnValueText { get; set; }
    }

}
