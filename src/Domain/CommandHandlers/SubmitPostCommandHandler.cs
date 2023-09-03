using MediatR;
using S4Capital.Api.Api.Dtos.Commands;
using S4Capital.Api.Core;
using S4Capital.Api.Domain.Resources;
using S4Capital.Api.Infrastructure.Repositories;

namespace S4Capital.Api.Domain.CommandHandlers;

public class SubmitPostCommandHandler : MediatorHandler, IRequestHandler<SubmitPostCommand>
{
    private readonly IPostRepository _postRepository;

    public SubmitPostCommandHandler(
        INotificationManager notificationManager,
        IHttpContextAccessor context,
        IPostRepository postRepository) : base(notificationManager, context)
    {
        _postRepository = postRepository;
    }

    public async Task Handle(SubmitPostCommand request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var post = await _postRepository.GetApprovedByIdAndAuthorAsync(request.PostId, userId);

        if (post is null)
        {
            Notify(Resource.PostNotFound);
            return;
        }

        post.Publish(userId);
        await _postRepository.UpdatePostAsync(post);
    }
}