namespace MinimalApiDemo.Services
{
    public interface IPostService
    {
        Task<Post> CreatePost(Post item);
        Task<List<Post>> GetPosts();
        Task<Post?> GetPost(int id);
        Task DeletePost(int id);
        Task<Post?> UpdatePost(int id, Post item);
      
    }
}
