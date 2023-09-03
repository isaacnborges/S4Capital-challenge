using Microsoft.AspNetCore.Identity;
using S4Capital.Api.Api.Dtos.Commands;
using System.ComponentModel;

namespace S4Capital.Api.Domain.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public PostStatus Status { get; set; }
    public string RejectionComment { get; set; }

    public IdentityUser Author { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }

    public IdentityUser Editor { get; set; }
    public string EditedBy { get; set; }
    public DateTime? EditedDate { get; set; }

    public IdentityUser Submitter { get; set; }
    public string SubmittedBy { get; set; }
    public DateTime? SubmittedDate { get; set; }

    public IdentityUser Approver { get; set; }
    public string ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }

    public IdentityUser Rejecter { get; set; }
    public string RejectBy { get; set; }
    public DateTime? RejectDate { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    private Post()
    { }

    public Post(CreatePostCommand command, string authorId)
    {
        Id = Guid.NewGuid();
        Title = command.Title;
        Content = command.Content;
        Status = PostStatus.Pending;
        CreatedBy = authorId;
        CreatedDate = DateTime.UtcNow;
    }

    public void Update(EditPostCommand command, string userId)
    {
        Title = command.Title;
        Content = command.Content;
        EditedBy = userId;
        EditedDate = DateTime.UtcNow;
    }

    public void Approve(string authorId)
    {
        Status = PostStatus.Approved;
        ApprovedBy = authorId;
        ApprovedDate = DateTime.UtcNow;
    }

    public void Reject(RejectPostCommand command, string authorId)
    {
        Status = PostStatus.Rejected;
        RejectBy = authorId;
        RejectDate = DateTime.UtcNow;

        if (string.IsNullOrWhiteSpace(command.RejectionComment) is false)
        {
            RejectionComment = command.RejectionComment;
        }
    }

    public void Publish(string authorId)
    {
        Status = PostStatus.Published;
        SubmittedBy = authorId;
        SubmittedDate = DateTime.UtcNow;
    }
}

public enum PostStatus
{
    [Description("Pending")]
    Pending,

    [Description("Approved")]
    Approved,

    [Description("Published")]
    Published,

    [Description("Rejected")]
    Rejected
}
