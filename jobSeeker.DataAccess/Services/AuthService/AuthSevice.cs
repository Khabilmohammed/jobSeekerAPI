using jobSeeker.DataAccess.Services.ITokenBlacklistService;
using jobSeeker.DataAccess.Services.IUserRepositoryService;
using jobSeeker.Models;
using jobSeeker.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.AuthService
{
    public class AuthSevice
    {
       
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenBlacklistServices _tokenBlacklistService;


        public AuthSevice(IUserRepository userRepository,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
             ITokenBlacklistServices tokenBlacklistService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _userManager = userManager;
            _tokenBlacklistService = tokenBlacklistService;
        }


        public async Task LogoutAsync(string token)
        {
            await _tokenBlacklistService.AddToBlacklistAsync(token);
        }

        // Method to validate token against blacklist
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _tokenBlacklistService.IsBlacklistedAsync(token);
        }


        public async Task<LoginResposeDTO> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault())
            }),
                    Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new LoginResposeDTO
                {
                    Username = user.UserName,
                    Id=user.Id,
                    Email = user.Email,
                    Token = tokenHandler.WriteToken(token),
                    Expiration = tokenDescriptor.Expires.Value,
                };
            }
            return null;

        }

    }
}
