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
    /// <summary>
    /// トランザクションをシェアするQueryExecuter
    /// </summary>
    public class QueryExecuterShare : IDisposable
    {
        #region メンバ変数
        /// <summary>
        /// コネクション
        /// </summary>
        private OracleConnection connection;
        /// <summary>
        /// トランザクション
        /// </summary>
        private OracleTransaction transaction;
        #endregion

        #region 生成
        /// <summary>
        /// 与えられたconfigSectionNameの接続文字列を用いて初期化を行います
        /// </summary>
        /// <param name="configSectionName"></param>
        public QueryExecuterShare(string configSectionName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[configSectionName];
            if (connectionString == null) throw new ArgumentNullException(configSectionName);
            this.connection = new OracleConnection(connectionString.ConnectionString);
            this.connection.Open();
        }

        /// <summary>
        /// コンフィグ内「connectionStrings->OracleConnection」を利用
        /// </summary>
        public QueryExecuterShare()
            : this("OracleConnection")
        {
        }
        #endregion

        #region トランザクション制御
        /// <summary>
        /// トランザクションを開始します
        /// </summary>
        public void BeginTransaction()
        {
            this.transaction = this.connection.BeginTransaction();
        }
        /// <summary>
        /// コミット
        /// </summary>
        public void Commit()
        {
            this.transaction.Commit();
        }
        /// <summary>
        /// ロールバック
        /// </summary>
        public void Rollback()
        {
            this.transaction.Rollback();
        }
        /// <summary>
        /// トランザクションのSave
        /// </summary>
        public void Save(string savePointName)
        {
            this.transaction.Save(savePointName);
        }
        /// <summary>
        /// savePointに戻す
        /// </summary>
        public void Rollback(string savePointName)
        {
            this.transaction.Rollback(savePointName);
        }

        #endregion

        #region プロシージャ実行
        /// <summary>
        /// マップなしストアドプロシージャ
        /// </summary>
        /// <returns>更新レコード件数</returns>
        public int StoreProcedureNotMap(Query query)
        {
            var command = QueryJigInternal.CreateCommandBase(query, CommandType.StoredProcedure, this.connection);
            return command.ExecuteNonQuery();
        }
        /// <summary>
        /// マップなしストアドプロシージャ
        /// </summary>
        /// <returns>更新のあったレコード数</returns>
        public OutputDto StoreProcedureNotMap<OutputDto>(Query query) where OutputDto : new()
        {
            var command = QueryJigInternal.CreateCommandBase(query, CommandType.StoredProcedure, this.connection);
            command.ExecuteNonQuery();

            return QueryJigInternal.MapValue<OutputDto>(command);
        }
        #endregion

        #region SELECT実行
        /// <summary>
        /// SELECT実行
        /// </summary>
        public List<DbDataRecord> ExecuteSelect(Query query)
        {
            var command = QueryJigInternal.CreateCommandBase(query, CommandType.Text, this.connection);
            var reader = command.ExecuteReader();

            return reader.Cast<DbDataRecord>().ToList();
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
            var command = QueryJigInternal.CreateCommandBase(query, CommandType.Text, this.connection);
            return command.ExecuteNonQuery();
        }
        /// <summary>
        /// INSERT, UPDATE, DELETE実行
        /// バッチ実行
        /// </summary>
        /// <param name="batchCount">実行レコード数</param>
        /// <returns></returns>
        public int ExecuteNonQueryBatch(Query query, int batchCount)
        {
            var command = QueryJigInternal.CreateCommandBase(query, CommandType.Text, this.connection);
            command.ArrayBindCount = batchCount;
            return command.ExecuteNonQuery();
        }
        #endregion

        #region for IDisposable
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                if (this.transaction != null)
                    this.transaction.Dispose();
                if (this.connection != null)
                    this.connection.Dispose();
            }
            this.disposed = true;
        }

        #endregion
    }
}
