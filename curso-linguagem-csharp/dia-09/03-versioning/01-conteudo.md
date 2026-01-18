# Versioning de APIs

## Estratégias de Versioning

### 1. URI Versioning
```
GET /api/v1/produtos
GET /api/v2/produtos
```

### 2. Query String Versioning
```
GET /api/produtos?api-version=1.0
GET /api/produtos?api-version=2.0
```

### 3. Header Versioning
```
GET /api/produtos
Headers: api-version: 1.0
```

### 4. Media Type Versioning
```
GET /api/produtos
Accept: application/vnd.minhaapi.v1+json
```

## Implementação no ASP.NET Core

### API Versioning Package
```csharp
// Program.cs
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Controller
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ProdutosController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public IActionResult GetV1() => Ok("Versão 1.0");

    [HttpGet]
    [MapToApiVersion("2.0")]
    public IActionResult GetV2() => Ok("Versão 2.0");
}
```

## Versionamento de Dados

### Expansão Aditiva
- Adicione novos campos opcionais
- Mantenha compatibilidade backward

### Contratos Explícitos
```csharp
[ApiVersion("1.0")]
public class ProdutoV1
{
    public int Id { get; set; }
    public string Nome { get; set; }
}

[ApiVersion("2.0")]
public class ProdutoV2 : ProdutoV1
{
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
}
```

## Estratégias de Migração

### Versionamento Paralelo
- Mantenha múltiplas versões simultaneamente
- Permita migração gradual dos clientes

### Depreciação Graceful
```csharp
[HttpGet]
[Obsolete("Use v2.0")]
[MapToApiVersion("1.0")]
public IActionResult GetV1()
{
    Response.Headers.Add("Deprecation", "true");
    return Ok("Esta versão será descontinuada");
}
```

## Documentação

### OpenAPI/Swagger
```csharp
// Suporte a múltiplas versões
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "API v2", Version = "v2" });
});
```

## Boas Práticas

- Use versionamento semântico (Major.Minor.Patch)
- Documente breaking changes claramente
- Forneça período de depreciação
- Mantenha compatibilidade quando possível
- Use header deprecation para avisos
