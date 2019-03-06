using mAuth.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace mAuth.BusinessLogics
{
    public class PeopleCounter : IPeopleCounterBusiness
    {
        private  readonly IPeopleCounterData _peopleCounterdata;

        public PeopleCounter(IPeopleCounterData people)
        {
            _peopleCounterdata = people;
        }

        public string AddPeopleCount(IPeopleCount data)
        {
            return _peopleCounterdata.AddPeopleCount(data);
        }
    }
}
