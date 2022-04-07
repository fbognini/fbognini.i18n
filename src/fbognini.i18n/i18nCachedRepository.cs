using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace fbognini.i18n
{
    public class i18nCachedRepository: Ii18nRepository
    {
        private Ii18nRepository repository;
        private IMemoryCache cache;
        private MemoryCacheEntryOptions cacheOptions;

        // alternatively use IDistributedCache if you use redis and multiple services
        public i18nCachedRepository(Ii18nRepository repository, IMemoryCache cache)
        {
            this.repository = repository;
            this.cache = cache;

            // 1 day caching
            cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(relative: TimeSpan.FromHours(6));
        }

        public string BaseUriResource
        {
            get
            {
                var key = "BaseUriResource";
                var value = cache.Get<string>(key);
                if (value != null)
                    return value;

                value = repository.BaseUriResource;
                if (value != null)
                    cache.Set(key, value, cacheOptions);

                return value;
            }
        }

        public string Translate(string language, int source)
        {
            string key = $"{language}_{source}";
            var value = cache.Get<string>(key);
            if (value != null)
                return value;

            value = repository.Translate(language, source);
            if (value != null)
                cache.Set(key, value, cacheOptions);

            return value;
        }

        public List<string> Languages
        {
            get
            {
                var key = "Languages";
                var value = cache.Get<List<string>>(key);
                if (value != null)
                    return value;

                value = repository.Languages;
                if (value != null)
                    cache.Set(key, value, cacheOptions);

                return value;
            }
        }

    }
}
