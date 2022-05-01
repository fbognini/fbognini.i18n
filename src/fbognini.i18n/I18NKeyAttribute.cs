using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n
{

    [AttributeUsage(AttributeTargets.Class)]
    public class I18NKeyAttribute : Attribute
    {
        public string Key;

        public I18NKeyAttribute(string key)
        {
            Key = key;
        }
    }
}
