using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.ISavedPostService
{
    public interface ISavedPostservices
    {
        Task<bool> SavePostAsync(string userId, int postId);
        Task<bool> RemoveSavedPostAsync(string userId, int postId);
        Task<IEnumerable<SavedPost>> GetUserSavedPostsAsync(string userId);
    }
}
