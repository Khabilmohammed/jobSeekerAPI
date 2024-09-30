using jobSeeker.DataAccess.Data.Repository.IUserRepository;
using jobSeeker.DataAccess.Services.OtpService;
using jobSeeker.Models.DTO;
using jobSeeker.Models;
using jobSeeker.Utility;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jobSeeker.DataAccess.Services.IEmailService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using jobSeeker.DataAccess.Services.ITokenBlacklistService;
using System.Net;
using Microsoft.EntityFrameworkCore;
using jobSeeker.DataAccess.Services.TokenService;
using System.Web;

namespace jobSeeker.DataAccess.Services.IUserRepositoryService
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly OTPService _otpService;
        private readonly IEmailservice _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenBlacklistServices _tokenBlacklistService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private static readonly Dictionary<string, RegisterRequestDTO> _temporaryStorage = new();

        public AuthService(IUserRepository userRepository,
                           UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           OTPService otpService,
                           IEmailservice emailService,
                           IConfiguration configuration,
                           ITokenBlacklistServices tokenBlacklistService,
                           IPasswordHasher<ApplicationUser> passwordHasher,
                           ITokenService tokenService
                             )
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _otpService = otpService;
            _emailService = emailService;
            _configuration = configuration;
            _tokenBlacklistService = tokenBlacklistService;
            _passwordHasher = passwordHasher;
            _tokenService=tokenService;
        }

        public async Task RegisterAsync(RegisterRequestDTO model)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
            if (existingUser != null)
                throw new Exception("User already exists with this username.");

            existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
                throw new Exception("User already exists with this email.");

            var otp = await _otpService.GenerateAndSaveOTPAsync(model.Email);
            var subject = "Your OTP Code";
            var body = $"Your OTP code is {otp}. It is valid for 10 minutes. Please complete the registration.";
            await _emailService.SendEmailAsync(model.Email, subject, body);

            // Store registration details temporarily
            _temporaryStorage[model.Email] = model;
        }

        public async Task<APIResponse> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return ResponseHelper.Error("User not found.", HttpStatusCode.NotFound);
            }
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                return ResponseHelper.Error($"Account is locked until {user.LockoutEnd.Value.UtcDateTime}.", HttpStatusCode.Forbidden);
            }
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return ResponseHelper.Error("Invalid password.", HttpStatusCode.Unauthorized);
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? string.Empty),
            new Claim("firstName", user.FirstName ?? string.Empty),
            new Claim("lastName", user.LastName ?? string.Empty),
            new Claim("city", user.City ?? string.Empty),
            new Claim("country", user.Country ?? string.Empty),
            new Claim("pincode", user.Pincode ?? string.Empty)
        }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Prepare the response
            var loginResponse = new LoginResposeDTO
            {
                Username = user.UserName,
                Id = user.Id,
                Email = user.Email,
                Token = tokenString,
                Expiration = tokenDescriptor.Expires.Value,
            };

            return ResponseHelper.Success(loginResponse);
        }


        public async Task<bool> ValidateOtpAsync(OTPValidationRequestDTO model)
        {
            var isValid = await _otpService.ValidateOTPAsync(model.Email, model.OTP);
            if (!isValid || !_temporaryStorage.TryGetValue(model.Email, out var registrationDetails))
                return false;

            var newUser = new ApplicationUser
            {
                UserName = registrationDetails.Username,
                Email = model.Email,
                FirstName = registrationDetails.FirstName,
                LastName = registrationDetails.LastName,
                City = registrationDetails.City,
                Country = registrationDetails.Country,
                Pincode = registrationDetails.Pincode,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, registrationDetails.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await EnsureRolesExist();
            await _userManager.AddToRoleAsync(newUser, registrationDetails.Role);

            _temporaryStorage.Remove(model.Email);
            return true;
        }

        public async Task ResendOtpAsync(string email)
        {

            var existingUserOtp = await _userRepository.GetUserByOTPtableAsync(email);

            if (existingUserOtp == null)
                throw new Exception("User not found with this email.");

            // Generate a new OTP
            var otp = await _otpService.GenerateAndSaveOTPAsync(email);
            var subject = "Your OTP Code";
            var body = $"Your new OTP code is {otp}. It is valid for 10 . Please complete the registration.";

            // Send the email
            await _emailService.SendEmailAsync(email, subject, body);
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


        public async Task ForgetPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                throw new Exception("User with this email doesn't exist.");

            // Generate a password reset token
            var resetToken = await _tokenService.GeneratePasswordResetTokenAsync(user);

            // Generate the reset link (this could be an API endpoint on your front-end)
            var resetLink = $"http://localhost:3000/resetPassword?email={user.Email}&token={HttpUtility.UrlEncode(resetToken)}";
            var subject = "Password Reset Request";
            var body = $"Please reset your password using this link: {resetLink}. The link is valid for 15 minutes.";

            // Send the password reset email
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid email.");

            var isValidToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token);
            if (!isValidToken)
                throw new Exception("Invalid or expired token.");

            // Hash the new password and update it
            var hashedPassword = _passwordHasher.HashPassword(user, newPassword);
            await _userRepository.UpdatePasswordAsync(user, hashedPassword);
        }

        private async Task EnsureRolesExist()
        {
            if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
            if (!await _roleManager.RoleExistsAsync(SD.Role_User))
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User));
            if (!await _roleManager.RoleExistsAsync(SD.Role_Company))
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Company));
        }

        
    }
}
