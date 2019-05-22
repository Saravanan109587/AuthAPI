using mAuth.BusinessEntities;
using mAuth.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mAuth.BusinessLogics
{
    public class EventsBusiness: IEventBusiness
    {
        private IEventRepo _eventRepo;
        public EventsBusiness(IEventRepo eventRepo)
        {
            _eventRepo = eventRepo;
        }


        public List<Event> getAllEvents(int oId, DateTime visitingDate)
        {
            return _eventRepo.getAllEvents(oId, visitingDate);
        }
    }
}
