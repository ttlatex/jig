using B1.QueryFactory;
using B1.Dto;
using Jig.QueryControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace B1.Business
{
    public class ListValueSelector
    {
        public static List<SelectedRecord> SelectItems()
        {
            var connectionstring = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;

            using (var connection = new OracleConnection(connectionstring))
            {
                connection.Open();
                
                return B1Query.SerachQuerry(connection);
            }
        }
    }
}
