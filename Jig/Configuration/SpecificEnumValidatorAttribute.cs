using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Configuration
{
    [AttributeUsageAttribute(AttributeTargets.Property)]
    public class SpecificEnumValidatorAttribute : ConfigurationValidatorAttribute
    {
        int[] AllowedNumbers { get; set; }

        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                return new SpecificEnumValidator(AllowedNumbers);
            }
        }
    }
}
