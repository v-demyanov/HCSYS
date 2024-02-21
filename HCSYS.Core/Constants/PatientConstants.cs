namespace HCSYS.Core.Constants;

public static class PatientConstants
{
    public static IDictionary<string, string> BirthDatePrefixes { get; } = new Dictionary<string, string>
    {
        ["eq"] = "=",
        ["ne"] = "<>", 
        ["gt"] = ">", 
        ["lt"] = "<",
        ["ge"] = ">=",
        ["le"] = "<=", 
    };
}
