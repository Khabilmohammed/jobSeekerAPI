using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.ITokenBlacklistService
{
    public interface ITokenBlacklistServices
    {
        Task AddToBlacklistAsync(string token);
        Task<bool> IsBlacklistedAsync(string token);
    }
}
