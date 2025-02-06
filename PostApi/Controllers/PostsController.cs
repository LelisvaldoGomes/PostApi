using Microsoft.AspNetCore.Mvc;
using PostApi.Models;
using PostApi.Services;

namespace PostApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postService.GetAllPostsAsync();
        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPost(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
            return NotFound("Post not found.");

        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] Post newPost)
    {
        if (newPost == null)
            return BadRequest("Required fields not filled.");

        var createdPost = await _postService.CreatePostAsync(newPost);
        if (createdPost == null)
            return BadRequest("Required fields not filled.");

        return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] Post updatedPost)
    {
        var existingPost = await _postService.UpdatePostAsync(id, updatedPost);
        if (existingPost == null)
            return NotFound("Post not found.");

        return Ok(existingPost);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var result = await _postService.DeletePostAsync(id);
        if (!result)
            return NotFound("Post not found.");

        return NoContent();
    }
}