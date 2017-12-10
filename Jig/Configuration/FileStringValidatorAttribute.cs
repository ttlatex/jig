using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Configuration
{
    [AttributeUsageAttribute(AttributeTargets.Property)]
    public class FileStringValidatorAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                return new FileStringValidator();
            }
        }
    }
}
