using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S4Capital.Api.Api.Dtos.Commands;
using S4Capital.Api.Api.Dtos.Queries;
using S4Capital.Api.Core;
using S4Capital.Api.Domain.ValueObjects;

namespace S4Capital.Api.Api.Controllers;

[Authorize]
[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly NotificationHandler _notificationHandler;

    public PostsController(IMediator mediator, INotificationHandler<Notification> notificationHandler)
    {
        _mediator = mediator;
        _notificationHandler = (NotificationHandler)notificationHandler;
    }

    /// <summary>
    /// Get all published posts
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [ApiExplorerSettings(GroupName = "Posts - Public")]
    [Authorize(Roles = $"{UserRole.Public}, {UserRole.Writer}, {UserRole.Editor}")]
    public async Task<ActionResult> GetAllPublishedPosts()
    {
        var query = new PublishedPostsQuery();
        var resust = await _mediator.Send(query);

        return Ok(resust);
    }

    /// <summary>
    /// Add comment to a published post
    /// </summary>
    /// <returns></returns>
    [HttpPost("{id}/comment")]
    [ApiExplorerSettings(GroupName = "Posts - Public")]
    [Authorize(Roles = $"{UserRole.Public}, {UserRole.Writer}, {UserRole.Editor}")]
    public async Task<ActionResult> AddCommentPublishedPost([FromRoute] Guid id, [FromBody] AddCommentCommand command)
    {
        command.PostId = id;
        await _mediator.Send(command);

        return CustomNoContent();
    }

    /// <summary>
    /// Get post by writer
    /// </summary>
    /// <returns></returns>
    [HttpGet("writer")]
    [ApiExplorerSettings(GroupName = "Posts - Writer")]
    [Authorize(Roles = UserRole.Writer)]
    public async Task<ActionResult> GetPostByWriter()
    {
        var query = new PostsByWriterQuery();
        var posts = await _mediator.Send(query);

        return Ok(posts);
    }

    /// <summary>
    /// Create a post
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost()]
    [ApiExplorerSettings(GroupName = "Posts - Writer")]
    [Authorize(Roles = UserRole.Writer)]
    public async Task<ActionResult> CreatePost([FromBody] CreatePostCommand command)
    {
        var post = await _mediator.Send(command);

        return CreatedAtAction("GetPostByWriter", new { writerId = post.CreatedBy }, post);
    }

    /// <summary>
    /// Edit a post
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ApiExplorerSettings(GroupName = "Posts - Writer")]
    [Authorize(Roles = UserRole.Writer)]
    public async Task<ActionResult> EditPost([FromRoute] Guid id, [FromBody] EditPostCommand command)
    {
        command.PostId = id;

        await _mediator.Send(command);

        return CustomNoContent();
    }

    /// <summary>
    /// Submit(publish) a post with approval status
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id}/submit")]
    [ApiExplorerSettings(GroupName = "Posts - Writer")]
    [Authorize(Roles = UserRole.Writer)]
    public async Task<ActionResult> SubmitPost([FromRoute] Guid id)
    {
        var command = new SubmitPostCommand(id);
        await _mediator.Send(command);

        return CustomNoContent();
    }

    /// <summary>
    /// Get pending posts
    /// </summary>
    /// <returns></returns>
    [HttpGet("pending-posts")]
    [ApiExplorerSettings(GroupName = "Posts - Editor")]
    [Authorize(Roles = UserRole.Editor)]
    public async Task<ActionResult> GetPendingPosts()
    {
        var query = new PendingPostsQuery();
        var resust = await _mediator.Send(query);

        return Ok(resust);
    }

    /// <summary>
    /// Approve a post
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id}/approve")]
    [ApiExplorerSettings(GroupName = "Posts - Editor")]
    [Authorize(Roles = UserRole.Editor)]
    public async Task<ActionResult> ApprovePost([FromRoute] Guid id)
    {
        var command = new ApprovePostCommand(id);
        await _mediator.Send(command);

        return CustomNoContent();
    }

    /// <summary>
    /// Reject a post
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPatch("{id}/reject")]
    [ApiExplorerSettings(GroupName = "Posts - Editor")]
    [Authorize(Roles = UserRole.Editor)]
    public async Task<ActionResult> RejectPost([FromRoute] Guid id, RejectPostCommand command)
    {
        command.PostId = id;
        await _mediator.Send(command);

        return CustomNoContent();
    }

    private ActionResult CustomNoContent()
    {
        if (_notificationHandler.HasNotifications())
        {
            return BadRequest(new
            {
                type = CustomExceptionMiddleware.CustomExceptionMiddleware.ValidationErrors,
                errors = _notificationHandler.GetNotifications().Select(n => n.Value)
            });
        }

        return NoContent();
    }
}
