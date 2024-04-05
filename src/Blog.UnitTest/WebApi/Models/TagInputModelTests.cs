using System.ComponentModel.DataAnnotations;
using Blog.Application;
using Blog.WebApi;
using FluentAssertions;

namespace Blog.UnitTest.WebApi.Models;

public class TagInputModelTests
{
    private readonly TagInputModel _tagInputModel;

    public TagInputModelTests()
    {
        _tagInputModel = new TagInputModel("Test Tag");
    }

    [Fact]
    public void InputToDto_ShouldReturnCorrectTagDto()
    {
        var tagDto = _tagInputModel.InputToDto();

        tagDto.Should().NotBeNull();
        tagDto.Name.Should().Be(_tagInputModel.Name);
    }

    [Fact]
    public void InputToDto_WithTagDtoParameter_ShouldUpdateTagDtoCorrectly()
    {
        var id = Guid.NewGuid();
        var name = "Original Tag";

        var tagDto = new TagDto(id, name, []);

        _tagInputModel.InputToDto(tagDto);

        tagDto.Should().NotBeNull();
        tagDto.Name.Should().Be(_tagInputModel.Name);
    }

    [Fact]
    public void Name_ShouldHaveRequiredAttribute()
    {
        var propertyInfo = typeof(TagInputModel).GetProperty(nameof(TagInputModel.Name));

        propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }
}