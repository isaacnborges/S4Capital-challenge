using MediatR;
using S4Capital.Api.Api.Dtos.Queries;
using S4Capital.Api.Api.Dtos.Responses;
using S4Capital.Api.Domain.Mappings;
using S4Capital.Api.Infrastructure.Repositories;

namespace S4Capital.Api.Domain.QueryHandlers;

public class PublishedPostsQueryHandler : IRequestHandler<PublishedPostsQuery, IEnumerable<PublishedPostResponse>>
{
    private readonly IPostRepository _postRepository;

    public PublishedPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IEnumerable<PublishedPostResponse>> Handle(PublishedPostsQuery request, CancellationToken cancellationToken)
    {
        var publishedPosts = await _postRepository.GetPublishedPosts();
        var postsResponse = publishedPosts.Select(x => x.ToPublishedPostResponse()).ToList();

        return postsResponse;
    }
}