using Jig.Common;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jig.QueryControl
{
    internal class QueryJigInternal
    {
        /// <summary>
        /// OutputValueにSQLからの取得結果をマップする
        /// </summary>
        internal static List<OutputValue> MapValue<OutputValue>(List<DbDataRecord> records) where OutputValue : new()
        {
            var outputValueList = new List<OutputValue>();

            var properties = typeof(OutputValue)
                .GetRuntimeProperties()
                .Where(x => x.GetCustomAttribute<IgnoreMemberAttribute>(true) == null);
            foreach (var drecord in records)
            {
                var outputValue = new OutputValue();

                foreach (var property in properties)
                {
                    var dbValue = drecord[property.Name];
                    if (DBNull.Value.Equals(dbValue)) continue;
                    property.SetValue(outputValue, dbValue);
                }

                outputValueList.Add(outputValue);
            }

            return outputValueList;
        }

        /// <summary>
        /// OutputValueにSQLからの取得結果をマップする
        /// </summary>
        internal static OutputValue MapValue<OutputValue>(OracleCommand command) where OutputValue : new()
        {

            var properties = typeof(OutputValue)
                .GetRuntimeProperties()
                .Where(x => x.GetCustomAttribute<IgnoreMemberAttribute>(true) == null);

            var outputValue = new OutputValue();

            foreach (var property in properties)
            {
                var dbValue = command.Parameters[property.Name].Value;

                if (dbValue == null || dbValue.ToString() == "null")
                {
                    property.SetValue(outputValue, null);
                    continue;
                }

                property.SetValue(outputValue, dbValue);
            }

            return outputValue;
        }

        internal static OracleCommand CreateCommandBase(Query query, CommandType commandType, OracleConnection connetction)
        {
            // インスタンス設定
            var command = connetction.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = query.CommandText;
            command.BindByName = true; // バインド変数の順序を意識しない

            if (query.Parameters == null) return command;

            foreach (var oracleParam in query.Parameters)
                command.Parameters.Add(oracleParam);

            return command;
        }

        internal static object OracleTypeConvert(object dbValue)
        {
            if (dbValue is OracleBoolean) return ((OracleBoolean)dbValue).Value;
            if (dbValue is OracleDecimal) return ((OracleDecimal)dbValue).Value;
            if (dbValue is OracleString) return ((OracleString)dbValue).Value;
            if (dbValue is OracleDate) return ((OracleDate)dbValue).Value;

            if (dbValue is OracleBoolean[]) return ((OracleBoolean[])dbValue).Select(x => x.Value).ToList();
            if (dbValue is OracleDecimal[]) return ((OracleDecimal[])dbValue).Select(x => x.Value).ToList();
            if (dbValue is OracleString[]) return ((OracleString[])dbValue).Select(x => x.Value).ToList();
            if (dbValue is OracleDate[]) return ((OracleDate[])dbValue).Select(x => x.Value).ToList();

            throw new InvalidCastException("型変換が行えない型です " + dbValue.GetType().ToString());
        }
    }
}
