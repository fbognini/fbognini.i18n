using AutoMapper;
using fbognini.i18n.Persistence.Entities;
using MediatR;

namespace fbognini.i18n.Dashboard.Handlers.Texts
{
    public class DeleteTextCommand : IRequest
    {
        public required string TextId { get; set; }
        public required string ResourceId { get; set; }

        internal class DeleteTextCommandHandler : IRequestHandler<DeleteTextCommand>
        {
            private readonly IMapper mapper;
            private readonly II18nRepository i18NRepository;

            public DeleteTextCommandHandler(
                IMapper mapper,
                II18nRepository i18nRepository)
            {
                this.mapper = mapper;
                i18NRepository = i18nRepository;
            }

            public async Task Handle(DeleteTextCommand command, CancellationToken cancellationToken)
            {
                i18NRepository.DeleteTranslations(command.TextId, command.ResourceId);
            }
        }
    }
}
