using mAuth.BusinessEntities;
using mAuth.BusinessLogics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MAuthAPI.Controllers
{
    public class EventController : ApiController
    {
        private IEventBusiness _eventBusiness;
        public EventController(IEventBusiness business)
        {
            _eventBusiness = business;
        }


        public List<Event> GetEventsByDate(int oId,DateTime date)
        {
            return _eventBusiness.getAllEvents(oId, date);
        }

        public void GetSalesTransaction(string eventCode)
        {

        }
    }
}
