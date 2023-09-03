using S4Capital.Api.Api.Dtos.Responses;
using S4Capital.Api.Domain.Entities;
using S4Capital.Api.Extensions;

namespace S4Capital.Api.Domain.Mappings;

public static class DomainToResponseMapping
{
    public static CreatePostResponse ToCreateResponse(this Post post) => new()
    {
        Id = post.Id,
        Content = post.Content,
        Title = post.Title,
        Status = post.Status.GetDescription(),
        CreatedBy = post.CreatedBy,
        CreatedDate = post.CreatedDate
    };

    public static PostByWriterResponse ToPostByWriterResponse(this Post post) => new()
    {
        Id = post.Id,
        Content = post.Content,
        Title = post.Title,
        Status = post.Status.GetDescription(),
        RejectionComment = post.RejectionComment
    };

    public static PendingPostResponse ToPendingPostResponse(this Post post) => new()
    {
        Id = post.Id,
        Content = post.Content,
        Title = post.Title,
        Status = post.Status.GetDescription(),
        Author = post.Author.UserName
    };

    public static PublishedPostResponse ToPublishedPostResponse(this Post post) => new()
    {
        Id = post.Id,
        Content = post.Content,
        Title = post.Title,
        Status = post.Status.GetDescription(),
        Author = post.Author.UserName,
        Comments = post.Comments.Select(comment => new CommentResponse
        {
            Id = comment.Id,
            Text = comment.Text,
            CommentDate = comment.CommentDate,
            Author = comment.Author.UserName
        }).ToList()
    };
}
