using fbognini.i18n.Persistence;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace fbognini.i18n
{
    internal class I18nCachedRepository: I18nRepository, II18nRepository
    {
        private IMemoryCache cache;
        private MemoryCacheEntryOptions cacheOptions;

        public I18nCachedRepository(I18nContext context, IMemoryCache cache)
            : base(context)
        {
            this.cache = cache;

            // 6 hours caching
            cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(relative: TimeSpan.FromHours(6));
        }

        public new string BaseUriResource
        {
            get
            {
                var key = "BaseUriResource";
                var value = cache.Get<string>(key);
                if (value != null)
                    return value;

                value = base.BaseUriResource;
                if (value != null)
                    cache.Set(key, value, cacheOptions);

                return value;
            }
        }

        public new string Translate(string language, int source)
        {
            string key = $"{language}_{source}";
            var value = cache.Get<string>(key);
            if (value != null)
                return value;

            value = base.Translate(language, source);
            if (value != null)
                cache.Set(key, value, cacheOptions);

            return value;
        }

        public new List<string> Languages
        {
            get
            {
                var key = "Languages";
                var value = cache.Get<List<string>>(key);
                if (value != null)
                    return value;

                value = base.Languages;
                if (value != null)
                    cache.Set(key, value, cacheOptions);

                return value;
            }
        }

    }
}
