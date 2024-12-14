using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotesWebAPI.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")]
[EnableCors("CustomAllowOriginsPolicy")]

public class AuthController : ControllerBase
{

    private static Dictionary<string, User> _users = new Dictionary<string, User>();

    [HttpPost("register")]

    public IActionResult Register([FromBody] RegisterModel model)
    {
        if (_users.ContainsKey(model.Email))
            return BadRequest("User already exists.");

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Password = model.Password  // Ensure hashing and salting are done in production!
           // DateOfBirth = model.DateOfBirth
        };

        _users[model.Email] = user;

        return Ok("User registered successfully.");
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (_users.ContainsKey(model.Email) && _users[model.Email].Password == model.Password)
        {
            var token = GenerateJwtToken(model.Email);
            return Ok(new { Token = token });
        }
        return Unauthorized("Invalid credentials.");
    }

    private string GenerateJwtToken(string email)
    {
        var claims = new[] {
        new Claim(ClaimTypes.Name, email),
        new Claim("email", email)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecureKeyHere"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "yourdomain.com",
            audience: "yourdomain.com",
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}


    public class RegisterModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

