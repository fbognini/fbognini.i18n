using fbognini.i18n.Persistence;
using fbognini.i18n.Persistence.Entities;
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

        private readonly II18nRepository i18NRepository;
        private readonly LocalizerSettings localizerSettings;

        public EFStringLocalizerFactory(II18nRepository i18NRepository, LocalizerSettings localizerSettings)
        {
            this.i18NRepository = i18NRepository;
            this.localizerSettings = localizerSettings;
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
            i18NRepository.DetachAllEntities();
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
            i18NRepository.DetachAllEntities();
        }

        private Dictionary<string, string> GetResources(string resourceId)
        {
            return i18NRepository.GetTranslations(null, null, resourceId)
                    .ToDictionary(kvp => (kvp.TextId + "." + kvp.LanguageId), kvp => kvp.Destination, StringComparer.OrdinalIgnoreCase);
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
