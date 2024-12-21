using jobSeeker.Models;
using jobSeeker.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jobSeeker.DataAccess.Services.IShareService
{
    public interface IShareServices
    {
        Task<ShareDTO> SharePostAsync(CreateShareDTO shareDTO);
        Task<IEnumerable<Share>> GetUserSharedPostsAsync(string userId);

    }
}
