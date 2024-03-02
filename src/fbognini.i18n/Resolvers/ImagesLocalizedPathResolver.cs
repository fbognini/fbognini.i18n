using AutoMapper;
using fbognini.i18n.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Resolvers
{
    public class ImagesLocalizedPathResolver : IMemberValueResolver<object, object, string, List<LocalizedImage>>
    {
        private readonly II18nRepository localizer;
        public ImagesLocalizedPathResolver(II18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public List<LocalizedImage> Resolve(object source, object destination, string sourceMember, List<LocalizedImage> destMember,
            ResolutionContext context)
        {
            var pictures = new List<LocalizedImage>();

            foreach (var language in localizer.Languages)
            {
                var image = new LocalizedImage()
                {
                    LanguageId = language,
                    PicturePath = sourceMember,
                    PictureUrl = sourceMember == null ? null : Utils.Combine(localizer.BaseUriResource, language, sourceMember)
                };

                pictures.Add(image);
            }

            return pictures;
        }
    }
}
