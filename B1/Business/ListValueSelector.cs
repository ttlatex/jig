using B1.QueryFactory;
using B1.Dto;
using Jig.QueryControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.Business
{
    public class ListValueSelector
    {
        public static List<SelectedRecord> SelectItems()
        {
            QueryExecuter executer = new QueryExecuter();

            var query = B1Query.SerachQuerry();
            return executer.ExecuteSelect<SelectedRecord>(query);
        }
    }
}
