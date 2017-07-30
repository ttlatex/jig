using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jig.Settings
{
    public class ConfigParser
    {
        /// <summary>
        /// Configクラスのインスタンスを作成します
        /// </summary>
        public static Config Instance<Config>() where Config : new()
        {
            var config = new Config();

            return config;
        }
    }
}
