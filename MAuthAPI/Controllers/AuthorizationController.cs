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

namespace MAuthAPI.Controllers
{
    public class AuthorizationController : ApiController
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static IMauthBussiness authBussiness;

        // private static IConfiguration appconfig;
        /// <summary>
        /// 
        /// </summary>
        public AuthorizationController(IMauthBussiness _mAuthBussiness)
        {
            authBussiness = _mAuthBussiness;
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
        public List<IAttraction> GetAttractions()
        {
         
            _log.Debug("Entered into Get Attractions");
            return authBussiness.getAttractions();
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
           authorizeRequest.attraction, authorizeRequest.updateID, authorizeRequest.posId);

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
                                    result.membertypeResult = authBussiness.GetMembershipList(authorizeRequest.code, authorizeRequest.includeMemListImg);
                                    break;
                                case MembershipAuthorizationTypes.ADDTIONALPROGRAMONLY:
                                    result.memAdditionalPrograms = authBussiness.GetMembershipAdditionalPrograms(authorizeRequest.code, authorizeRequest.posId, authorizeRequest.updateID);
                                    break;

                                case MembershipAuthorizationTypes.BOTH:
                                    {
                                        result.membertypeResult = authBussiness.GetMembershipList(authorizeRequest.code, authorizeRequest.includeMemListImg);
                                        result.memAdditionalPrograms = authBussiness.GetMembershipAdditionalPrograms(authorizeRequest.code, authorizeRequest.posId, authorizeRequest.updateID);
                                    }
                                    break;
                            }

                        }
                        break;
                    case TrasactionTypes.TRANSACTIONWISE:
                        result.transactionResult = authBussiness.GetTicketList(authorizeRequest.code, "Authorization",
                            authorizeRequest.attraction, authorizeRequest.posId);
                        break;
                    case TrasactionTypes.TICKETWISE:
                        {
                            //Added this block to send actual attraction code present in the TicketAuditDetail instead of sending the selected attraction in the app.
                            //if (validateStatus.Length > 7)
                            //{
                                result.ticketResult = authBussiness.Authorize(authorizeRequest.code, authorizeRequest.attraction, authorizeRequest.updateID, "",
                                    authorizeRequest.posId);
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
                if (authMemOrTransReq != null && authMemOrTransReq.MemList != null && authMemOrTransReq.MemList.Count >0)
                {
                    _log.Debug("Entered into Membership List");
                    foreach (MembershipList member in authMemOrTransReq.MemList)
                    {

                        if (member.name == "Child")
                        {
                            for (int i = 1; i <= member.childCount; i++)
                            {
                                //Adding 999 as membership Line ID for UnNamed Child - Raj Venkat.
                                result = authBussiness.Authorize(member.membershipCode, authMemOrTransReq.Attraction, authMemOrTransReq.UpdateID, "999", authMemOrTransReq.POSID);
                            }
                        }
                        else
                        {
                            _log.Debug("Entered into Membership List Else");
                            _log.Debug("Membership Code " + member.membershipCode);
                            _log.Debug("Line Id" + member.membershipListLineId);
                            // _repository.Authorize(member.MembershipCode, authMemOrTransReq.Attraction, authMemOrTransReq.UpdateID, member.MembershipListLineId, authMemOrTransReq.POSID);
                            result = authBussiness.Authorize(member.membershipCode, Convert.ToString(member.membershipListLineId).Trim() == "0" ? member.name : authMemOrTransReq.Attraction, authMemOrTransReq.UpdateID, member.membershipListLineId, authMemOrTransReq.POSID, TransactionId);
                        }
                    }

                    if (authMemOrTransReq.MemList.Exists(x => x.membershipListLineId.Trim() == "0"))
                    {
                        SalesTransactionDetail salesTransactionDetail = new SalesTransactionDetail();
                        salesTransactionDetail.SalesTransactionId = TransactionId;
                        salesTransactionDetail.ItemNumber = string.Empty;
                        salesTransactionDetail.VisitorType = string.Empty;
                        salesTransactionDetail.UpdateId = authMemOrTransReq.UpdateID;
                        salesTransactionDetail.POSId = authMemOrTransReq.POSID;
                        authBussiness.UpdateMemberBenefits(salesTransactionDetail, 0, authMemOrTransReq.MemList.FirstOrDefault().membershipCode);
                    }

                     if (authMemOrTransReq.BenefitList !=null && authMemOrTransReq.BenefitList.Count > 0)
                    {
                          TransactionId = authBussiness.GetTransactionId(authMemOrTransReq.POSID, 1, authMemOrTransReq.UpdateID);
                            authBussiness.SaveSalesTransaction(authMemOrTransReq.BenefitList, authMemOrTransReq.POSID, authMemOrTransReq.UpdateID, TransactionId, authMemOrTransReq.MemList.FirstOrDefault().membershipCode);
                         
                    }

                }
               
                else if (authMemOrTransReq != null && authMemOrTransReq.TransList != null)
                {
                    var attractionCode = authMemOrTransReq.Attraction;

                    foreach (ITransactionTypeResult ticket in authMemOrTransReq.TransList)
                    {

                        for (int count = 0; count <= ticket.ticketCount - 1; count++)
                        {
                            result = authBussiness.Authorize(ticket.ticketId, attractionCode, authMemOrTransReq.UpdateID, "", authMemOrTransReq.POSID);
                        }

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
    }
}
