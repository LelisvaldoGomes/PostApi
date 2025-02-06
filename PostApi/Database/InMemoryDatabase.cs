using PostApi.Models;

namespace PostApi.Database;

public class InMemoryDatabase
{
    private readonly List<Post> _posts;

    public InMemoryDatabase()
    {
        _posts = new List<Post>();
    }

    public void Add(Post post)
    {
        _posts.Add(post);
    }

    public void AddRange(IEnumerable<Post> posts)
    {
        _posts.AddRange(posts);
    }

    public void Remove(Post post)
    {
        _posts.Remove(post);
    }

    public void Clear()
    {
        _posts.Clear();
    }

    public IEnumerable<Post> GetAll()
    {
        return _posts;
    }

    public int GetNextId()
    {
        if (_posts.Count == 0)
            return 1;

        return _posts.Max(p => p.Id) + 1;
    }
}