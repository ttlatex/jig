using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1.Settings
{
    public class B1Settings : ConfigurationSection
    {
        [ConfigurationProperty("OutputFolder", IsRequired = true)]
        public string OutputFolder
        {
            get { return (string)this["OutputFolder"]; }
        }

        [ConfigurationProperty("TemplatePath", IsRequired = true)]
        public string TemplatePath
        {
            get { return (string)this["TemplatePath"]; }
        }
    }
}
