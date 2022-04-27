using fbognini.i18n.Persistence.Entities;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Localizers
{
    public class EFStringLocalizer: IStringLocalizer
    {
        private readonly Dictionary<string, string> translations;
        private readonly string key;
        private readonly bool createNewRecordWhenDoesNotExists;
        private readonly II18nRepository i18NRepository;

        public EFStringLocalizer(Dictionary<string, string> translations, string key, II18nRepository i18NRepository, bool createNewRecordWhenDoesNotExists = false)
        {
            this.translations = translations;
            this.key = key;
            this.i18NRepository = i18NRepository;
            this.createNewRecordWhenDoesNotExists = createNewRecordWhenDoesNotExists;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var text = GetText(name, out var error);
                return new LocalizedString(name, text, error);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var localizedString = this[name];
                return new LocalizedString(name, string.Format(localizedString.Value, arguments), localizedString.ResourceNotFound);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return translations.Select(x => new LocalizedString(x.Key, x.Value));
        }

        private string GetText(string id, out bool error)
        {

#if NET451
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
#elif NET46
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
#else
            var culture = CultureInfo.CurrentCulture;
#endif

            string computedKey = $"{id}.{culture}";
            string parentComputedKey = $"{id}.{culture.Parent.TwoLetterISOLanguageName}";

            if (translations.TryGetValue(computedKey, out string translation) || translations.TryGetValue(parentComputedKey, out translation))
            {
                error = false;
                return translation;
            }

            error = true;
            if (createNewRecordWhenDoesNotExists)
            {
                var cultures = i18NRepository.AddTranslations(id, key, string.Empty, new Dictionary<string, string>() { [culture.ToString()] = id });
                foreach (var item in cultures)
                {
                    translations.Add($"{id}.{item.LanguageId}", id);
                }

                return id;
            }

            //if (_returnKeyOnlyIfNotFound)
            //{
            //    return key;
            //}

            return key + "." + computedKey;
        }

    }
}
