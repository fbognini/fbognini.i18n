using fbognini.i18n.Persistence;
using fbognini.i18n.Persistence.Entities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
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
        private readonly I18nSettings.LocalizerSettings localizerSettings;

        public EFStringLocalizerFactory(II18nRepository i18NRepository, IOptions<I18nSettings> i18noptions)
        {
            this.i18NRepository = i18NRepository;
            this.localizerSettings = i18noptions.Value.Localizer;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return Create(GetI18NKey(resourceSource));
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return Create(CompositeKey(baseName, location));
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

        public IStringLocalizer CreateWithRawKey(Type resourceSource)
        {
            return CreateWithRawKey(GetI18NKey(resourceSource));
        }

        public IStringLocalizer CreateWithRawKey(string rawKey)
        {
            if (localizers.ContainsKey(rawKey))
            {
                return localizers[rawKey];
            }

            var localizer = new EFStringLocalizer(GetResources(rawKey), rawKey, i18NRepository, localizerSettings.CreateNewRecordWhenDoesNotExists);
            return localizers.GetOrAdd(rawKey, localizer);
        }

        public void ResetCache()
        {
            localizers.Clear();
            i18NRepository.DetachAllEntities();
        }

        public void ResetCache(Type resourceSource)
        {
            ResetCache(GetI18NKey(resourceSource));
        }

        public void ResetCache(string baseName, string location)
        {
            ResetCache(CompositeKey(baseName, location));
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
            if (!string.IsNullOrWhiteSpace(localizerSettings.OverrideResourceId))
            {
                return localizerSettings.OverrideResourceId;
            }

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

            return key;
        }

        private string CompositeKey(string baseName, string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return baseName;

            // it's ok for views, be careful for other situations
            var name = string.Join('.', baseName.Split('.').TakeLast(2));
            return name;
        }

        public string GetI18NKey(Type resourceSource)
        {
            var attribute = resourceSource.GetCustomAttributes(typeof(I18NKeyAttribute), false).SingleOrDefault();
            if (attribute == null)
            {
                return GetRecursiveResourceName(resourceSource);
            }

            return ((I18NKeyAttribute)attribute).Key;
        }


        private static string GetRecursiveResourceName(Type resourceSource)
        {
            var builder = GetRecursiveResourceName(resourceSource, new StringBuilder());
            if (builder.Length > 0)
            {
                builder = builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString();
        }

        private static StringBuilder GetRecursiveResourceName(Type resourceSource, StringBuilder name)
        {
            if (resourceSource == null)
            {
                return name;
            }

            return GetRecursiveResourceName(resourceSource.DeclaringType, name.Insert(0, '.').Insert(0, resourceSource.Name));
        }
    }
}
