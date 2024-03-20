using Blog.Application;
using System.ComponentModel.DataAnnotations;

namespace Blog.WebApi.Models.Input
{
    public record CommentInputModel
    {
        [Required]
        [MaxLength(256)]
        public required string Author { get; set; }
        [Required]
        [MaxLength(512)]
        public required string Content { get; set; }

        [Required]
        public required Guid PostId { get; set; }

        public CommentDto InputToDto()
            => new()
            {
                Id = Guid.NewGuid(),
                Author = Author,
                Content = Content,
                PostId = PostId
            };

        public void InputToDto(CommentDto commentDto)
            {
                commentDto.Author = Author;
                commentDto.Content = Content;
                commentDto.PostId = PostId;
            }
    }
}
