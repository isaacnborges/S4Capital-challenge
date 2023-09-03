using MediatR;

namespace S4Capital.Api.Api.Dtos.Commands;

public class RejectPostCommand : IRequest
{
    public Guid PostId { get; set; }
    public string RejectionComment { get; set; }
}
