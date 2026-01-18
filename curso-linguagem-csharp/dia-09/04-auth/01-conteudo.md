# Autenticação e Autorização

## Autenticação JWT

### Configuração Básica
```csharp
// Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

### Geração de Tokens
```csharp
public class AuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("custom_claim", "value")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
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

## Autorização

### Policy-Based Authorization
```csharp
// Program.cs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("PremiumUser", policy =>
        policy.RequireClaim("subscription", "premium"));
});

// Controller
[Authorize(Policy = "AdminOnly")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(int id)
{
    // Apenas admins podem deletar usuários
}
```

### Role-Based Authorization
```csharp
[Authorize(Roles = "Admin,Manager")]
[HttpPost]
public async Task<IActionResult> CreateReport()
{
    // Admin ou Manager podem criar relatórios
}
```

### Claims-Based Authorization
```csharp
[Authorize(Policy = "DepartmentManager")]
public class DepartmentController : ControllerBase
{
    // Apenas gerentes do departamento específico
}
```

## Refresh Tokens

### Implementação Básica
```csharp
public class TokenService
{
    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _tokenRepository.GetByTokenAsync(refreshToken);
        if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
            throw new SecurityTokenException("Invalid refresh token");

        var newAccessToken = GenerateAccessToken(storedToken.User);
        var newRefreshToken = GenerateRefreshToken();

        await _tokenRepository.UpdateAsync(storedToken.Id, newRefreshToken);

        return new TokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token
        };
    }
}
```

## Segurança Adicional

### Rate Limiting
```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
    });
});

app.UseRateLimiter();
```

### CORS
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:3000", "https://meuapp.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

app.UseCors("AllowSpecificOrigins");
```

## Middleware de Segurança

### Custom Authorization Middleware
```csharp
public class CustomAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public CustomAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var authorizeAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

        if (authorizeAttribute != null)
        {
            // Lógica customizada de autorização
            var user = context.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = 401;
                return;
            }
        }

        await _next(context);
    }
}
```
