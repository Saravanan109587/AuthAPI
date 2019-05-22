using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mAuth.BusinessEntities
{
    public class Event
    {
        public string eventCode { get; set; }
        public string description { get; set; }
    }


   public interface IEventBusiness
    {
        List<Event> getAllEvents(int oId, DateTime vistingDate);
    }


    public interface IEventRepo
    {
        List<Event> getAllEvents(int oId, DateTime vistingDate);
    }




}
