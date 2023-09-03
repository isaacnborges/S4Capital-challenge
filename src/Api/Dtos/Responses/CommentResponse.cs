namespace S4Capital.Api.Api.Dtos.Responses;

public struct CommentResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CommentDate { get; set; }
    public string Author { get; set; }
}
