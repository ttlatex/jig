using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.Settings
{
    public class AppSettings
    {
        public string OutputFolder { get; set; }
        public string TemplatePath { get; set; }

        public AppSettings()
        {
            this.OutputFolder = ConfigurationManager.AppSettings[nameof(OutputFolder)];
            this.TemplatePath = ConfigurationManager.AppSettings[nameof(TemplatePath)];
        }

        public void ThrowArgumetError()
        {
            if (String.IsNullOrEmpty(this.OutputFolder))
                throw new ArgumentNullException(nameof(OutputFolder));

            if (String.IsNullOrEmpty(this.TemplatePath))
                throw new ArgumentNullException(nameof(TemplatePath));
            if (!File.Exists(this.TemplatePath))
                throw new FileNotFoundException(nameof(TemplatePath));
        }
    }
}
