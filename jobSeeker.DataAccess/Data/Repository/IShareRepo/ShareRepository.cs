using jobSeeker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.IShareRepo
{
    public class ShareRepository:IShareRepository
    {
        private readonly ApplicationDbContext _context;
        public ShareRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Share> AddAsync(Share share)
        {
            await _context.Shares.AddAsync(share);
            await _context.SaveChangesAsync();
            return share; 
        }


        public async Task<IEnumerable<Share>> GetSharesByUserIdAsync(string userId)
        {
            return await _context.Shares      
         .Where(sp => sp.SenderId == userId)
         .ToListAsync();
        }
    }
}
