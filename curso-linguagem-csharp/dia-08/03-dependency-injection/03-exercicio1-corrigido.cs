// Interfaces
public interface ITransientService
{
    Guid GetId();
}

public interface IScopedService
{
    Guid GetId();
}

public interface ISingletonService
{
    Guid GetId();
    DateTime GetCreationTime();
}

// Implementações
public class TransientService : ITransientService
{
    private readonly Guid _id = Guid.NewGuid();

    public Guid GetId() => _id;
}

public class ScopedService : IScopedService
{
    private readonly Guid _id = Guid.NewGuid();

    public Guid GetId() => _id;
}

public class SingletonService : ISingletonService
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly DateTime _creationTime = DateTime.UtcNow;

    public Guid GetId() => _id;
    public DateTime GetCreationTime() => _creationTime;
}

// Configuração no Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configurar lifetimes
builder.Services.AddTransient<ITransientService, TransientService>();
builder.Services.AddScoped<IScopedService, ScopedService>();
builder.Services.AddSingleton<ISingletonService, SingletonService>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();