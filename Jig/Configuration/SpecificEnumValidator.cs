using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Configuration
{
    public class SpecificEnumValidator : ConfigurationValidatorBase
    {
        private int[] AllowedNumbers;

        public SpecificEnumValidator(int[] allowedNumbers)
        {
            AllowedNumbers = allowedNumbers;
        }

        public override bool CanValidate(Type type)
        {
            return type.IsEnum;
        }

        public override void Validate(object value)
        {
            var data = (int)value;

            if (!AllowedNumbers.Any(x => x == data))
            {
                throw new ConfigurationErrorsException("次の値のいずれかを設定してください:" + string.Join(",", AllowedNumbers));
            }
        }
    }
}
