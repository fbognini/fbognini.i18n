using AutoMapper;
using Azure;
using fbognini.Core.Data.Pagination;
using fbognini.WebFramework.FullSearch;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fbognini.i18n.Dashboard.Handlers.Languages
{
    public class GetPaginatedLanguagesQuery : IRequest<PaginationResponse<LanguageDto>>, IFullSearchQuery
    {
        public FullSearch FullSearch { get; set; } = new();

        internal class GetPaginatedLanguagesQueryHandler : IRequestHandler<GetPaginatedLanguagesQuery, PaginationResponse<LanguageDto>>
        {
            private readonly IMapper mapper;
            private readonly II18nRepository i18NRepository;

            public GetPaginatedLanguagesQueryHandler(
                IMapper mapper,
                II18nRepository i18nRepository)
            {
                this.mapper = mapper;
                i18NRepository = i18nRepository;
            }

            public async Task<PaginationResponse<LanguageDto>> Handle(GetPaginatedLanguagesQuery query, CancellationToken cancellationToken)
            {
                var criteria = new LanguageSelectCriteria();
                criteria.LoadFullSearchQuery(query);

                var response = i18NRepository.GetPaginatedLanguages(criteria);

                return new PaginationResponse<LanguageDto>()
                {
                    Items = mapper.Map<List<LanguageDto>>(response.Items),
                    Pagination = response.Pagination,
                };
            }
        }
    }
}
