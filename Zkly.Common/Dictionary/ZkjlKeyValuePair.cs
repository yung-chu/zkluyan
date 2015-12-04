using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.Common.Dictionary
{
    public class ZkjlKeyValuePair
    {
        public ZkjlKeyValuePair(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public int Key { get; set; }

        public string Value { get; set; }
    }
}
