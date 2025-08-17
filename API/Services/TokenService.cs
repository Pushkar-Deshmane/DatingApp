using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        //Token Key
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot get token key");
        if (tokenKey.Length < 64) throw new Exception("Token Key needs to be >= 64 characters");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        //Claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        //Credentials
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        //Token Descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        //Token Handler
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
