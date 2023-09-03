namespace S4Capital.Api.Api.Dtos.Responses;

public struct PublishedPostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public string Author { get; set; }
    public IEnumerable<CommentResponse> Comments { get; set; }
}
