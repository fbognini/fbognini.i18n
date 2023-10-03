using fbognini.Core.Data.Pagination;
using fbognini.Core.Data;
using fbognini.i18n.Persistence.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace fbognini.i18n
{
    public interface II18nRepository
    {
        string BaseUriResource { get; }
        //string Translate(string language, int source);
        List<string> Languages { get; }


        IEnumerable<Language> GetLanguages();
        PaginationResponse<Language> GetPaginatedLanguages(SelectCriteria<Language> criteria);
        void AddLanguage(Language language);
        void AddLanguageWithTranslations(Language language);
        Language UpdateLanguage(string id, string description, bool isActive, bool isDefault);

        IEnumerable<Translation> AddTranslations(string textId, string resourceId, string description, Dictionary<string, string> translations);
        void DeleteTranslations(string textId, string resourceId);
        IEnumerable<Translation> GetTranslations(string languageId, string textId, string resourceId, DateTime? since = null);
        Translation GetTranslation(string languageId, string textId, string resourceId);
        PaginationResponse<Translation> GetPaginatedTranslations(SelectCriteria<Translation> criteria);
        PaginationResponse<Text> GetPaginatedTexts(SelectCriteria<Text> criteria);

        void UpdateTranslation(Translation translation);
        void UpdateTranslations(List<Translation> translations);

        void ImportExcel(string path, bool all, bool deletenotmatched);
        byte[] ExportExcel();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="translations"></param>
        /// <param name="all">if true, import all rows, otherwise import only updated rows</param>
        /// <param name="deletenotmatched">if true, delete translation when not found</param>
        void ImportTranslations(IEnumerable<Translation> translations, bool all, bool deletenotmatched);

        internal void DetachAllEntities();
    }
}
