using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace mAuth.BusinessEntities
{
    public interface IHttpAccess
    {
        HttpClient GetClient();
        Task<string> GetStringAsync(string uri);
        Task<string> PostStringAsync(string uri, string data);
    }
}