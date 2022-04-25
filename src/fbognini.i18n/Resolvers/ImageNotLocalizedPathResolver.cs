using AutoMapper;
using fbognini.i18n.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Resolvers
{

    public class ImageNotLocalizedPathResolver : IMemberValueResolver<object, object, string, LocalizedImage>
    {
        private readonly II18nRepository localizer;
        public ImageNotLocalizedPathResolver(II18nRepository localizer)
        {
            this.localizer = localizer;
        }

        public LocalizedImage Resolve(object source, object destination, string sourceMember, LocalizedImage destMember,
            ResolutionContext context)
        {
            return new LocalizedImage()
            {
                LanguageId = i18nConstants.GENERAL,
                PicturePath = sourceMember,
                PictureUrl = sourceMember == null ? null : Utils.Combine(localizer.BaseUriResource, i18nConstants.GENERAL, sourceMember)
            };
        }
    }
}
