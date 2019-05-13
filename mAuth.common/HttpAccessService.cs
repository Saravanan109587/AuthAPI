using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using mAuth.BusinessEntities;

namespace mAuth.Common
{
    public class HttpAccessService : IHttpAccess
    {
        private HttpClient _httpClient;

        public HttpAccessService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(Constants.TIMEOUT_PERIOD);
        }

        public HttpClient GetClient()
        {
            return _httpClient;
        }

        public async Task<string> GetStringAsync(string uri)
        {
            try
            {
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return body;
                }
                // exceptions happen on api side
                throw new Exception(body);
            }
            catch (Exception ex)
            {
                // exceptions like timeout, network error
                throw ex;
            }
        }

        public async Task<string> PostStringAsync(string uri, string data)
        {
            try
            {
                var response = await _httpClient.PostAsync(uri, new StringContent(data, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return body;
                }
                // exceptions happen on api side
                throw new Exception(body);

            }
            catch (Exception ex)
            {
                // exceptions like timeout, network error
                throw ex;
            }

        }
    }
}
