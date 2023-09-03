using MediatR;
using S4Capital.Api.Api.Dtos.Commands;
using S4Capital.Api.Core;
using S4Capital.Api.Domain.Resources;
using S4Capital.Api.Infrastructure.Repositories;

namespace S4Capital.Api.Domain.CommandHandlers;

public class EditPostCommandHandler : MediatorHandler, IRequestHandler<EditPostCommand>
{
    private readonly IPostRepository _postRepository;

    public EditPostCommandHandler(
        INotificationManager notificationManager, 
        IHttpContextAccessor context, 
        IPostRepository postRepository) : base(notificationManager, context)
    {
        _postRepository = postRepository;
    }

    public async Task Handle(EditPostCommand request, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var post = await _postRepository.GetPendingByIdAndAuthorAsync(request.PostId, userId);

        if (post is null)
        {
            Notify(Resource.PostNotFound);
            return;
        }

        post.Update(request, userId);
        await _postRepository.UpdatePostAsync(post);
    }
}