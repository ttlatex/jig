using Jig.QueryControl;
using A1.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A1.QueryFactory;

namespace A1.Business
{
    public class SerchLogic
    {
        private QueryExecuter executer = new QueryExecuter();

        public string SearchName(string id)
        {
            var iValue = new InputValue() { IN_ID = id };

            var query = new SearchQuery().IDQuery(iValue);
            var output = executer.StoreProcedure<OutputValue>(query);

            return (output.OUT_NAME == null)
                ? "検索結果は0件です"
                : output.OUT_NAME;
        }
    }
}
