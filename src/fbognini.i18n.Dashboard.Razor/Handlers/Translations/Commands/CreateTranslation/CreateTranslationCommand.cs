using AutoMapper;
using fbognini.i18n.Persistence.Entities;
using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Translations
{
    public class CreateTranslationCommand : IRequest<TranslationDto>
    {
        public required string Id { get; set; }
        public required string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }

        internal class CreateTranslationCommandHandler : IRequestHandler<CreateTranslationCommand, TranslationDto>
        {
            private readonly IMapper mapper;
            private readonly II18nRepository i18NRepository;

            public CreateTranslationCommandHandler(
                IMapper mapper,
                II18nRepository i18nRepository)
            {
                this.mapper = mapper;
                i18NRepository = i18nRepository;
            }

            public async Task<TranslationDto> Handle(CreateTranslationCommand command, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
