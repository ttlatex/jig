using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.QueryControl
{
    public class QueryExecuter
    {
        /// <summary>
        /// データソースのコネクション文字列
        /// </summary>
        private string ConnectionString;

        /// <summary>
        /// 与えられたconfigSectionNameの接続文字列を用いて初期化を行います
        /// </summary>
        /// <param name="configSectionName"></param>
        public QueryExecuter(string configSectionName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[configSectionName];
            if (connectionString == null) throw new ArgumentNullException(configSectionName);
            this.ConnectionString = connectionString.ConnectionString;
        }

        /// <summary>
        /// コンフィグ内「connectionStrings->OracleConnection」を利用
        /// </summary>
        public QueryExecuter()
            : this("OracleConnection")
        {
        }

        public int StoredProcedureNonMap(Query query)
        {
            using (var connection = new OracleConnection(this.ConnectionString))
            {
                connection.Open();
                return 0;
            }
        }
    }
}
