using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
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

        #region 生成
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
        #endregion

        #region プロシージャ実行
        /// <summary>
        /// マップなしストアドプロシージャ
        /// </summary>
        /// <returns>更新レコード件数</returns>
        public int StoreProcedureNotMap(Query query)
        {
            using (var connection = new OracleConnection(this.ConnectionString))
            {
                connection.Open();
                var command = QueryJigInternal.CreateCommandBase(query, CommandType.StoredProcedure, connection);
                return command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// マップなしストアドプロシージャ
        /// </summary>
        public OutputDto StoreProcedureNotMap<OutputDto>(Query query) where OutputDto : new()
        {
            using (var connection = new OracleConnection(this.ConnectionString))
            {
                connection.Open();
                var command = QueryJigInternal.CreateCommandBase(query, CommandType.StoredProcedure, connection);
                command.ExecuteNonQuery();

                return QueryJigInternal.MapValue<OutputDto>(command);
            }
        }
        #endregion

        #region SELECT実行
        /// <summary>
        /// SELECT実行
        /// </summary>
        public List<DbDataRecord> ExecuteSelect(Query query)
        {
            using (var connection = new OracleConnection(this.ConnectionString))
            {
                connection.Open();
                var command = QueryJigInternal.CreateCommandBase(query, CommandType.Text, connection);
                var reader = command.ExecuteReader();

                return reader.Cast<DbDataRecord>().ToList();
            }
        }
        /// <summary>
        /// SELECT実行
        /// </summary>
        public List<OutputDto> ExecuteSelect<OutputDto>(Query query) where OutputDto : new()
        {
            var records = this.ExecuteSelect(query);
            return QueryJigInternal.MapValue<OutputDto>(records);
        }
        #endregion

        #region INSERT, UPDATE, DELETE実行
        /// <summary>
        /// INSERT, UPDATE, DELETE実行
        /// </summary>
        /// <param name="query"></param>
        /// <returns>更新レコード件数</returns>
        public int ExecuteNonQuery(Query query)
        {
            using (var connection = new OracleConnection(this.ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var command = QueryJigInternal.CreateCommandBase(query, CommandType.Text, connection);

                try
                {
                    return command.ExecuteNonQuery();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
        /// <summary>
        /// INSERT, UPDATE, DELETE実行
        /// バッチ実行
        /// </summary>
        /// <param name="batchCount">実行レコード数</param>
        /// <returns></returns>
        public int ExecuteNonQueryBatch(Query query, int batchCount)
        {
            using (var connection = new OracleConnection(this.ConnectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                var command = QueryJigInternal.CreateCommandBase(query, CommandType.Text, connection);
                command.ArrayBindCount = batchCount;

                try
                {
                    return command.ExecuteNonQuery();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
        #endregion
    }
}
