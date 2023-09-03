﻿using MediatR;
using S4Capital.Api.Api.Dtos.Responses;

namespace S4Capital.Api.Api.Dtos.Queries;

public class PublishedPostsQuery : IRequest<IEnumerable<PublishedPostResponse>>
{
}
