using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jig.QueryControl
{
    // OracleParameterの設定はodp.netのマナルを参照すること
    // Sizeプロパティは配列かどうかで意味が変わる

    public class QueryJig
    {
        #region 入力パラメータ

        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Input)
        /// </summary>
        public static OracleParameter ToOracleInParam<T>(string paramName, T paramValue)
        {
            var type = GetOracleDbType<T>();
            return new OracleParameter(paramName, type, (Object)paramValue, ParameterDirection.Input);
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


        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Input)
        /// PLSQLAssociativeArray
        /// </summary>
        public static OracleParameter ToOracleInListParam<T>(string paramName, T[] paramValue)
        {
            var type = GetOracleDbType<T>();

            var param = new OracleParameter(paramName, type, (object)paramValue, ParameterDirection.Input);
            param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            if (type == OracleDbType.Varchar2)
                param.ArrayBindSize = paramValue.Select(x => x.ToString().Length).ToArray(); // 可変長データはデータ長の設定が必要
            return param;
        }

        #endregion

        #region 入力パラメータ式木

        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Input)
        /// </summary>
        public static OracleParameter ToOracleInParam<T>(Expression<Func<T>> e)
        {
            var type = GetOracleDbType<T>();
            var member = (MemberExpression)e.Body;
            return new OracleParameter(member.Member.Name, type, (object)e.Compile()(), ParameterDirection.Input);
        }

        // バッチ処理は( ･`ω･´)

        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Input)
        /// PLSQLAssociativeArray
        /// </summary>
        public static OracleParameter ToOracleInListParam<T>(Expression<Func<T[]>> e)
        {
            var array = e.Compile()();
            var type = GetOracleDbType<T>();

            var member = (MemberExpression)e.Body;
            var param = new OracleParameter(member.Member.Name, type, (object)array, ParameterDirection.Input);
            param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            if (type == OracleDbType.Varchar2)
                param.ArrayBindSize = array.Select(x => x.ToString().Length).ToArray(); // 可変長データはデータ長の設定が必要
            return param;
        }

        #endregion

        #region 出力パメータ

        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Output)
        /// </summary>
        /// <param name="byteNum">要素のバイト数（可変長文字列の場合指定する）</param>
        public static OracleParameter ToOracleOutParameter<T>(string paramName, int byteNum)
        {
            var type = GetOracleDbType<T>();
            return new OracleParameter(paramName, type, byteNum, null, ParameterDirection.Output);
        }

        /// <summary>
        /// OracleParameterの設定(ParameterDirection.Output)
        /// PLSQLAssociativeArray
        /// </summary>
        /// <param name="byteNum">要素のバイト数（可変長文字列の場合指定する）</param>
        public static OracleParameter ToOracleOutListParameter<T>(string paramName, int paramCount, int byteNum)
        {
            var type = GetOracleDbType<T>();
            var param = new OracleParameter(paramName, type, paramCount, null, ParameterDirection.Output);
            param.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
            param.ArrayBindSize = Enumerable.Repeat(element: byteNum, count: paramCount).ToArray();

            return param;
        }

        #endregion

        private static OracleDbType GetOracleDbType<T>()
        {
            Type t = typeof(T);
            if (t == typeof(bool)) return OracleDbType.Boolean;
            if (t == typeof(short)) return OracleDbType.Int16;
            if (t == typeof(int)) return OracleDbType.Int32;
            if (t == typeof(long)) return OracleDbType.Decimal;
            if (t == typeof(float)) return OracleDbType.Double;
            if (t == typeof(double)) return OracleDbType.Double;
            if (t == typeof(decimal)) return OracleDbType.Decimal;
            if (t == typeof(string)) return OracleDbType.Varchar2;
            if (t == typeof(DateTime)) return OracleDbType.Date;

            throw new InvalidCastException("型変換が行えない型です " + t.ToString());
        }
    }
}
