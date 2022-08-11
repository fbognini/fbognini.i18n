using AutoMapper;
using fbognini.i18n.Localizers;
using Microsoft.Extensions.Localization;
using System.Threading;

namespace fbognini.i18n.Resolvers
{
    public class TranslateResolver : IMemberValueResolver<object, object, int, string>, IMemberValueResolver<object, object, int?, string>, IMemberValueResolver<object, object, string, string>
    {
        private readonly II18nRepository localizer;
        public TranslateResolver(II18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public string Resolve(object source, object destination, int sourceMember, string destMember,
            ResolutionContext context)
        {
            if (sourceMember == -1)
                return null;

            var language = Thread.CurrentThread.CurrentCulture;
            var type = source.GetType();

            var value = localizer.Translate(language.Name, sourceMember);
            if (value != null)
                return value;

            return sourceMember.ToString();
        }

        public string Resolve(object source, object destination, int? sourceMember, string destMember,
            ResolutionContext context)
        {
            if (!sourceMember.HasValue)
                return null;

            return Resolve(source, destination, sourceMember.Value, destMember, context);
        }

        public string Resolve(object source, object destination, string sourceMember, string destMember,
            ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(sourceMember))
                return null;

            var language = Thread.CurrentThread.CurrentCulture;
            var type = source.GetType();

            var value = localizer.Translate(language.Name, sourceMember);
            if (value != null)
                return value;

            return sourceMember.ToString();
        }
    }
}
