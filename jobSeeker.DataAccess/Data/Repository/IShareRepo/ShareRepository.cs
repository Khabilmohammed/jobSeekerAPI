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
            return share; // Return the created share entity
        }

        public async Task<Share> GetByIdAsync(int id)
        {
            return await _context.Shares
                .Include(s => s.Post)    // Include the related Post entity
                .Include(s => s.Sender)  // Include the Sender (User) details
                .Include(s => s.Recipient) // Include the Recipient (User) details
                .FirstOrDefaultAsync(s => s.ShareId == id);
        }


        public async Task<IEnumerable<Share>> GetSharesByUserIdAsync(string userId)
        {
            return await _context.Shares
                .Include(s => s.Post)     // Include Post details
                .Include(s => s.Sender)   // Include Sender details
                .Where(s => s.SenderId == userId) // Filter by SenderId
                .ToListAsync();
        }
    }
}
