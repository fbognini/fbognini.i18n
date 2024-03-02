using AutoMapper;
using Azure;
using fbognini.Core.Domain.Query.Pagination;
using fbognini.i18n.Dashboard.Helpers;
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
            private readonly II18nRepository i18NRepository;

            public GetPaginatedLanguagesQueryHandler(
                II18nRepository i18nRepository)
            {
                i18NRepository = i18nRepository;
            }

            public async Task<PaginationResponse<LanguageDto>> Handle(GetPaginatedLanguagesQuery query, CancellationToken cancellationToken)
            {
                var criteria = new LanguageSelectCriteria();
                criteria.LoadFullSearchQuery(query);

                var response = i18NRepository.GetPaginatedLanguages(criteria);

                return new PaginationResponse<LanguageDto>()
                {
                    Items = response.Items.Select(x => x.ToDto()).ToList(),
                    Pagination = response.Pagination,
                };
            }
        }
    }
}
