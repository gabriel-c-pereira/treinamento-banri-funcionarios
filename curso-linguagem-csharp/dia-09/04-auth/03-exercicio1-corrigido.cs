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

// appsettings.json
{
  "Jwt": {
    "Key": "minha-chave-secreta-super-segura-com-pelo-menos-32-caracteres",
    "Issuer": "MinhaAPI",
    "Audience": "MinhaAPIUsers"
  }
}

// AuthController.cs
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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await _authService.AuthenticateAsync(request.Email, request.Password);
        if (token == null)
            return Unauthorized(new { message = "Credenciais inválidas" });

        return Ok(new { token });
    }
}

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    [HttpGet("perfil")]
    [Authorize]
    public IActionResult GetPerfil()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new { userId, email });
    }
}

// AuthService.cs
public interface IAuthService
{
    Task<string> AuthenticateAsync(string email, string password);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly Dictionary<string, (string Password, string Role)> _users = new()
    {
        { "admin@exemplo.com", ("admin123", "Admin") },
        { "user@exemplo.com", ("user123", "User") }
    };

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        if (_users.TryGetValue(email, out var userData) && userData.Password == password)
        {
            return GenerateToken(email, userData.Role);
        }

        return null;
    }

    private string GenerateToken(string email, string role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role)
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

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}