using Blog.Application;
using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.Models.Input
{
    public record PostInputModel
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public PostDto InputToDto()
            => new()
            {
                Id = Guid.NewGuid(),
                Title = Title,
                Content = Content,
                Comments = []
            };

        public void InputToDto(PostDto postDto)
        {
            postDto.Title = Title;
            postDto.Content = Content;
        }
    }
}
