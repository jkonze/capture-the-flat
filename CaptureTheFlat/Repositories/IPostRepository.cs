using System.Collections.Generic;
using CaptureTheFlat.Models;

namespace CaptureTheFlat.Repositories
{
    public abstract class IPostRepository
    {
        public abstract IEnumerable<Post> GetAllPosts();
        public abstract IEnumerable<Post> GetAllPostsWithoutCache();
        public abstract void Migrate();


    }
}