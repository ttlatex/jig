using System;
using System.Configuration;
using System.IO;

namespace Jig.Configuration
{
    public class FileStringValidator : ConfigurationValidatorBase
    {
        public const string DefalultValue = "<>";

        private bool MustExist;

        public FileStringValidator(bool mustExist)
        {
            MustExist = mustExist;
        }

        public override bool CanValidate(Type type)
        {
            return type == typeof(string);
        }

        public override void Validate(object value)
        {
            var path = (string)value;

            if (path == DefalultValue)
                return;

            if (!File.Exists(path))
            {
                throw new ConfigurationErrorsException("ファイルが存在しません:" + path);
            }
        }
    }
}
