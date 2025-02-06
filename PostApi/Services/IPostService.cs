using PostApi.Models;

namespace PostApi.Services;

public interface IPostService
{
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post> GetPostByIdAsync(int id);
    Task<Post> CreatePostAsync(Post newPost);
    Task<Post> UpdatePostAsync(int id, Post updatedPost);
    Task<bool> DeletePostAsync(int id);
}