using PostApi.Models;

namespace PostApi.Repository;

public class PostRepository : IPostRepository
{
    public List<Post> GetPosts()
    {
        using var context = new ApiContext();
        var list = context.Posts.ToList();

        return list;
    }

    public void AddRange(List<Post> posts)
    {
        using var context = new ApiContext();
        context.Posts.AddRange(posts);
        context.SaveChanges();
    }

    public void Add(Post post)
    {
        using var context = new ApiContext();
        context.Posts.Add(post);
        context.SaveChanges();
    }

    public void Update(Post post)
    {
        using var context = new ApiContext();
        context.Posts.Update(post);
        context.SaveChanges();
    }

    public void Delete(Post post)
    {
        using var context = new ApiContext();
        context.Posts.Remove(post);
        context.SaveChanges();
    }

    public Post GetById(int id)
    {
        using var context = new ApiContext();
        var post = context.Posts.Find(id);

        return post;
    }
}