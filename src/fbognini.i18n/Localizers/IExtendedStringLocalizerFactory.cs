using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Localizers
{
    public interface IExtendedStringLocalizerFactory : IStringLocalizerFactory
    { 
        void ResetCache();
        void ResetCache(Type resourceSource);
        void ResetCache(string baseName, string location);
        string NormalizeKey(string key);

        string GetI18NKey(Type resourceSource);


        IStringLocalizer CreateWithRawKey(Type resourceSource);
        IStringLocalizer CreateWithRawKey(string rawKey);
    }
}
