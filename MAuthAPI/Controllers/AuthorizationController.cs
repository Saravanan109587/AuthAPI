using log4net;
using mAuth.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Transactions;
using mAuth.Common;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace MAuthAPI.Controllers
{
    public class AuthorizationController : ApiController
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static IMauthBussiness authBussiness;
        private static ICityPass cityPass;

        // private static IConfiguration appconfig;
        /// <summary>
        /// 
        /// </summary>
        public AuthorizationController(IMauthBussiness _mAuthBussiness, ICityPass _cityPass)
        {
            authBussiness = _mAuthBussiness;
            cityPass = _cityPass;
            //appconfig = _appconfig;
            _log.Debug("Entered into mAUth controller");

        }

        /// <summary>
        /// get all the IOS Station List
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        public List<IAuthStation> GetStationList()
        {
            _log.Debug("Entered intoGet Stattion List");

            return authBussiness.getAuthStations();

        }
        /// <summary>
        /// get all the valid attractions
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        public List<IAttraction> GetAttractions(int id)
        {
         
            _log.Debug("Entered into Get Attractions");
            return authBussiness.getAttractions(id);
        }

        /// <summary>
        /// Validate and Authorize Sync Data  
        /// </summary>
        /// <param name="authorizeRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public  async Task<string> ValidateandAuthSyncData(IAuthorizeRequest authorizeRequest)
        {
            try
            {
                ICodeValidationResult validatedResult = new ICodeValidationResult();
                if (authorizeRequest.thirdPartyType ==  Constants.CITYPASS && authorizeRequest.thirdPartyVerificationStatus == Constants.THIRDPARTYNOTVERIFIED)
                {
                    var result = await cityPass.Use(authorizeRequest);
                    if(result != null && result.ReturnData != null && result.ReturnData.Count > 0 && result.ReturnData[0].FailureReason == null)
                    {
                        var cityPassVisitorType = result.ReturnData[0].Ticket.TicketClassName;
                        var ActualVisitorType = System.Configuration.ConfigurationManager.AppSettings[cityPassVisitorType];
                        authorizeRequest.visitorType = ActualVisitorType;
                        authBussiness.SaveCityPassTransaction(authorizeRequest, System.Configuration.ConfigurationManager.AppSettings[Constants.CITYPASSTICKETCODE], System.Configuration.ConfigurationManager.AppSettings[Constants.CITYPASSTICKETDESCRIPTION]);
                    }
                    if (result.ReturnData[0].FailureReason != null)
                        validatedResult.ticketResult = result.ReturnData[0].Message;
                }
                else if(authorizeRequest.thirdPartyType == Constants.CITYPASS && authorizeRequest.thirdPartyVerificationStatus == Constants.THIRDPARTYVERIFIED)
                {
                    var cityPassVisitorType = authorizeRequest.visitorType;
                    var ActualVisitorType = System.Configuration.ConfigurationManager.AppSettings[cityPassVisitorType];
                    authorizeRequest.visitorType = ActualVisitorType;
                    authBussiness.SaveCityPassTransaction(authorizeRequest, System.Configuration.ConfigurationManager.AppSettings[Constants.CITYPASSTICKETCODE], System.Configuration.ConfigurationManager.AppSettings[Constants.CITYPASSTICKETDESCRIPTION]);
                }
                else
                      validatedResult = Validate(authorizeRequest);

                if (validatedResult.type == TrasactionTypes.TRANSACTIONWISE)
                {
                    AuthorizeMemOrTransRequest authMemOrTransReq = new AuthorizeMemOrTransRequest();
                    authMemOrTransReq.Attraction = authorizeRequest.attraction;
                    authMemOrTransReq.POSID = authorizeRequest.posId;
                    authMemOrTransReq.TransList = validatedResult.transactionResult;
                    authMemOrTransReq.UpdateID = authorizeRequest.updateID;
                    authMemOrTransReq.code = authorizeRequest.code;
                    authMemOrTransReq.LocationId = authorizeRequest.locationId;
                    return AuthorizeMemOrTrans(authMemOrTransReq);
                }
                else if (validatedResult.type == TrasactionTypes.MEMBERWISE)
                {
                    AuthorizeMemOrTransRequest authMemOrTransReq = new AuthorizeMemOrTransRequest();
                    authMemOrTransReq.Attraction = authorizeRequest.attraction;
                    authMemOrTransReq.POSID = authorizeRequest.posId;
                    authMemOrTransReq.MemList = mapMembershipListData(validatedResult.membertypeResult.membershipList);
                    authMemOrTransReq.validAttractions = validatedResult.membertypeResult.validAttractions;
                    authMemOrTransReq.UpdateID = authorizeRequest.updateID;
                    authMemOrTransReq.LocationId = authorizeRequest.locationId;
                    return AuthorizeMemOrTrans(authMemOrTransReq);
                }
                else if(validatedResult.ticketResult != null)
                {
                    authBussiness.InsertFailedOfflineData(authorizeRequest, validatedResult.ticketResult);
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Map finded all members to authorize for offline
        /// </summary>
        /// <param name="sourceMemershipList"></param>
        /// <returns></returns>
        public List<MembershipList> mapMembershipListData(List<IMemershipList> sourceMemershipList)
        {
            List<MembershipList> destMemershipList = new List<MembershipList>();

            foreach (IMemershipList sourceMemership in sourceMemershipList) {
                MembershipList destMemership = new MembershipList();
                destMemership.membershipCode = sourceMemership.membershipcode;
                destMemership.membershipListLineId = sourceMemership.membershipListLineId.ToString();
                destMemership.name = sourceMemership.name;
                destMemership.organizationUnitId = (Int16)sourceMemership.organizationUnitId;
                destMemershipList.Add(destMemership);
            }

            return destMemershipList;
        }


        /// <summary>
        /// validate the id and return the result is ticket id authorize automatically
        /// </summary>
        /// <param name="authorizeRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public ICodeValidationResult Validate(IAuthorizeRequest authorizeRequest)
        {

            _log.Debug("Entered into validate with code" + " " + authorizeRequest.code);
            //var config = appconfig.GetValue<string>("AppConfig:MembershipAuthorizationType");
            ICodeValidationResult result = new ICodeValidationResult();
            var authStatus = authBussiness.Validate(authorizeRequest.code,
           authorizeRequest.attraction, authorizeRequest.updateID, authorizeRequest.posId, authorizeRequest.createdDate);

            var validity = string.Empty;
            string[] validateStatus = authStatus.Split(new char[] { '|' });
            _log.Debug("Status" + " " + validateStatus[0]);
            if (validateStatus[0] == "Valid")//ValidStatus
            {
                result.type = validateStatus[3];
                _log.Debug("Type " + " " + validateStatus[3]);
                switch (validateStatus[3])//AuthType
                {

                    case TrasactionTypes.MEMBERWISE:
                        {
                            var authtype = System.Configuration.ConfigurationManager.AppSettings["MEMBERAUTHTYPE"].ToString();

                            switch (authtype.Trim().ToUpper())
                            {
                                case MembershipAuthorizationTypes.MEMBERSHIPLISTONLY:
                                    result.membertypeResult = authBussiness.GetMembershipList(authorizeRequest.code, authorizeRequest.includeMemListImg, authorizeRequest.attraction);
                                    break;
                                case MembershipAuthorizationTypes.ADDTIONALPROGRAMONLY:
                                    result.memAdditionalPrograms = authBussiness.GetMembershipAdditionalPrograms(authorizeRequest.code, authorizeRequest.posId, authorizeRequest.updateID);
                                    break;

                                case MembershipAuthorizationTypes.BOTH:
                                    {
                                        result.membertypeResult = authBussiness.GetMembershipList(authorizeRequest.code, authorizeRequest.includeMemListImg, authorizeRequest.attraction);
                                        result.memAdditionalPrograms = authBussiness.GetMembershipAdditionalPrograms(authorizeRequest.code, authorizeRequest.posId, authorizeRequest.updateID);
                                    }
                                    break;
                            }

                        }
                        break;
                    case TrasactionTypes.TRANSACTIONWISE:
                        result.transactionResult = authBussiness.GetTicketList(authorizeRequest.code,authorizeRequest.attraction, authorizeRequest.posId, authorizeRequest.isGroupTicket);
                        break;
                    case TrasactionTypes.TICKETWISE:
                        {
                            //Added this block to send actual attraction code present in the TicketAuditDetail instead of sending the selected attraction in the app.
                            //if (validateStatus.Length > 7)
                            //{
                                result.ticketResult = authBussiness.Authorize(authorizeRequest.code, authorizeRequest.attraction, authorizeRequest.updateID, "",
                                    authorizeRequest.posId,"");
                            //}
                            //else
                            //{
                            //    throw new ApplicationException("Attraction Not found. ");
                            //}
                            break;
                        }
                }
            }
            else
                result.ticketResult = validateStatus[4];

            return result;

        }

        /// <summary>
        /// Authorize membership and transaction after selected by customer
        /// </summary>
        /// <param name="authMemOrTransReq"></param>
        [HttpPost]
        public string AuthorizeMemOrTrans(AuthorizeMemOrTransRequest authMemOrTransReq)
        {
            string TransactionId = "";
            string result = string.Empty;
            _log.Debug("Entered into AuthorizeMemOrTrans");
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(authMemOrTransReq);

            try
            {
                if (authMemOrTransReq != null && authMemOrTransReq.MemList != null && authMemOrTransReq.MemList.Count > 0)
                {
                    _log.Debug("Entered into Membership List");
                    List<string> UserSelectedAttraction = authMemOrTransReq.Attraction.Split('|').ToList();
                    List<string> validAttraction = new List<string>();
                    foreach (string selectedAttr in UserSelectedAttraction)
                    {
                        List<string> datas = authMemOrTransReq.validAttractions.FindAll(validAttr => validAttr.Trim() == selectedAttr.Trim());
                        validAttraction = validAttraction.Concat(datas).ToList();

                    }


                    if (validAttraction != null && validAttraction.Count > 0)
                    {
                        foreach (string attraction in validAttraction)
                        {
                            foreach (MembershipList member in authMemOrTransReq.MemList)
                            {

                                if (member.name == "Child")
                                {
                                    for (int i = 1; i <= member.childCount; i++)
                                    {
                                        //Adding 999 as membership Line ID for UnNamed Child - Raj Venkat.
                                        result = authBussiness.Authorize(member.membershipCode, attraction, authMemOrTransReq.UpdateID, "999", authMemOrTransReq.POSID);
                                    }
                                }
                                else
                                {
                                    _log.Debug("Entered into Membership List Else");
                                    _log.Debug("Membership Code " + member.membershipCode);
                                    _log.Debug("Line Id" + member.membershipListLineId);
                                    // _repository.Authorize(member.MembershipCode, authMemOrTransReq.Attraction, authMemOrTransReq.UpdateID, member.MembershipListLineId, authMemOrTransReq.POSID);

                                    result = authBussiness.Authorize(member.membershipCode, Convert.ToString(member.membershipListLineId).Trim() == "0" ? member.name : attraction, authMemOrTransReq.UpdateID, member.membershipListLineId, authMemOrTransReq.POSID, TransactionId);
                                }
                            }
                        }
                        //if (authMemOrTransReq.MemList.Exists(x => x.membershipListLineId.Trim() == "0"))
                        //{
                        //    SalesTransactionDetail salesTransactionDetail = new SalesTransactionDetail();
                        //    salesTransactionDetail.SalesTransactionId = TransactionId;
                        //    salesTransactionDetail.ItemNumber = string.Empty;
                        //    salesTransactionDetail.VisitorType = string.Empty;
                        //    salesTransactionDetail.UpdateId = authMemOrTransReq.UpdateID;
                        //    salesTransactionDetail.POSId = authMemOrTransReq.POSID;
                        //    authBussiness.UpdateMemberBenefits(salesTransactionDetail, 0, authMemOrTransReq.MemList.FirstOrDefault().membershipCode);
                        //}

                    }
                    else
                        throw new Exception("Invalid Attraction");
                }
                if (authMemOrTransReq.BenefitList != null && authMemOrTransReq.BenefitList.Count > 0)
                {
                    TransactionId = authBussiness.GetTransactionId(authMemOrTransReq.POSID, authMemOrTransReq.LocationId, authMemOrTransReq.UpdateID);
                    authBussiness.SaveSalesTransaction(authMemOrTransReq.BenefitList, authMemOrTransReq.POSID, authMemOrTransReq.UpdateID, TransactionId, authMemOrTransReq.code, authMemOrTransReq.LocationId);

                }

                else if (authMemOrTransReq != null && authMemOrTransReq.TransList != null)
                    {
                        var attractionCode = authMemOrTransReq.Attraction;

                        foreach (ITransactionTypeResult ticket in authMemOrTransReq.TransList)
                        {
                          result = authBussiness.Authorize(ticket.ticketId, ticket.attractionCode, authMemOrTransReq.UpdateID, "", authMemOrTransReq.POSID, ticket.transactionId, ticket.quantity,ticket.visitorType,ticket.eventTime,ticket.ticketCode);
                        }

                    }
                
            }
            catch (Exception e)
            {
                result = "Failed";
                throw;
            }
        

            return result;

        }

        /// <summary>
        /// To Search the membership based on the given filters
        /// </summary>
        /// <returns></returns>
        public List<IMembershipSearchList> MembershipSearch(IMembershipSearchFilter memberFilter)
        {
            return authBussiness.MembershipSearch(memberFilter);
        }
    }
}
