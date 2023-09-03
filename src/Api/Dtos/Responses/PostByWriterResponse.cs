namespace S4Capital.Api.Api.Dtos.Responses;

public struct PostByWriterResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public string RejectionComment { get; set; }
}
