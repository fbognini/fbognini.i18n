using fbognini.Core.Domain.Query.Pagination;
using fbognini.i18n.Dashboard.Helpers;
using fbognini.WebFramework.FullSearch;
using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Translations
{
    public class GetPaginatedTranslationsQuery : IRequest<PaginationResponse<TranslationDto>>, IFullSearchQuery
    {
        public string? LanguageId { get; set; }
        public string? TextId { get; set; }
        public string? ResourceId { get; set; }
        public bool OnlyNotTranslated { get; set; } = false;

        public FullSearch FullSearch { get; set; } = new();

        internal class GetPaginatedTranslationsQueryHandler : IRequestHandler<GetPaginatedTranslationsQuery, PaginationResponse<TranslationDto>>
        {
            private readonly II18nRepository i18NRepository;

            public GetPaginatedTranslationsQueryHandler(
                II18nRepository i18nRepository)
            {
                i18NRepository = i18nRepository;
            }

            public async Task<PaginationResponse<TranslationDto>> Handle(GetPaginatedTranslationsQuery query, CancellationToken cancellationToken)
            {
                var criteria = new TranslationSelectCriteria()
                {
                    LanguageId = query.LanguageId,
                    TextId = query.TextId,
                    ResourceId = query.ResourceId,
                    NotTranslated = query.OnlyNotTranslated ? true : null,
                };
                criteria.LoadFullSearchQuery(query);
                var response = i18NRepository.GetPaginatedTranslations(criteria);

                return new PaginationResponse<TranslationDto>()
                {
                    Items = response.Items.Select(x => x.ToDto()).ToList(),
                    Pagination = response.Pagination,
                };
            }
        }
    }
}
