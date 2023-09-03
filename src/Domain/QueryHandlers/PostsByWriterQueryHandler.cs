using MediatR;
using S4Capital.Api.Api.Dtos.Queries;
using S4Capital.Api.Api.Dtos.Responses;
using S4Capital.Api.Core;
using S4Capital.Api.Domain.Mappings;
using S4Capital.Api.Infrastructure.Repositories;

namespace S4Capital.Api.Domain.QueryHandlers;

public class PostsByWriterQueryHandler : MediatorHandler, IRequestHandler<PostsByWriterQuery, IEnumerable<PostByWriterResponse>>
{
    private readonly IPostRepository _postRepository;

    public PostsByWriterQueryHandler(
        INotificationManager notificationManager,
        IHttpContextAccessor context,
        IPostRepository postRepository) : base(notificationManager, context)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PostByWriterResponse>> Handle(PostsByWriterQuery request, CancellationToken cancellationToken)
    {
        var postsByWriter = await _postRepository.GetPostsByAuthorAsync(GetUserId());
        var postsResponse = postsByWriter.Select(x => x.ToPostByWriterResponse()).ToList();

        return postsResponse;
    }
}