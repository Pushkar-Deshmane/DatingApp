using System;
using API.Models;

namespace API.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
