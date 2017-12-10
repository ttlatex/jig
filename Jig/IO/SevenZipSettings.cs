using System.Configuration;

namespace Jig.IO
{
    public class SevenZipSettings : ConfigurationSection
    {
        [ConfigurationProperty("SevenZipExePath", IsRequired = true)]
        public string SevenZipExePath
        {
            get { return (string)this["SevenZipExePath"]; }
        }

        [ConfigurationProperty("RetryCounts")]
        public int RetryCounts
        {
            get { return (int)this["RetryCounts"]; }
        }


        [ConfigurationProperty("RetryWaitMiliSeconds")]
        [IntegerValidator(MinValue = 0)]
        public int RetryWaitMiliSeconds
        {
            get { return (int)this["RetryWaitMiliSeconds"]; }
        }


        [ConfigurationProperty("ProcessTimeoutMiliSeconds")]
        [IntegerValidator(MinValue = 0)]
        public int ProcessTimeoutMiliSeconds
        {
            get { return (int)this["ProcessTimeoutMiliSeconds"]; }
        }
    }
}
