using Jig.QueryControl;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using B1.Dto;

namespace B1.QueryFactory
{
    public class B1Query
    {
        public static List<SelectedRecord> SerachQuerry(DbConnection connection)
        {
            const string sql = @"
SELECT
  FIRST_NAME
FROM
    EMPLOYEES
Where
    EMPLOYEE_ID <= 10
";
            return connection.Query<SelectedRecord>(sql).ToList();
        }

        public static Query UpdateQuerry()
        {
            const string sql = @"

";
            return new Query(sql, null);
        }
    }
}
