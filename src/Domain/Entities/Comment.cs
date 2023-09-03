using Microsoft.AspNetCore.Identity;
using S4Capital.Api.Api.Dtos.Commands;

namespace S4Capital.Api.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentDate { get; set; }
    public IdentityUser Author { get; set; }
    public string AuthorId { get; set; }
    public Post Post { get; set; }
    public Guid PostId { get; set; }

    private Comment()
    { }

    public Comment(AddCommentCommand command, string authorId)
    {
        Id = Guid.NewGuid();
        Text = command.Comment;
        CommentDate = DateTime.UtcNow;
        PostId = command.PostId;
        AuthorId = authorId;
    }
}
