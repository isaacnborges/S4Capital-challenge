using MediatR;
using S4Capital.Api.Api.Dtos.Responses;

namespace S4Capital.Api.Api.Dtos.Commands;

public class CreatePostCommand : IRequest<CreatePostResponse>
{
    public string Title { get; set; }
    public string Content { get; set; }
}

