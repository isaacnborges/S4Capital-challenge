using MediatR;
using S4Capital.Api.Api.Dtos.Commands;
using S4Capital.Api.Api.Dtos.Responses;
using S4Capital.Api.Core;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Domain.Mappings;
using S4Capital.Api.Infrastructure.Repositories;

namespace S4Capital.Api.Domain.CommandHandlers;

public class CreatePostCommandHandler : MediatorHandler, IRequestHandler<CreatePostCommand, CreatePostResponse>
{
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(
        INotificationManager notificationManager,
        IHttpContextAccessor context,
        IPostRepository postRepository) : base(notificationManager, context)
    {
        _postRepository = postRepository;
    }

    public async Task<CreatePostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Post(request, GetUserId());

        await _postRepository.CreatePostAsync(post);

        var postResponse = post.ToCreateResponse();

        return postResponse;
    }
}