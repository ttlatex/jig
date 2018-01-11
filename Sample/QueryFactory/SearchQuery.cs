using Jig.QueryControl;
using A1.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace A1.QueryFactory
{
    public class SearchQuery
    {
        public Query IDQuery(InputValue iValue)
        {
            const string ProcedureName = "PKG_A1.SELECT_NAME";
            var oracleParams = new List<OracleParameter>();

            oracleParams.Add(QueryJig.ToOracleInParam("IN_ID", iValue.IN_ID));
            oracleParams.Add(QueryJig.ToOracleInParam("IN_PHONE_NUMBER", iValue.IN_ID));

            //// 式木
            //oracleParams.Add(QueryJig.ToOracleInParam(() => iValue.IN_ID));
            //oracleParams.Add(QueryJig.ToOracleInParam(() => iValue.IN_PHONE_NUMBER));

            //// OracleParameter
            //oracleParams.Add(new OracleParameter("IN_ID", OracleDbType.Varchar2, iValue.IN_ID, ParameterDirection.Input));
            //oracleParams.Add(new OracleParameter("IN_ID", OracleDbType.Int32, iValue.IN_PHONE_NUMBER, ParameterDirection.Input));

            // 引数OUT 可変型はサイズを設定する必要がある
            oracleParams.Add(QueryJig.ToOracleOutParameter<string>("OUT_NAME", 200));

            //oracleParams.Add(new OracleParameter("OUT_NAME", OracleDbType.Varchar2, 200, null, ParameterDirection.Output));

            return new Query(ProcedureName, oracleParams);
        }
    }
}
