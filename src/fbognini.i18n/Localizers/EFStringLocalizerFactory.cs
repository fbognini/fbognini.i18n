using fbognini.i18n.Persistence;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Localizers
{
    internal class EFStringLocalizerFactory : IStringLocalizerFactory, IExtendedStringLocalizerFactory
    {
        private const string Global = "global";

        private static readonly ConcurrentDictionary<string, IStringLocalizer> localizers = new ConcurrentDictionary<string, IStringLocalizer>();

        private readonly I18nContext context;
        private readonly LocalizerSettings localizerSettings;
        private readonly II18nRepository i18NRepository;

        public EFStringLocalizerFactory(I18nContext context, LocalizerSettings localizerSettings, II18nRepository i18NRepository)
        {
            this.context = context;
            this.localizerSettings = localizerSettings;
            this.i18NRepository = i18NRepository;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return Create(resourceSource.Name);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return Create(baseName + location);
        }

        private IStringLocalizer Create(string key)
        {
            key = NormalizeKey(key);

            if (localizers.ContainsKey(key))
            {
                return localizers[key];
            }

            var localizer = new EFStringLocalizer(GetResources(key), key, i18NRepository, localizerSettings.CreateNewRecordWhenDoesNotExists);
            return localizers.GetOrAdd(key, localizer);
        }

        public void ResetCache()
        {
            localizers.Clear();
            lock (context)
            {
                context.DetachAllEntities();
            }
        }

        public void ResetCache(Type resourceSource)
        {
            ResetCache(resourceSource.Name);
        }

        public void ResetCache(string baseName, string location)
        {
            ResetCache(baseName + location);
        }

        private void ResetCache(string key)
        {
            key = NormalizeKey(key);

            localizers.TryRemove(key, out _);
            lock (context)
            {
                context.DetachAllEntities();
            }
        }

        private Dictionary<string, string> GetResources(string resourceKey)
        {
            lock (context)
            {
                return context.Translations
                    .Where(data => data.Text.ResourceId == resourceKey)
                    .ToDictionary(kvp => (kvp.TextId + "." + kvp.LanguageId), kvp => kvp.Destination, StringComparer.OrdinalIgnoreCase);
            }
        }

        public string NormalizeKey(string key)
        {
            foreach (var suffix in localizerSettings.RemoveSuffixs.Where(s => key.EndsWith(s)))
            {
                key = key[..^suffix.Length];
            }

            foreach (var prefix in localizerSettings.RemovePrefixs.Where(s => key.StartsWith(s)))
            {
                key = key[prefix.Length..];
            }

            if (!string.IsNullOrWhiteSpace(localizerSettings.BaseResourceId))
            {
                key = $"{localizerSettings.BaseResourceId}.{key}";
            }

            if (!string.IsNullOrWhiteSpace(localizerSettings.OverrideResourceId))
            {
                key = localizerSettings.OverrideResourceId;
            }

            return key;
        }
    }
}
