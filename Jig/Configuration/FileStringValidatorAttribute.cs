using System;
using System.Configuration;

namespace Jig.Configuration
{
    [AttributeUsageAttribute(AttributeTargets.Property)]
    public class FileStringValidatorAttribute : ConfigurationValidatorAttribute
    {
        public bool MustExist { get; set; }

        public override ConfigurationValidatorBase ValidatorInstance
        {
            get
            {
                return new FileStringValidator(MustExist);
            }
        }
    }
}
