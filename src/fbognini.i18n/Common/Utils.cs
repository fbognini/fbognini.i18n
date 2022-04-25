using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Common
{
    internal static class Utils
    {
        internal static string Combine(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
                return null;

            var url = paths[0];
            for (int i = 1; i < paths.Length; i++)
            {
                url = string.Format("{0}/{1}", url.TrimEnd('/'), paths[i].TrimStart('/'));
            }

            return url;
        }
    }
}
