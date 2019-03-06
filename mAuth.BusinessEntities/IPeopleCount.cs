using System;
using System.Collections.Generic;
using System.Text;

namespace mAuth.BusinessEntities
{
     public class IPeopleCount
    {
        public string attractionCode { get; set; }
        public string updateId { get; set; }
        public DateTime dateOfShow { get; set; }
        public string timeOfShow { get; set; }
        public int count { get; set; }
        public int posId { get; set; }

    }

    public interface IPeopleCounterBusiness
    {
        string AddPeopleCount(IPeopleCount data);
    }

    public interface IPeopleCounterData
    {
        string AddPeopleCount(IPeopleCount data);
    }
}
