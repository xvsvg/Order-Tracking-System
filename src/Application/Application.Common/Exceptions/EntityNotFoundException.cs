namespace Application.Common.Exceptions;

public class EntityNotFoundException : ApplicationException
{
    public EntityNotFoundException(string message) : base(message)
    {
    }

    public static EntityNotFoundException For<T>(Guid id)
    {
        return new EntityNotFoundException($"Entity of type {typeof(T).Name} with id \'{id}\' was not found");
    }
    
    public static EntityNotFoundException For<T>(string attribute)
    {
        return new EntityNotFoundException($"Entity of type {typeof(T).Name} with attribute \'{attribute}\' was not found");
    }
}