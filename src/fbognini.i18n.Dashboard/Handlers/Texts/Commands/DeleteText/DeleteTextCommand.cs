using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Texts
{
    public class DeleteTextCommand : IRequest
    {
        public required string TextId { get; set; }
        public required string ResourceId { get; set; }

        internal class DeleteTextCommandHandler : IRequestHandler<DeleteTextCommand>
        {
            private readonly II18nRepository i18NRepository;

            public DeleteTextCommandHandler(
                II18nRepository i18nRepository)
            {
                i18NRepository = i18nRepository;
            }

            public async Task Handle(DeleteTextCommand command, CancellationToken cancellationToken)
            {
                i18NRepository.DeleteTranslations(command.TextId, command.ResourceId);
            }
        }
    }
}
