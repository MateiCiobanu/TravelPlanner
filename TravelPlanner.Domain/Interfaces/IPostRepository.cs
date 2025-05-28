using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Entities;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<ICollection<Post>> GetPosts();
        Task<Post> GetPostById(int id);
        Task<bool> CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(Post post);
        Task<bool> Save();
    }
}
