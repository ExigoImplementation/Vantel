using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class DynamicHelper
    {
        public static dynamic AddProperty(dynamic source, string key, object value)
        {
            var target = source as IDictionary<String, object>;

            target[key] = value;

            return target;
        }
    }
}
