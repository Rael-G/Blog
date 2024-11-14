using System.ComponentModel.DataAnnotations;
using Blog.Application;
using Blog.Domain;
using Blog.WebApi.Models.Input;
using FluentAssertions;

namespace Blog.UnitTest.WebApi.Models;

public class CommentInputModelTests
{
    private readonly CommentInputModel _commentInputModel;

    public CommentInputModelTests()
    {
        _commentInputModel = new CommentInputModel { Author = "John Doe", Content = "Test comment", PostId = Guid.NewGuid() };
    }

    [Fact]
    public void InputToDto_ShouldReturnCorrectCommentDto()
    {
        var commentDto = _commentInputModel.InputToDto();

        commentDto.Should().NotBeNull();
        commentDto.Author.Should().Be(_commentInputModel.Author);
        commentDto.Content.Should().Be(_commentInputModel.Content);
        commentDto.PostId.Should().Be(_commentInputModel.PostId);
    }

    [Fact]
    public void InputToDto_WithCommentDtoParameter_ShouldUpdateCommentDtoCorrectly()
    {
        var commentDto = new CommentDto() {};

        _commentInputModel.InputToDto(commentDto);

        commentDto.Should().NotBeNull();
        commentDto.Author.Should().Be(_commentInputModel.Author);
        commentDto.Content.Should().Be(_commentInputModel.Content);
    }

    [Fact]
    public void InputToDto_WithCommentDtoParameter_ShouldNotAlterDtoPostId()
    {
        var postId = Guid.NewGuid();

        var commentDto = new CommentDto() {PostId = postId};

        _commentInputModel.InputToDto(commentDto);

        commentDto.Should().NotBeNull();
        commentDto.PostId.Should().Be(postId);
    }

    [Fact]
    public void Author_ShouldHaveRequiredAttribute()
    {
        var propertyInfo = typeof(CommentInputModel).GetProperty(nameof(CommentInputModel.Author));

        propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Author_ShouldHaveMaxLengthAttribute()
    {
        var propertyInfo = typeof(CommentInputModel).GetProperty(nameof(CommentInputModel.Author));

        propertyInfo.Should().BeDecoratedWith<MaxLengthAttribute>()
            .Which.Length.Should().Be(Comment.AuthorMaxLength);
    }

    [Fact]
    public void Content_ShouldHaveRequiredAttribute()
    {
        var propertyInfo = typeof(CommentInputModel).GetProperty(nameof(CommentInputModel.Content));

        propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Content_ShouldHaveMaxLengthAttribute()
    {
        var propertyInfo = typeof(CommentInputModel).GetProperty(nameof(CommentInputModel.Content));

        propertyInfo.Should().BeDecoratedWith<MaxLengthAttribute>()
            .Which.Length.Should().Be(Comment.ContentMaxLength);
    }

    [Fact]
    public void PostId_ShouldHaveRequiredAttribute()
    {
        var propertyInfo = typeof(CommentInputModel).GetProperty(nameof(CommentInputModel.PostId));

        propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }
}
