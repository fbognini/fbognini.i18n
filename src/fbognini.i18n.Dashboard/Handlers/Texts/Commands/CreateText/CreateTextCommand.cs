using AutoMapper;
using fbognini.i18n.Persistence.Entities;
using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Texts
{
    public class CreateTextCommand : IRequest<TextDto>
    {
        public required string TextId { get; set; }
        public required string ResourceId { get; set; }
        public string? Description { get; set; }

        internal class CreateTextCommandHandler : IRequestHandler<CreateTextCommand, TextDto>
        {
            private readonly IMapper mapper;
            private readonly II18nRepository i18NRepository;

            public CreateTextCommandHandler(
                IMapper mapper,
                II18nRepository i18nRepository)
            {
                this.mapper = mapper;
                i18NRepository = i18nRepository;
            }

            public async Task<TextDto> Handle(CreateTextCommand command, CancellationToken cancellationToken)
            {
                var languages = i18NRepository.GetLanguages();

                var translations = languages.ToDictionary(keySelector: language => language.Id, elementSelector: _ => command.TextId);
                var t = i18NRepository.AddTranslations(command.TextId, command.ResourceId, command.Description, translations);
                return mapper.Map<TextDto>(t.First().Text);
            }
        }
    }
}
