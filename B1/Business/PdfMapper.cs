using B1.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.Business
{
    public class PdfMapper
    {
        public static List<AssortPdfPage> MapToPdf(List<SelectedRecord> records)
        {
            // 通し行数
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
    }
}
