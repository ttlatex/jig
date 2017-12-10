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
        QueryExecuter executer = new QueryExecuter();

        public List<AssortPdfPage> SelectItems()
        {
            var query = B1Query.SerachQuerry();
            var records = executer.ExecuteSelect<SelectedRecord>(query);

            if (records.Count == 0)
                return ZeroCountList();


            int detailNo = 0;
            return records
                .Buffer(2)
                .Select((x, i) => new AssortPdfPage
                {
                    Title = "名前リスト",
                    Page = (i + 1).ToString(),

                    Detail = x.Select(y => new AssortPdfPageDetail
                    {
                        No = (++detailNo).ToString(),
                        Name = y.FIRST_NAME,
                    }).ToList()
                })
                .ToList();
        }

        /// <summary>
        /// 0件時のリスト内容
        /// </summary>
        public static List<AssortPdfPage> ZeroCountList()
        {
            var pdfValue = new AssortPdfPage()
            {
                Title = "名前リスト",
                Page = "1",
                Detail = new List<AssortPdfPageDetail> { new AssortPdfPageDetail { Name = "---対象なし---" } },
            };

            return new List<AssortPdfPage> { pdfValue };
        }
    }
}
