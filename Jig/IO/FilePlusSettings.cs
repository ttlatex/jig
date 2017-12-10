using System.Configuration;

namespace Jig.IO
{
    public class FilePlusSettings : ConfigurationSection
    {
        [ConfigurationProperty("FileRetryCounts")]
        [IntegerValidator(MinValue = 0)]
        public int FileRetryCounts
        {
            get { return (int)this["FileRetryCounts"]; }
        }


        [ConfigurationProperty("FileRetryWaitMiliSeconds")]
        [IntegerValidator(MinValue = 0)]
        public int FileRetryWaitMiliSeconds
        {
            get { return (int)this["FileRetryWaitMiliSeconds"]; }
        }
    }
}
