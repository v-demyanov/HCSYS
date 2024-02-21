namespace HCSYS.Core.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) wasn't found.")
    {
    }
}
