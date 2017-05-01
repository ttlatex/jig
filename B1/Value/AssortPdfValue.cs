using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.Value
{
    public class AssortPdfValue
    {
        public string Title { get; set; }
        public string Page { get; set; }

        public List<AssortDetailPdfValue> Detail { get; set; }
    }
}
