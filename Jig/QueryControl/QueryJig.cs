using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jig.QueryControl
{
    public class QueryJig
    {
        #region 入力パラメータ
        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Input)
        /// </summary>
        public static OracleParameter ToOracleInParam<T>(string paramName, T paramValue)
        {
            var type = GetOracleDbType<T>();
            return new OracleParameter(paramName, type,(Object)paramValue, ParameterDirection.Input);
        }

        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Input)
        /// </summary>
        public static OracleParameter ToOracleInParam<T>(Expression<Func<T>> e)
        {
            var type = GetOracleDbType<T>();
            var member = (MemberExpression)e.Body;
            return new OracleParameter(member.Member.Name, type, (object)e.Compile()(), ParameterDirection.Input);
        }

        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Input)
        /// バッチ処理用
        /// </summary>
        public static OracleParameter ToOracleInParamBatch<T>(string paramName, T[] paramValue)
        {
            var type = GetOracleDbType<T>();
            return new OracleParameter(paramName, type, (Object)paramValue, ParameterDirection.Input);
        }

        #endregion


        private static OracleDbType GetOracleDbType<T>()
        {
            Type t = typeof(T);
            if (t == typeof(bool)) return OracleDbType.Boolean;
            if (t == typeof(short)) return OracleDbType.Boolean;
            if (t == typeof(int)) return OracleDbType.Boolean;
            if (t == typeof(long)) return OracleDbType.Boolean;
            if (t == typeof(float)) return OracleDbType.Boolean;
            if (t == typeof(decimal)) return OracleDbType.Boolean;
            if (t == typeof(string)) return OracleDbType.Boolean;
            if (t == typeof(DateTime)) return OracleDbType.Boolean;

            throw new InvalidCastException("型変換が行えない型です " + t.ToString());
        }
    }
}
