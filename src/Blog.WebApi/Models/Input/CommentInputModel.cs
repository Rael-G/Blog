using Blog.Application;

namespace Blog.WebApi.Models.Input
{
    public record CommentInputModel
    {
        public string Author { get; set; }
        public string Content { get; set; }

        public CommentDto InputToDto(Guid? id = null)
            => new() 
            {
                Id = id ?? Guid.NewGuid(), 
                Author = Author, 
                Content = Content 
            };
    }
}
