namespace HCSYS.Api.Models;

public record ErrorResponse
{
    public int StatusCode { get; init; }

    required public string Title { get; init; }
}
