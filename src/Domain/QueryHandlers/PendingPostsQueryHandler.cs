using MediatR;
using S4Capital.Api.Api.Dtos.Queries;
using S4Capital.Api.Api.Dtos.Responses;
using S4Capital.Api.Domain.Mappings;
using S4Capital.Api.Infrastructure.Repositories;

namespace S4Capital.Api.Domain.QueryHandlers;

public class PendingPostsQueryHandler : IRequestHandler<PendingPostsQuery, IEnumerable<PendingPostResponse>>
{
    private readonly IPostRepository _postRepository;

    public PendingPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PendingPostResponse>> Handle(PendingPostsQuery request, CancellationToken cancellationToken)
    {
        var pendingPosts = await _postRepository.GetPendingPosts();
        var postsResponse = pendingPosts.Select(x => x.ToPendingPostResponse()).ToList();

        return postsResponse;
    }
}