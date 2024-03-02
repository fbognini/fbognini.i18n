using fbognini.i18n.Dashboard.Helpers;
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
            private readonly II18nRepository i18NRepository;

            public CreateTextCommandHandler(
                II18nRepository i18nRepository)
            {
                i18NRepository = i18nRepository;
            }

            public async Task<TextDto> Handle(CreateTextCommand command, CancellationToken cancellationToken)
            {
                var languages = i18NRepository.GetLanguages();

                var translations = languages.ToDictionary(keySelector: language => language.Id, elementSelector: _ => command.TextId);
                var t = i18NRepository.AddTranslations(command.TextId, command.ResourceId, command.Description, translations);

                return t.First().Text.ToDto();
            }
        }
    }
}
