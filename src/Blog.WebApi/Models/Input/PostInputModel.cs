using Blog.Application;
using Blog.Domain;
using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.Models.Input
{
    public record PostInputModel : IInputModel<PostDto>
    {
        [Required]
        [MaxLength(Post.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public IEnumerable<TagDto> Tags { get; set; }

        public PostInputModel(string title, string content, IEnumerable<TagDto> tags)
        {
            Title = title;
            Content = content;
            Tags = tags;
        }

        public PostDto InputToDto()
            => new(Guid.NewGuid(), Title, Content, [], Tags);

        public void InputToDto(PostDto postDto)
        {
            postDto.Title = Title;
            postDto.Content = Content;
        }
    }
}