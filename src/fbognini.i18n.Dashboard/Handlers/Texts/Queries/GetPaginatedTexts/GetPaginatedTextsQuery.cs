using fbognini.Core.Domain.Query.Pagination;
using fbognini.i18n.Dashboard.Helpers;
using fbognini.WebFramework.FullSearch;
using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Texts
{
    public class GetPaginatedTextsQuery : IRequest<PaginationResponse<TextDto>>, IFullSearchQuery
    {
        public string? TextId { get; set; }
        public string? ResourceId { get; set; }

        public FullSearch FullSearch { get; set; } = new();

        internal class GetPaginatedTextsQueryHandler : IRequestHandler<GetPaginatedTextsQuery, PaginationResponse<TextDto>>
        {
            private readonly II18nRepository i18NRepository;

            public GetPaginatedTextsQueryHandler(
                II18nRepository i18nRepository)
            {
                i18NRepository = i18nRepository;
            }

            public async Task<PaginationResponse<TextDto>> Handle(GetPaginatedTextsQuery query, CancellationToken cancellationToken)
            {
                var criteria = new TextSelectCriteria()
                {
                    TextId = query.TextId,
                    ResourceId = query.ResourceId,
                };
                var response = i18NRepository.GetPaginatedTexts(criteria);

                return new PaginationResponse<TextDto>()
                {
                    Items = response.Items.Select(x => x.ToDto()).ToList(),
                    Pagination = response.Pagination,
                };
            }
        }
    }
}
