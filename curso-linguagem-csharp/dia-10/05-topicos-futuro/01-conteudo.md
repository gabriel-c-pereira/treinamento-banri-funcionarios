# 05 - Tópicos para o Futuro

## Source Generators (C# 9+)

### Criando um Source Generator
```csharp
[Generator]
public class AutoNotifyGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Registrar syntax receiver
        context.RegisterForSyntaxNotifications(() => new AutoNotifySyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not AutoNotifySyntaxReceiver receiver)
            return;

        foreach (var classDeclaration in receiver.Classes)
        {
            var source = GenerateAutoNotifyClass(classDeclaration);
            context.AddSource($"{classDeclaration.Identifier.Text}.g.cs", source);
        }
    }

    private string GenerateAutoNotifyClass(ClassDeclarationSyntax classDeclaration)
    {
        var namespaceName = GetNamespace(classDeclaration);
        var className = classDeclaration.Identifier.Text;

        return $@"
// Auto-generated code
namespace {namespaceName}
{{
    public partial class {className}
    {{
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {{
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }}
    }}
}}
";
    }
}
```

### Usando o Source Generator
```csharp
// Em tempo de compilação, o source generator cria:
public partial class Person : INotifyPropertyChanged
{
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    // Código gerado automaticamente
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

## Minimal APIs (ASP.NET Core 6+)

### API Simples e Moderna
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Map endpoints
app.MapGet("/api/users", async (AppDbContext db) =>
    await db.Users.ToListAsync());

app.MapGet("/api/users/{id}", async (int id, AppDbContext db) =>
    await db.Users.FindAsync(id) is User user ? Results.Ok(user) : Results.NotFound());

app.MapPost("/api/users", async (User user, AppDbContext db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/api/users/{user.Id}", user);
});

app.MapPut("/api/users/{id}", async (int id, User updatedUser, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    user.Name = updatedUser.Name;
    user.Email = updatedUser.Email;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/users/{id}", async (int id, AppDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
```

## .NET MAUI (Multi-platform App UI)

### Aplicação Cross-Platform
```csharp
// App.xaml.cs
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}

// MainPage.xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyApp.MainPage">

    <ScrollView>
        <VerticalStackLayout Spacing="25" Padding="30">
            <Label Text="Hello, World!"
                   FontSize="32"
                   HorizontalOptions="Center" />

            <Button x:Name="CounterBtn"
                    Text="Click me"
                    FontSize="18"
                    HorizontalOptions="Center"
                    Clicked="OnCounterClicked" />
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>

// MainPage.xaml.cs
public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;
        CounterBtn.Text = $"Clicked {count} time{(count == 1 ? "" : "s")}";
    }
}
```

## Native AOT (Ahead-of-Time Compilation)

### Configuração para AOT
```xml
<!-- MyApp.csproj -->
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <!-- Habilitar Native AOT -->
    <PublishAot>true</PublishAot>
    <!-- Otimizar para tamanho -->
    <OptimizationPreference>Size</OptimizationPreference>
    <!-- Remover assemblies não utilizados -->
    <TrimMode>full</TrimMode>
  </PropertyGroup>

</Project>
```

### Limitações do AOT
```csharp
// ❌ Reflection não funciona em AOT
public static object CreateInstance(Type type)
{
    return Activator.CreateInstance(type); // Runtime error em AOT
}

// ✅ Alternativa: usar generics ou source generators
public static T CreateInstance<T>() where T : new()
{
    return new T();
}
```

## Cloud-Native Development

### Azure Functions com .NET
```csharp
public static class HttpExample
{
    [FunctionName("HttpExample")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        name = name ?? data?.name;

        return name != null
            ? (ActionResult)new OkObjectResult($"Hello, {name}")
            : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
    }
}
```

### Microserviços com Dapr
```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add Dapr
builder.Services.AddDaprClient();

// Add services
builder.Services.AddSingleton<IOrderService, OrderService>();

var app = builder.Build();

// Map routes
app.MapPost("/orders", async (Order order, IOrderService orderService, DaprClient daprClient) =>
{
    var orderId = await orderService.CreateOrderAsync(order);

    // Publish event
    await daprClient.PublishEventAsync("order-pubsub", "order-created", order);

    return Results.Created($"/orders/{orderId}", order);
});

app.MapGet("/orders/{id}", async (string id, IOrderService orderService) =>
{
    var order = await orderService.GetOrderAsync(id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
});

app.Run();
```

## Performance e Otimização

### Span<T> e Memory<T>
```csharp
public static class StringProcessor
{
    public static bool TryParseNumber(ReadOnlySpan<char> input, out int result)
    {
        result = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (!char.IsDigit(input[i]))
                return false;

            result = result * 10 + (input[i] - '0');
        }

        return true;
    }

    public static void ProcessLargeData(Memory<byte> data)
    {
        // Processar dados sem alocação adicional
        var span = data.Span;

        for (int i = 0; i < span.Length; i++)
        {
            span[i] = (byte)(span[i] ^ 0xFF); // Exemplo de processamento
        }
    }
}
```

### Records Struct
```csharp
// Record struct para performance
public readonly record struct Point(double X, double Y)
{
    public double Distance => Math.Sqrt(X * X + Y * Y);
}

// Uso eficiente
var points = new Point[] { new(1, 2), new(3, 4), new(5, 6) };
var totalDistance = points.Sum(p => p.Distance);
```

## Tendências Futuras

### AI e Machine Learning
- **ML.NET**: Machine learning integrado ao .NET
- **Azure Cognitive Services**: APIs de IA na nuvem
- **ONNX Runtime**: Execução de modelos de ML

### WebAssembly
```csharp
// Blazor WebAssembly
@page "/counter"

<h1>Counter</h1>
<p>Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

### IoT e Edge Computing
```csharp
// .NET IoT Libraries
using System.Device.Gpio;

public class LedController
{
    private readonly GpioController _controller;
    private readonly int _pinNumber = 18;

    public LedController()
    {
        _controller = new GpioController();
        _controller.OpenPin(_pinNumber, PinMode.Output);
    }

    public void TurnOn() => _controller.Write(_pinNumber, PinValue.High);
    public void TurnOff() => _controller.Write(_pinNumber, PinValue.Low);
}
```
