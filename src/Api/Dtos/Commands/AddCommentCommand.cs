using MediatR;

namespace S4Capital.Api.Api.Dtos.Commands;

public class AddCommentCommand : IRequest
{
    public Guid PostId { get; set; }
    public string Comment { get; set; }
}