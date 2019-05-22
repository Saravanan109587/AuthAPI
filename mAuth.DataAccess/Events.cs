using Dapper;
using mAuth.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mAuth.DataAccess
{
    public class EventsRepo : IEventRepo
    {
        private string connection;
        public EventsRepo(string _connection)
        {
            connection = _connection;
        }


        public List<Event> getAllEvents(int oId, DateTime visitingDate)
        {
            List<Event> eventList;

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@OrganizationUnitId", oId);
            parameter.Add("@visitingDate", visitingDate);

            try
            {
                using (var con = new SqlConnection(connection))
                {
                    var result = con.QueryMultiple("usp_ios_GetEducationProgramCapacity",
                         parameter,
                         commandType: CommandType.StoredProcedure);
                    eventList = result.Read<Event>().ToList();
                }

                return eventList;
            }
            catch (Exception e)
            {
                throw;
            }


        }
    }
}
