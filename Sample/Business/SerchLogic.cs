using Jig.QueryControl;
using A1.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A1.Business
{
    public class SerchLogic
    {
        private QueryExecuter executer = new QueryExecuter();

        public string SearchName(string id, int phoneNumber)
        {
            var iValue = new InputValue() { IN_ID = id, IN_PHONE_NUMBER = phoneNumber };
            return "";
        }
    }
}
