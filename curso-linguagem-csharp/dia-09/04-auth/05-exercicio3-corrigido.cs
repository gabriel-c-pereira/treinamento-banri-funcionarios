// Models
public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}

public class RefreshTokenEntity
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; }
}

// AuthController.cs
[ApiController]
[Route("api/auth")]
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

        var result = await _authService.LoginAsync(request);
        if (result == null)
            return Unauthorized(new { message = "Credenciais inválidas" });

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(result);
        }
        catch (SecurityTokenException)
        {
            return Unauthorized(new { message = "Refresh token inválido" });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            await _authService.RevokeUserTokensAsync(userId);
        }

        return Ok(new { message = "Logout realizado com sucesso" });
    }
}

// AuthService.cs
public interface IAuthService
{
    Task<TokenResponse> LoginAsync(LoginRequest request);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    Task RevokeUserTokensAsync(string userId);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly Dictionary<string, (string Password, string Role, string UserId)> _users = new()
    {
        { "admin@exemplo.com", ("admin123", "Admin", "user-1") },
        { "user@exemplo.com", ("user123", "User", "user-2") }
    };

    public AuthService(IConfiguration config, IRefreshTokenRepository tokenRepository)
    {
        _config = config;
        _tokenRepository = tokenRepository;
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        if (_users.TryGetValue(request.Email, out var userData) &&
            userData.Password == request.Password)
        {
            // Revoke existing tokens for user
            await _tokenRepository.RevokeUserTokensAsync(userData.UserId);

            var accessToken = GenerateAccessToken(userData.UserId, request.Email, userData.Role);
            var refreshToken = await GenerateRefreshTokenAsync(userData.UserId);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        return null;
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _tokenRepository.GetByTokenAsync(refreshToken);
        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            throw new SecurityTokenException("Invalid refresh token");

        // Generate new tokens
        var accessToken = GenerateAccessToken(storedToken.UserId, "user@email.com", "User");
        var newRefreshToken = await GenerateRefreshTokenAsync(storedToken.UserId);

        // Revoke old refresh token
        await _tokenRepository.RevokeTokenAsync(storedToken.Id);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }

    public async Task RevokeUserTokensAsync(string userId)
    {
        await _tokenRepository.RevokeUserTokensAsync(userId);
    }

    private string GenerateAccessToken(string userId, string email, string role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
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

    private async Task<string> GenerateRefreshTokenAsync(string userId)
    {
        var token = Guid.NewGuid().ToString("N");
        var refreshToken = new RefreshTokenEntity
        {
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false
        };

        await _tokenRepository.AddAsync(refreshToken);
        return token;
    }
}

// RefreshTokenRepository.cs
public interface IRefreshTokenRepository
{
    Task<RefreshTokenEntity> GetByTokenAsync(string token);
    Task AddAsync(RefreshTokenEntity token);
    Task RevokeTokenAsync(int id);
    Task RevokeUserTokensAsync(string userId);
}

public class InMemoryRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly List<RefreshTokenEntity> _tokens = new();
    private int _nextId = 1;

    public Task<RefreshTokenEntity> GetByTokenAsync(string token)
    {
        var refreshToken = _tokens.FirstOrDefault(t => t.Token == token && !t.IsRevoked);
        return Task.FromResult(refreshToken);
    }

    public Task AddAsync(RefreshTokenEntity token)
    {
        token.Id = _nextId++;
        _tokens.Add(token);
        return Task.CompletedTask;
    }

    public Task RevokeTokenAsync(int id)
    {
        var token = _tokens.FirstOrDefault(t => t.Id == id);
        if (token != null)
        {
            token.IsRevoked = true;
        }
        return Task.CompletedTask;
    }

    public Task RevokeUserTokensAsync(string userId)
    {
        var userTokens = _tokens.Where(t => t.UserId == userId);
        foreach (var token in userTokens)
        {
            token.IsRevoked = true;
        }
        return Task.CompletedTask;
    }
}