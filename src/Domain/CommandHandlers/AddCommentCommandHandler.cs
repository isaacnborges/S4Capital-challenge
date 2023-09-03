using MediatR;
using S4Capital.Api.Api.Dtos.Commands;
using S4Capital.Api.Core;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Domain.Resources;
using S4Capital.Api.Infrastructure.Repositories;

namespace S4Capital.Api.Domain.CommandHandlers;

public class AddCommentCommandHandler : MediatorHandler, IRequestHandler<AddCommentCommand>
{
    private readonly IPostRepository _postRepository;

    public AddCommentCommandHandler(
        INotificationManager notificationManager,
        IHttpContextAccessor context,
        IPostRepository postRepository) : base(notificationManager, context)
    {
        _postRepository = postRepository;
    }

    public async Task Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPublishedByIdAsync(request.PostId);

        if (post is null)
        {
            Notify(Resource.PostNotFound);
            return;
        }

        var comment = new Comment(request, GetUserId());
        await _postRepository.AddCommentAsync(comment);
    }
}
