using System;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using mAuth.BusinessEntities;
using mAuth.Common;
using System.Collections.Generic;

namespace mAuth.BusinessLogics
{
    public class CityPassService : ICityPass
    {
        IHttpAccess _httpAccess;

        public CityPassService(IHttpAccess httpAccess)
        {
            _httpAccess = httpAccess;
            var authHeader = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.CITYPASS_USERNAME, Constants.CITYPASS_PASSWORD)))
            );
            _httpAccess.GetClient().DefaultRequestHeaders.Authorization = authHeader;
        }
                

        public async Task<TicketUsageCollectionResponse> Use(IAuthorizeRequest authorizeRequest)
        {
            TicketUsageCollectionRequest collectionRequest = new TicketUsageCollectionRequest();
            TicketUsageRequest request = new TicketUsageRequest();
            request.TicketBarcode = authorizeRequest.code;
            request.TicketUsageDate = DateTime.Now.ToString();
            request.UserID = authorizeRequest.updateID;
            collectionRequest.TicketUsageRequests = new List<TicketUsageRequest>();
            collectionRequest.TicketUsageRequests.Add(request);

            var url = Constants.CITYPASS_URL + Constants.CITYPASS_USE;
            var data = await _httpAccess.PostStringAsync(url, JsonConvert.SerializeObject(collectionRequest));
            TicketUsageCollectionResponse usageResponse = JsonConvert.DeserializeObject<TicketUsageCollectionResponse>(data, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            return usageResponse;
        }
    }

}
