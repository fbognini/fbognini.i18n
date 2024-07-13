using AutoMapper;
using fbognini.i18n.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Resolvers
{
    public class NotLocalizedPathResolver : IMemberValueResolver<object, object, string?, string?>
    {
        private readonly II18nRepository localizer;
        public NotLocalizedPathResolver(II18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public string? Resolve(object source, object destination, string? sourceMember, string? destMember,
            ResolutionContext context)
        {
            if (sourceMember == null)
                return null;

            var dest = Utils.Combine(localizer.BaseUriResource, i18nConstants.GENERAL, sourceMember);

            return dest;
        }
    }
}
