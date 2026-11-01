using BookReviewApi.DTOs;
using BookReviewApi.Models;
using BookReviewApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookReviewApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public UserService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<RegisterResult> RegisterAsync(RegisterDto dto)
    {
        var username = dto.Username.Trim();
        var email = dto.Email.Trim();

        if (await _userRepo.UsernameExistsAsync(username))
            return new RegisterResult(null, "Username is already taken");

        if (await _userRepo.EmailExistsAsync(email))
            return new RegisterResult(null, "Email is already registered");

        var user = new User
        {
            Username = username,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _userRepo.AddUserAsync(user);

        var response = new RegisterResponseDto
        {
            Username = user.Username,
            Email = user.Email
        };

        return new RegisterResult(response, null);
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetUserByUsernameAsync(dto.Username.Trim());

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var jwtKey = _config["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("Jwt:Key is missing from configuration");

        if (Encoding.UTF8.GetBytes(jwtKey).Length < 32)
            throw new InvalidOperationException("Jwt:Key must be at least 32 characters long");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}