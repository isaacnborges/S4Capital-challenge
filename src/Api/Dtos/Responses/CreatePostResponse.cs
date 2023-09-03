namespace S4Capital.Api.Api.Dtos.Responses;

public struct CreatePostResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}
