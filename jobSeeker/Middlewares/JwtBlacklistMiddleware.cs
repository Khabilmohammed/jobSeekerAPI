using jobSeeker.DataAccess.Services.IUserRepositoryService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.JWTBlackListService
{
    public class JwtBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthService _authService;

        public JwtBlacklistMiddleware(RequestDelegate next, AuthService authService)
        {
            _next = next;
            _authService = authService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null && await _authService.IsTokenBlacklistedAsync(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Token is blacklisted.");
                return;
            }


            await _next(context);
        }
    }

   
}
