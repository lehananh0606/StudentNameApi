namespace StudentNameApi.Helpers
{
    public sealed record ErrorResponse(int StatusCode, string? Message, bool isError, dynamic Errors, DateTime Timestamp);
}
