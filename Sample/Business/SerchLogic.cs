using Dapper;
using Jig.QueryControl;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Data;

namespace A1.Business
{
    public class SerchLogic
    {
        private QueryExecuter executer = new QueryExecuter();

        public string SearchName(string id)
        {
            var connectionstring = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;

            using (var connection = new OracleConnection(connectionstring))
            {
                connection.Open();

                var param = new { IN_ID = id, IN_PHONE_NUMBER = "" };
                var outName = connection.QuerySingleOrDefault<string>("PKG_A1.SELECT_NAME", param, commandType: CommandType.StoredProcedure);

                return (outName == null)
                    ? "検索結果は0件です"
                    : outName;
            }
        }
    }
}
