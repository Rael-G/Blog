using Blog.Application;
using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.Models.Input
{
    public record CommentInputModel
    {
        [Required]
        [MaxLength(256)]
        public string Author { get; set; }
        [Required]
        [MaxLength(512)]
        public string Content { get; set; }

        public CommentDto InputToDto()
            => new()
            {
                Id = Guid.NewGuid(),
                Author = Author,
                Content = Content
            };

        public void InputToDto(CommentDto commentDto)
            {
                commentDto.Author = Author;
                commentDto.Content = Content;
            }
    }
}
