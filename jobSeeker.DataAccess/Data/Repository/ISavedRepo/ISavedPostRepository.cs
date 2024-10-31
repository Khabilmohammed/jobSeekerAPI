using jobSeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Data.Repository.ISavedRepo
{
    public interface ISavedPostRepository
    {
        Task<bool> SavedPostExistsAsync(string userId, int postId);
        Task<IEnumerable<SavedPost>> GetUserSavedPostsAsync(string userId);
        Task<SavedPost> GetSavedPostAsync(string userId, int postId);
        Task AddSavedPostAsync(SavedPost savedPost);
        Task RemoveSavedPostAsync(SavedPost savedPost);
        Task<int> SaveChangesAsync();// to save any changes made
    }
}
