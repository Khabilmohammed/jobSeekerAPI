using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IShareRepo
{
    public interface IShareRepository
    {
        Task<Share> AddAsync(Share share);
        Task<IEnumerable<Share>> GetSharesByUserIdAsync(string userId);
    }
}
