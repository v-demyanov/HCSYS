namespace HCSYS.Core.Exceptions;

public class UnprocessableEntityException : Exception
{
    private const string MessageText = "Validation failed.";

    public UnprocessableEntityException(string fieldName, string error)
    {
        Errors = new Dictionary<string, string[]>
        {
            [fieldName] = new string[] { error },
        };
    }

    public UnprocessableEntityException(IDictionary<string, string[]> errors)
        : base(MessageText)
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
}
