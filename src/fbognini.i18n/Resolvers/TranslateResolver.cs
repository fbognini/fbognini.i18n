using AutoMapper;
using fbognini.i18n.Localizers;
using Microsoft.Extensions.Localization;
using System.Threading;

namespace fbognini.i18n.Resolvers
{
    public class TranslateResolver : IMemberValueResolver<object, object, string?, string?>
    {
        private readonly IExtendedStringLocalizerFactory stringLocalizerFactory;

        public TranslateResolver(IExtendedStringLocalizerFactory stringLocalizerFactory)
        {
            this.stringLocalizerFactory = stringLocalizerFactory;
        }

        public string? Resolve(object source, object destination, string? sourceMember, string? destMember,
            ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(sourceMember))
                return null;

            var type = source.GetType();

            var localizer = stringLocalizerFactory.CreateWithRawKey(type);
            return localizer[sourceMember];
        }
    }
}
