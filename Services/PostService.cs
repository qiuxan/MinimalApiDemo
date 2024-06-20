
using Microsoft.Extensions.Hosting;

namespace MinimalApiDemo.Services
{
    public class PostService : IPostService
    {

        private static readonly List<Post> AllPosts = new()
        {
            new() { Id = 1, Title = "First Post", Content = "Hello World" },
            new() { Id = 2, Title = "Second Post", Content = "Hello Again" },
            new() { Id = 3, Title = "Third Post", Content = "Goodbye World" },
        };

        public Task<Post> CreatePost(Post item)
        {
            AllPosts.Add(item);
            return Task.FromResult(item);
        }

        public Task DeletePost(int id)
        {
            var post = AllPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                throw new KeyNotFoundException();

            }
            AllPosts.Remove(post);
            return Task.CompletedTask;

        }

        public Task<Post> GetPost(int id)
        {
            var post = AllPosts.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(post);
        }

        public Task<List<Post>> GetPosts()
        {
            return Task.FromResult(AllPosts);
        }

        public Task<Post?> UpdatePost(int id, Post item)
        {
            var index = AllPosts.FindIndex(p => p.Id == id);
            if (index == -1)
            {
                return Task.FromResult<Post?>(null);
            }
            AllPosts[index] = item;
            return Task.FromResult(item);
            
        }
    }
}
