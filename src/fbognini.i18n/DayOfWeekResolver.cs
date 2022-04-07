using AutoMapper;
using System.Threading;

namespace fbognini.i18n
{
    public class DayOfWeekResolver : IMemberValueResolver<object, object, int, string>
    {
        private readonly Ii18nRepository localizer;
        public DayOfWeekResolver(Ii18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public string Resolve(object source, object destination, int sourceMember, string destMember,
            ResolutionContext context)
        {
            var language = Thread.CurrentThread.CurrentCulture;
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat.DayNames[sourceMember % 7].ToString();
        }
    }
}
