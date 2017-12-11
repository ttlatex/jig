using Jig.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Pdf
{
    public class PdfPrinterSettings : ConfigurationSection
    {
        [ConfigurationProperty("AdobeReaderPath", DefaultValue = FileStringValidator.DefalultValue, IsRequired = true)]
        [FileStringValidator]
        public string AdobeReaderPath
        {
            get { return (string)this["AdobeReaderPath"]; }
        }

        [ConfigurationProperty("DefaultPrinterName")]
        public string DefaultPrinterName
        {
            get { return (string)this["DefaultPrinterName"]; }
        }

        [ConfigurationProperty("FileRetryWaitMiliSeconds", DefaultValue = 60)]
        [IntegerValidator(MinValue = 0)]
        public int FileRetryWaitMiliSeconds
        {
            get { return (int)this["FileRetryWaitMiliSeconds"]; }
        }
    }
}
