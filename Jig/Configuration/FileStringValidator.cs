using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Configuration
{
    public class FileStringValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(string);
        }

        public override void Validate(object value)
        {
            var path = (string)value;

            if (!File.Exists(path))
            {
                throw new ConfigurationErrorsException("ファイルが存在しません:" + path);
            }
        }
    }
}
