using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace Jig.QueryControl
{
    public class Query
    {
        /// <summary>
        /// SQL文
        /// </summary>
        public string CommandText { get; private set; }
        /// <summary>
        /// バインドパラメータ
        /// </summary>
        public List<OracleParameter> Parameters { get; private set; }

        public Query(string commandText, List<OracleParameter> parameers)
        {
            this.CommandText = commandText;
            this.Parameters = parameers;
        }
    }
}
