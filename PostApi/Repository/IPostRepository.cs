using PostApi.Models;

namespace PostApi.Repository;

public interface IPostRepository
{
    public List<Post> GetPosts();
    void AddRange(List<Post> posts);
    
    void Add(Post post);
    
    void Update(Post post);
    
    void Delete(Post post);
    
    Post GetById(int id);
}