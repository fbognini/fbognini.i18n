﻿using fbognini.i18n.Dashboard.Helpers;
using fbognini.i18n.Persistence.Entities;
using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Languages
{
    public class CreateLanguageCommand : IRequest<LanguageDto>
    {
        public required string Id { get; set; }
        public required string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }

        internal class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, LanguageDto>
        {
            private readonly II18nRepository i18NRepository;

            public CreateLanguageCommandHandler(
                II18nRepository i18nRepository)
            {
                i18NRepository = i18nRepository;
            }

            public async Task<LanguageDto> Handle(CreateLanguageCommand command, CancellationToken cancellationToken)
            {
                var language = new Language()
                {
                    Id = command.Id,
                    Description = command.Description,
                    IsActive = command.IsActive,
                    IsDefault = command.IsDefault
                };

                i18NRepository.AddLanguageWithTranslations(language);

                return language.ToDto();
            }
        }
    }
}
