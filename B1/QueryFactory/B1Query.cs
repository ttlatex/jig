using Jig.QueryControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.QueryFactory
{
    public class B1Query
    {
        public static Query SerachQuerry()
        {
            const string sql = @"

";
            return new Query(sql, null);
        }

        public static Query UpdateQuerry()
        {
            const string sql = @"

";
            return new Query(sql, null);
        }
    }
}
