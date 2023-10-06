using AutoMapper;
using fbognini.i18n.Persistence.Entities;
using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Languages
{
    public class UpdateLanguageCommand : IRequest<LanguageDto>
    {
        public string Id { get; set; } = default!;
        public required string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }

        internal class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, LanguageDto>
        {
            private readonly IMapper mapper;
            private readonly II18nRepository i18NRepository;

            public UpdateLanguageCommandHandler(
                IMapper mapper,
                II18nRepository i18nRepository)
            {
                this.mapper = mapper;
                i18NRepository = i18nRepository;
            }

            public async Task<LanguageDto> Handle(UpdateLanguageCommand command, CancellationToken cancellationToken)
            {
                var language = i18NRepository.UpdateLanguage(command.Id, command.Description, command.IsActive, command.IsDefault);

                return mapper.Map<LanguageDto>(language);
            }
        }
    }
}
