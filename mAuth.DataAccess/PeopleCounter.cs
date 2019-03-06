using mAuth.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace mAuth.DataAccess
{
    public class PeopleCounter : IPeopleCounterData
    {
        private readonly string _connection;
        public PeopleCounter(string connection)
        {
            _connection = connection;
        }
        public string AddPeopleCount(IPeopleCount data)
        {

            string result = string.Empty;
            DynamicParameters param = new DynamicParameters();
            param.Add("@attractioncode", data.attractionCode);
            param.Add("@posId", data.posId);
            param.Add("@updateId", data.updateId);
            param.Add("@dateOfShow", data.dateOfShow);
            param.Add("@timeOfShow", data.timeOfShow);
            param.Add("@count", data.count);

            try
            {

                using (var con = new SqlConnection(_connection))
                {

                    result = con.Query<string>("usp_IOS_Add_PeopleCount", param, commandType: CommandType.StoredProcedure).SingleOrDefault<string>();

                }
            }
            catch (Exception e)
            {

                throw;
            }
            return result;
        }
    }
}
