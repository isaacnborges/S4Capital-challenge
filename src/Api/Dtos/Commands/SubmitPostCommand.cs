using MediatR;

namespace S4Capital.Api.Api.Dtos.Commands;

public class SubmitPostCommand : IRequest
{
    public Guid PostId { get; set; }

    public SubmitPostCommand(Guid postId)
    {
        PostId = postId;
    }
}
