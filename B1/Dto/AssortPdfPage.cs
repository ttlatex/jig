using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.Dto
{
    public class AssortPdfPage
    {
        public string Title { get; set; }
        public string Page { get; set; }

        public List<AssortPdfPageDetail> Detail { get; set; }
    }

    public class AssortPdfPageDetail
    {
        public string No { get; set; }
        public string Name { get; set; }
    }
}
