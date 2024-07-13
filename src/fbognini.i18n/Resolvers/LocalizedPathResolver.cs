using AutoMapper;
using System.Collections.Generic;
using System.Threading;

namespace fbognini.i18n.Resolvers
{
    public class LocalizedPathResolver : IMemberValueResolver<object, object, string?, string?>
    {
        private readonly II18nRepository localizer;
        public LocalizedPathResolver(II18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public string? Resolve(object source, object destination, string? sourceMember, string? destMember,
            ResolutionContext context)
        {
            var language = Thread.CurrentThread.CurrentCulture;
            if (sourceMember == null)
                return null;

            var dest = Combine(localizer.BaseUriResource, language.Name, sourceMember);

            return dest;
        }

        private string? Combine(params string[] paths)
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
