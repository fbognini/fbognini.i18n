using AutoMapper;
using fbognini.Core.Data.Pagination;
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
            private readonly IMapper mapper;
            private readonly II18nRepository i18NRepository;

            public GetPaginatedTranslationsQueryHandler(
                IMapper mapper,
                II18nRepository i18nRepository)
            {
                this.mapper = mapper;
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
                    Items = mapper.Map<List<TranslationDto>>(response.Items),
                    Pagination = response.Pagination,
                };
            }
        }
    }
}
