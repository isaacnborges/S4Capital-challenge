using MediatR;

namespace S4Capital.Api.Api.Dtos.Commands;

public class EditPostCommand : IRequest
{
    public Guid PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}
