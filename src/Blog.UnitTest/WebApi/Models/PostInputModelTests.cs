using System.ComponentModel.DataAnnotations;
using Blog.Application;
using Blog.Domain;
using Blog.WebApi.Models.Input;
using FluentAssertions;

namespace Blog.UnitTest.WebApi.Models;

public class PostInputModelTests
{
    private readonly PostInputModel _postInputModel;

    public PostInputModelTests()
    {
        _postInputModel = new PostInputModel("Test Post", "This is a test post content", []);
    }

    [Fact]
    public void InputToDto_ShouldReturnCorrectPostDto()
    {
        var postDto = _postInputModel.InputToDto();

        postDto.Should().NotBeNull();
        postDto.Title.Should().Be(_postInputModel.Title);
        postDto.Content.Should().Be(_postInputModel.Content);
        postDto.Tags.Should().BeEquivalentTo(_postInputModel.Tags);
    }

    [Fact]
    public void InputToDto_WithPostDtoParameter_ShouldUpdatePostDtoCorrectly()
    {
        var postDto = new PostDto() { };

        _postInputModel.InputToDto(postDto);

        postDto.Should().NotBeNull();
        postDto.Title.Should().Be(_postInputModel.Title);
        postDto.Content.Should().Be(_postInputModel.Content);
        postDto.Tags.Should().BeEquivalentTo(_postInputModel.Tags);
    }

    [Fact]
    public void Title_ShouldHaveRequiredAttribute()
    {
        var propertyInfo = typeof(PostInputModel).GetProperty(nameof(PostInputModel.Title));

        propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Title_ShouldHaveMaxLengthAttribute()
    {
        var propertyInfo = typeof(PostInputModel).GetProperty(nameof(PostInputModel.Title));

        propertyInfo.Should().BeDecoratedWith<MaxLengthAttribute>()
            .Which.Length.Should().Be(Post.TitleMaxLength);
    }

    [Fact]
    public void Content_ShouldHaveRequiredAttribute()
    {
        var propertyInfo = typeof(PostInputModel).GetProperty(nameof(PostInputModel.Content));

        propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Tags_ShouldHaveRequiredAttribute()
    {
        var propertyInfo = typeof(PostInputModel).GetProperty(nameof(PostInputModel.Tags));

        propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }
}