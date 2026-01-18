# 📝 Exercícios - Autenticação

## 🎯 Objetivo

Praticar implementação de autenticação e autorização no ASP.NET Core usando JWT, roles, policies e refresh tokens.

---

## ✏️ Exercício 1: JWT Básico

**Dificuldade**: ⭐ Iniciante

Configure autenticação JWT na aplicação com login e proteção de endpoints:

### Requisitos:
1. **Modelos de autenticação**:
```csharp
public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Senha { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime Expiracao { get; set; }
    public string Usuario { get; set; }
    public string Role { get; set; }
}

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string SenhaHash { get; set; }
}
```

2. **Configuração JWT no Program.cs**:
```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();
```

3. **Serviço de autenticação**:
```csharp
public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    // Usuários de exemplo (em produção, viriam do banco)
    private readonly List<Usuario> _usuarios = new()
    {
        new Usuario { Id = 1, Nome = "Admin", Email = "admin@email.com", Role = "Admin", SenhaHash = "admin123" },
        new Usuario { Id = 2, Nome = "João", Email = "joao@email.com", Role = "User", SenhaHash = "user123" }
    };

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var usuario = _usuarios.FirstOrDefault(u =>
            u.Email == request.Email && u.SenhaHash == request.Senha);

        if (usuario == null)
            return null;

        var token = GerarJwtToken(usuario);
        return new LoginResponse
        {
            Token = token,
            Expiracao = DateTime.Now.AddHours(1),
            Usuario = usuario.Nome,
            Role = usuario.Role
        };
    }

    private string GerarJwtToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

4. **Controller de autenticação**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        if (response == null)
            return Unauthorized("Credenciais inválidas");

        return Ok(response);
    }
}
```

5. **Controller protegido**:
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DadosController : ControllerBase
{
    [HttpGet("publico")]
    [AllowAnonymous]
    public IActionResult GetPublico()
    {
        return Ok(new { mensagem = "Este endpoint é público" });
    }

    [HttpGet("protegido")]
    public IActionResult GetProtegido()
    {
        var usuario = User.Identity.Name;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            mensagem = "Este endpoint é protegido",
            usuario = usuario,
            role = role,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdmin()
    {
        return Ok(new { mensagem = "Apenas administradores podem acessar" });
    }
}
```

6. **Arquivo appsettings.json**:
```json
{
  "Jwt": {
    "Key": "chave-secreta-muito-longa-para-jwt-token-2025",
    "Issuer": "MinhaAPI",
    "Audience": "MinhaAPI-Clientes"
  }
}
```

7. **Testes de autenticação**:
```bash
# Login
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@email.com", "senha": "admin123"}'

# Endpoint público
curl -H "Accept: application/json" "https://localhost:5001/api/dados/publico"

# Endpoint protegido (usar token do login)
curl -H "Authorization: Bearer {TOKEN}" \
  -H "Accept: application/json" \
  "https://localhost:5001/api/dados/protegido"

# Endpoint admin
curl -H "Authorization: Bearer {TOKEN}" \
  -H "Accept: application/json" \
  "https://localhost:5001/api/dados/admin"
```

---

## ✏️ Exercício 2: Roles e Policies

**Dificuldade**: ⭐⭐ Intermediário

Implemente autorização baseada em roles e policies com middleware customizado:

### Requisitos:
1. **Configuração de policies no Program.cs**:
```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("UserPremium", policy =>
        policy.RequireRole("User", "Admin")
              .RequireClaim("SubscriptionLevel", "Premium", "Gold"));

    options.AddPolicy("IdadeMinima", policy =>
        policy.Requirements.Add(new IdadeMinimaRequirement(18)));

    options.AddPolicy("HorarioComercial", policy =>
        policy.Requirements.Add(new HorarioComercialRequirement()));
});
```

2. **Requirement customizado para idade**:
```csharp
public class IdadeMinimaRequirement : IAuthorizationRequirement
{
    public int IdadeMinima { get; }

    public IdadeMinimaRequirement(int idadeMinima)
    {
        IdadeMinima = idadeMinima;
    }
}

public class IdadeMinimaHandler : AuthorizationHandler<IdadeMinimaRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IdadeMinimaRequirement requirement)
    {
        var dataNascimentoClaim = context.User.FindFirst("DataNascimento");
        if (dataNascimentoClaim == null)
        {
            return Task.CompletedTask;
        }

        if (DateTime.TryParse(dataNascimentoClaim.Value, out var dataNascimento))
        {
            var idade = DateTime.Now.Year - dataNascimento.Year;
            if (dataNascimento > DateTime.Now.AddYears(-idade))
                idade--;

            if (idade >= requirement.IdadeMinima)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}
```

3. **Requirement para horário comercial**:
```csharp
public class HorarioComercialRequirement : IAuthorizationRequirement { }

public class HorarioComercialHandler : AuthorizationHandler<HorarioComercialRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HorarioComercialRequirement requirement)
    {
        var horaAtual = DateTime.Now.Hour;
        var diaSemana = DateTime.Now.DayOfWeek;

        // Horário comercial: segunda a sexta, 8h às 18h
        if (diaSemana >= DayOfWeek.Monday && diaSemana <= DayOfWeek.Friday &&
            horaAtual >= 8 && horaAtual < 18)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

4. **Registro dos handlers**:
```csharp
builder.Services.AddSingleton<IAuthorizationHandler, IdadeMinimaHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, HorarioComercialHandler>();
```

5. **Controller com diferentes políticas**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class RecursosController : ControllerBase
{
    [HttpGet("admin")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult GetAdmin()
    {
        return Ok(new { mensagem = "Recurso exclusivo para administradores" });
    }

    [HttpGet("premium")]
    [Authorize(Policy = "UserPremium")]
    public IActionResult GetPremium()
    {
        var subscriptionLevel = User.FindFirst("SubscriptionLevel")?.Value;
        return Ok(new { mensagem = "Recurso premium", nivel = subscriptionLevel });
    }

    [HttpGet("maioridade")]
    [Authorize(Policy = "IdadeMinima")]
    public IActionResult GetMaioridade()
    {
        return Ok(new { mensagem = "Conteúdo para maiores de 18 anos" });
    }

    [HttpGet("comercial")]
    [Authorize(Policy = "HorarioComercial")]
    public IActionResult GetHorarioComercial()
    {
        return Ok(new
        {
            mensagem = "Recurso disponível apenas em horário comercial",
            horario = DateTime.Now.ToString("HH:mm:ss"),
            dia = DateTime.Now.DayOfWeek
        });
    }

    [HttpGet("multiplas-policies")]
    [Authorize(Policy = "UserPremium")]
    [Authorize(Policy = "HorarioComercial")]
    public IActionResult GetMultiplasPolicies()
    {
        return Ok(new { mensagem = "Recurso com múltiplas políticas" });
    }
}
```

6. **Middleware de logging de acessos**:
```csharp
public class AuthorizationLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationLoggingMiddleware> _logger;

    public AuthorizationLoggingMiddleware(RequestDelegate next, ILogger<AuthorizationLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var authorizeData = endpoint.Metadata.GetMetadata<IAuthorizeData>();
            if (authorizeData != null)
            {
                var user = context.User.Identity?.Name ?? "Anônimo";
                var statusCode = context.Response.StatusCode;
                var path = context.Request.Path;

                _logger.LogInformation(
                    "Acesso autorizado - Usuário: {User}, Status: {Status}, Path: {Path}",
                    user, statusCode, path);
            }
        }
    }
}
```

7. **Registro do middleware**:
```csharp
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthorizationLoggingMiddleware>();
```

---

## ✏️ Exercício 3: Refresh Tokens

**Dificuldade**: ⭐⭐⭐ Avançado

Implemente sistema completo de refresh tokens com invalidação:

### Requisitos:
1. **Modelos para refresh tokens**:
```csharp
public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}

public class RefreshToken
{
    public string Token { get; set; }
    public string UsuarioId { get; set; }
    public DateTime Expiracao { get; set; }
    public bool Invalido { get; set; }
    public DateTime CriadoEm { get; set; }
    public string CriadoPorIp { get; set; }
}

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
```

2. **Serviço de tokens**:
```csharp
public interface ITokenService
{
    Task<TokenResponse> GerarTokensAsync(string usuarioId, string role);
    Task<TokenResponse?> RefreshTokensAsync(string refreshToken, string ipAddress);
    Task RevogarRefreshTokenAsync(string refreshToken, string ipAddress = null);
    Task RevogarTodosTokensUsuarioAsync(string usuarioId);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly ConcurrentDictionary<string, RefreshToken> _refreshTokens = new();

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<TokenResponse> GerarTokensAsync(string usuarioId, string role)
    {
        var accessToken = GerarAccessToken(usuarioId, role);
        var refreshToken = GerarRefreshToken();

        var refreshTokenObj = new RefreshToken
        {
            Token = refreshToken,
            UsuarioId = usuarioId,
            Expiracao = DateTime.Now.AddDays(7),
            CriadoEm = DateTime.Now,
            CriadoPorIp = "127.0.0.1" // Em produção, pegar do contexto
        };

        _refreshTokens[refreshToken] = refreshTokenObj;

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiration = DateTime.Now.AddMinutes(15),
            RefreshTokenExpiration = refreshTokenObj.Expiracao
        };
    }

    public async Task<TokenResponse?> RefreshTokensAsync(string refreshToken, string ipAddress)
    {
        if (!_refreshTokens.TryGetValue(refreshToken, out var storedToken))
            return null;

        if (storedToken.Invalido || storedToken.Expiracao < DateTime.Now)
            return null;

        // Gerar novos tokens
        var newTokens = await GerarTokensAsync(storedToken.UsuarioId, "User"); // Role deveria vir do usuário

        // Invalidar token antigo
        storedToken.Invalido = true;

        return newTokens;
    }

    public async Task RevogarRefreshTokenAsync(string refreshToken, string ipAddress = null)
    {
        if (_refreshTokens.TryGetValue(refreshToken, out var token))
        {
            token.Invalido = true;
        }
    }

    public async Task RevogarTodosTokensUsuarioAsync(string usuarioId)
    {
        var tokensUsuario = _refreshTokens.Where(t => t.Value.UsuarioId == usuarioId);
        foreach (var token in tokensUsuario)
        {
            token.Value.Invalido = true;
        }
    }

    private string GerarAccessToken(string usuarioId, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuarioId),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GerarRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
```

3. **Controller de autenticação com refresh**:
```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AuthController(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _authService.LoginAsync(request);
        if (usuario == null)
            return Unauthorized();

        var tokens = await _tokenService.GerarTokensAsync(usuario.Id.ToString(), usuario.Role);
        return Ok(tokens);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var newTokens = await _tokenService.RefreshTokensAsync(request.RefreshToken, ipAddress);

        if (newTokens == null)
            return Unauthorized("Refresh token inválido ou expirado");

        return Ok(newTokens);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        await _tokenService.RevogarRefreshTokenAsync(request.RefreshToken, ipAddress);
        return Ok();
    }

    [HttpPost("logout-all")]
    [Authorize]
    public async Task<IActionResult> LogoutAll()
    {
        var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (usuarioId != null)
        {
            await _tokenService.RevogarTodosTokensUsuarioAsync(usuarioId);
        }
        return Ok();
    }
}
```

4. **Configuração de JWT com expiração curta**:
```csharp
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero // Remover tolerância de expiração
    };

    // Evento para refresh token quando access token expirar
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});
```

5. **Testes completos**:
```bash
# 1. Login
LOGIN_RESPONSE=$(curl -s -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@email.com", "senha": "admin123"}')

ACCESS_TOKEN=$(echo $LOGIN_RESPONSE | jq -r '.accessToken')
REFRESH_TOKEN=$(echo $LOGIN_RESPONSE | jq -r '.refreshToken')

# 2. Usar access token
curl -H "Authorization: Bearer $ACCESS_TOKEN" \
  "https://localhost:5001/api/dados/protegido"

# 3. Aguardar expiração do access token (15 minutos) e usar refresh
curl -X POST "https://localhost:5001/api/auth/refresh" \
  -H "Content-Type: application/json" \
  -d "{\"refreshToken\": \"$REFRESH_TOKEN\"}"

# 4. Logout
curl -X POST "https://localhost:5001/api/auth/logout" \
  -H "Content-Type: application/json" \
  -d "{\"refreshToken\": \"$REFRESH_TOKEN\"}"
```
