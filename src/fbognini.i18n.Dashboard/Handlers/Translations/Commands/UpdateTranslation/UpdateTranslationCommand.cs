using AutoMapper;
using fbognini.i18n.Persistence.Entities;
using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Translations
{
    public class UpdateTranslationCommand : IRequest<TranslationDto>
    {
        public required string LanguageId { get; set; }
        public required string TextId { get; set; }
        public required string ResourceId { get; set; }
        public required string Destination { get; set; }

        internal class UpdateTranslationCommandHandler : IRequestHandler<UpdateTranslationCommand, TranslationDto>
        {
            private readonly IMapper mapper;
            private readonly II18nRepository i18NRepository;

            public UpdateTranslationCommandHandler(
                IMapper mapper,
                II18nRepository i18nRepository)
            {
                this.mapper = mapper;
                i18NRepository = i18nRepository;
            }

            public async Task<TranslationDto> Handle(UpdateTranslationCommand command, CancellationToken cancellationToken)
            {
                var translation = new Translation()
                {
                    LanguageId = command.LanguageId,
                    TextId = command.TextId,
                    ResourceId = command.ResourceId,
                    Destination = command.Destination,
                };
                i18NRepository.UpdateTranslation(translation);

                return mapper.Map<TranslationDto>(i18NRepository.GetTranslation(command.LanguageId, command.TextId, command.ResourceId));
            }
        }
    }
}
