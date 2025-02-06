using Newtonsoft.Json;
using PostApi.Models;

namespace PostApi.Services;

public class PostService : IPostService
{
    private readonly List<Post> _posts = [];

    public async Task<IEnumerable<Post>> GetAllPostsAsync()
    {
        if (_posts.Count == 0)
            await PopulateDatabase();

        return _posts;
    }

    public async Task<Post> GetPostByIdAsync(int id)
    {
        if (_posts.Count == 0)
            await PopulateDatabase();

        return _posts.FirstOrDefault(p => p.Id == id)!;
    }

    public Task<Post> CreatePostAsync(Post newPost)
    {
        if (newPost.Id == 0 || newPost.UserId == 0 || string.IsNullOrEmpty(newPost.Title) ||
            string.IsNullOrEmpty(newPost.Body))
            return Task.FromResult<Post>(null!);

        _posts.Add(newPost);
        return Task.FromResult(newPost);
    }

    public Task<Post> UpdatePostAsync(int id, Post updatedPost)
    {
        var existingPost = _posts.FirstOrDefault(p => p.Id == id);

        if (existingPost == null) return Task.FromResult<Post?>(null)!;

        existingPost.Title = updatedPost.Title;
        existingPost.Body = updatedPost.Body;

        return Task.FromResult(existingPost);
    }

    public Task<bool> DeletePostAsync(int id)
    {
        var post = _posts.FirstOrDefault(p => p.Id == id);

        if (post == null) return Task.FromResult(false);

        _posts.Remove(post);
        return Task.FromResult(true);
    }

    private async Task PopulateDatabase()
    {
        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
            var content = await response.Content.ReadAsStringAsync();
            var retrievedPosts = JsonConvert.DeserializeObject<List<Post>>(content);
            _posts.AddRange(retrievedPosts!);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while populating the database: {ex.Message}");
        }
    }
}