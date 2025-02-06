using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using PostApi.Models;

namespace PostApi.Test;

public class UnitTest
{
    private readonly HttpClient _client;

    private readonly Post _expectedPost = new Post
        { Id = 101, UserId = 10, Title = "Test Post 1", Body = "Test Body 1" };

    public UnitTest()
    {
        var application = new WebApplicationFactory<Program>();
        _client = application.CreateClient();
    }

    [Fact]
    public async Task TestRootEndpoint()
    {
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        var response = await client.GetStringAsync("/");
        Assert.NotEqual("Hello World!", response);
    }

    [Fact]
    public async Task GetAllPosts_ReturnsAllPosts()
    {
        var response = await _client.GetStringAsync("/api/posts");

        var result = JsonConvert.DeserializeObject<List<Post>>(response);

        Assert.Equal(100, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
    }

    [Fact]
    public async Task GetPost_WithExistingId_ReturnsPost()
    {
        var response =
            await _client.PostAsync("/api/post", new StringContent(JsonConvert.SerializeObject(_expectedPost)));

        if (response.IsSuccessStatusCode)
        {
            var response2 = await _client.GetStringAsync($"/api/post/{_expectedPost.Id}");

            var result = JsonConvert.DeserializeObject<Post>(response2);

            Assert.Equal(_expectedPost.Id, result.Id);
            Assert.Equal(_expectedPost.UserId, result.UserId);
            Assert.Equal(_expectedPost.Title, result.Title);
            Assert.Equal(_expectedPost.Body, result.Body);
        }
    }

    [Fact]
    public async Task GetPost_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetStringAsync($"/api/post/{short.MaxValue}");

        Assert.Equal("Post not found.", response);
    }

    [Fact]
    public async Task CreatePost_WithValidData_ReturnsCreatedPost()
    {
        var stringPost = JsonConvert.SerializeObject(_expectedPost);
        var contentPost = new StringContent(stringPost, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/post", contentPost);

        if (response.IsSuccessStatusCode)
        {
            var content = response.Content;
            var jsonContent = content.ReadAsStringAsync().Result;

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            Assert.Equal(stringPost, jsonContent);
        }
    }

    [Fact]
    public async Task CreatePost_WithInvalidData_ReturnsBadRequest()
    {
        var newPost = new Post { Id = 0, Title = "", Body = "" };
        var requestBody = JsonConvert.SerializeObject(newPost);
        var response = await _client.PostAsync("/api/post", new StringContent(requestBody));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePost_WithExistingId_ReturnsUpdatedPost()
    {
        var existingPost = new Post { Id = 103, UserId = 10, Title = "Test Post 1", Body = "Test Body 1" };
        var stringPost = JsonConvert.SerializeObject(existingPost);
        var contentPost = new StringContent(stringPost, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/post", contentPost);

        var updatedPost = new Post { Id = 103, UserId = 10, Title = "Updated Post 1", Body = "Updated Body 1" };
        var requestBody = JsonConvert.SerializeObject(updatedPost);
        var response = await _client.PutAsync($"/api/post/{existingPost.Id}", new StringContent(requestBody));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = JsonConvert.DeserializeObject<Post>(response.Content.ReadAsStringAsync().Result);

        Assert.Equal(updatedPost.Id, result.Id);
        Assert.Equal(updatedPost.UserId, result.UserId);
        Assert.Equal(updatedPost.Title, result.Title);
        Assert.Equal(updatedPost.Body, result.Body);
    }

    [Fact]
    public async Task UpdatePost_WithNonExistingId_ReturnsNotFound()
    {
        var updatedPost = new Post { Id = short.MaxValue, Title = "Updated Post 1", Body = "Updated Body 1" };
        var requestBody = JsonConvert.SerializeObject(updatedPost);
        var response = await _client.PutAsync($"/api/post/{updatedPost.Id}", new StringContent(requestBody));
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

        var content = response.Content;
        var jsonContent = content.ReadAsStringAsync().Result;
        Assert.Equal("Post not found.", jsonContent);
    }

    [Fact]
    public async Task DeletePost_WithExistingId_RemovesPost()
    {
        var stringPost = JsonConvert.SerializeObject(_expectedPost);
        var contentPost = new StringContent(stringPost, Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/post", contentPost);

        var response = await _client.DeleteAsync($"/api/post/{_expectedPost.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var response2 = await _client.GetStringAsync($"/api/post/{_expectedPost.Id}");

        Assert.Equal("Post not found.", response2);
    }

    [Fact]
    public async Task DeletePost_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"/api/post/{short.MaxValue}");
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

        var content = response.Content;
        var jsonContent = content.ReadAsStringAsync().Result;
        Assert.Equal("Post not found.", jsonContent);
    }
}