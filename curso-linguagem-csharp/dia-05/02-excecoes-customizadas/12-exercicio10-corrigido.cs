// Result Pattern
public record Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }

    public static Result<T> Ok(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Fail(string error) => new() { IsSuccess = false, Error = error };
}

// Comparação
public class UserService
{
    // Com exceções
    public User GetUserById(int id)
    {
        var user = _repository.Find(id);
        if (user == null)
            throw new UserNotFoundException(id);
        
        if (!user.IsActive)
            throw new UserInactiveException(id);

        return user;
    }

    // Com Result Pattern
    public Result<User> GetUserByIdSafe(int id)
    {
        var user = _repository.Find(id);
        if (user == null)
            return Result<User>.Fail($"User {id} not found");
        
        if (!user.IsActive)
            return Result<User>.Fail($"User {id} is inactive");

        return Result<User>.Ok(user);
    }
}

// Uso
var result = service.GetUserByIdSafe(123);
if (result.IsSuccess)
{
    Console.WriteLine($"User: {result.Value.Name}");
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}