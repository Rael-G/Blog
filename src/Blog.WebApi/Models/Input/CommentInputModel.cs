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

        public CommentInputModel(string author, string content, Guid postId)
        {
            Author = author;
            Content = content;
            PostId = postId;
        }

        public CommentInputModel()
        {

        }

        public CommentDto InputToDto()
            => new(Guid.NewGuid(), Author, Content, PostId);

        public void InputToDto(CommentDto commentDto)
            {
                commentDto.Author = Author;
                commentDto.Content = Content;
                commentDto.PostId = PostId;
            }
    }
}
