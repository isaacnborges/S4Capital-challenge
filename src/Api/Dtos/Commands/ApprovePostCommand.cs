using MediatR;

namespace S4Capital.Api.Api.Dtos.Commands;

public class ApprovePostCommand : IRequest
{
    public Guid PostId { get; set; }

    public ApprovePostCommand(Guid postId)
    {
        PostId = postId;
    }
}
