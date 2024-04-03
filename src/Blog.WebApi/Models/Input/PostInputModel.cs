using Blog.Application;
using Blog.Domain;
using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.Models.Input
{
    public record PostInputModel
    {
        [Required]
        [MaxLength(Post.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public PostInputModel(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public PostDto InputToDto()
            => new(Guid.NewGuid(), Title, Content, [], []);

        public void InputToDto(PostDto postDto)
        {
            postDto.Title = Title;
            postDto.Content = Content;
        }
    }
}