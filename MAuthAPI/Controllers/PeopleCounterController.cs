using mAuth.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAuthAPI.Controllers
{
    public class PeopleCountController : ApiController
    {
        private readonly IPeopleCounterBusiness _people;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="people"></param>
        public PeopleCountController(IPeopleCounterBusiness people)
        {
            _people = people;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddPeopleCount(IPeopleCount data)
        {
            return _people.AddPeopleCount(data);
        }
    }
}
