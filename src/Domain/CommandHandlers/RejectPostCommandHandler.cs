using MediatR;
using S4Capital.Api.Api.Dtos.Commands;
using S4Capital.Api.Core;
using S4Capital.Api.Domain.Resources;
using S4Capital.Api.Infrastructure.Repositories;

namespace S4Capital.Api.Domain.CommandHandlers;

public class RejectPostCommandHandler : MediatorHandler, IRequestHandler<RejectPostCommand>
{
    private readonly IPostRepository _postRepository;

    public RejectPostCommandHandler(
        INotificationManager notificationManager,
        IHttpContextAccessor context,
        IPostRepository postRepository) : base(notificationManager, context)
    {
        _postRepository = postRepository;
    }

    public async Task Handle(RejectPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetPendingByIdAsync(request.PostId);

        if (post is null)
        {
            Notify(Resource.PostNotFound);
            return;
        }

        post.Reject(request, GetUserId());
        await _postRepository.UpdatePostAsync(post);
    }
}