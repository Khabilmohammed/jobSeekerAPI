using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.ITokenBlacklistService
{
    public class TokenBlacklistService : ITokenBlacklistServices
    {
        private static readonly HashSet<string> _blacklistedTokens = new HashSet<string>();
        public Task AddToBlacklistAsync(string token)
        {
            _blacklistedTokens.Add(token);
            return Task.CompletedTask;
        }

        public Task<bool> IsBlacklistedAsync(string token)
        {
            return Task.FromResult(_blacklistedTokens.Contains(token));
        }
    }
}
