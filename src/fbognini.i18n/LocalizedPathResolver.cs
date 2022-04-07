using AutoMapper;
using System.Collections.Generic;
using System.Threading;

namespace fbognini.i18n
{
    public class ImageNotLocalizedPathResolver : IMemberValueResolver<object, object, string, LocalizedImageDto>
    {
        private readonly Ii18nRepository localizer;
        public ImageNotLocalizedPathResolver(Ii18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public LocalizedImageDto Resolve(object source, object destination, string sourceMember, LocalizedImageDto destMember,
            ResolutionContext context)
        {
            var pictures = new List<LocalizedImageDto>();
            var language = "general";

            return new LocalizedImageDto()
            {
                LanguageId = language,
                PicturePath = sourceMember,
                PictureUrl = sourceMember == null ? null : Combine(localizer.BaseUriResource, language, sourceMember)
            };
        }

        private string Combine(params string[] paths)
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

    public class ImageAllLocalizedPathResolver : IMemberValueResolver<object, object, string, List<LocalizedImageDto>>
    {
        private readonly Ii18nRepository localizer;
        public ImageAllLocalizedPathResolver(Ii18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public List<LocalizedImageDto> Resolve(object source, object destination, string sourceMember, List<LocalizedImageDto> destMember,
            ResolutionContext context)
        {
            var pictures = new List<LocalizedImageDto>();
            //if (sourceMember == null)
            //    return null;

            foreach (var language in localizer.Languages)
            {
                var image = new LocalizedImageDto()
                {
                    LanguageId = language,
                    PicturePath = sourceMember,
                    PictureUrl = sourceMember == null ? null : Combine(localizer.BaseUriResource, language, sourceMember)
                };

                pictures.Add(image);
            }

            return pictures;
        }

        private string Combine(params string[] paths)
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

    public class LocalizedPathResolver : IMemberValueResolver<object, object, string, string>
    {
        private readonly Ii18nRepository localizer;
        public LocalizedPathResolver(Ii18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public string Resolve(object source, object destination, string sourceMember, string destMember,
            ResolutionContext context)
        {
            var language = Thread.CurrentThread.CurrentCulture;
            if (sourceMember == null)
                return null;

            var dest = Combine(localizer.BaseUriResource, language.Name, sourceMember);

            return dest;
        }

        private string Combine(params string[] paths)
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

    public class NotLocalizedPathResolver : IMemberValueResolver<object, object, string, string>
    {
        private readonly Ii18nRepository localizer;
        public NotLocalizedPathResolver(Ii18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public string Resolve(object source, object destination, string sourceMember, string destMember,
            ResolutionContext context)
        {
            if (sourceMember == null)
                return null;

            var dest = Combine(localizer.BaseUriResource, "general", sourceMember);

            return dest;
        }

        private string Combine(params string[] paths)
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
