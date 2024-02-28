using Blog.Application;
using Blog.Domain;

namespace Blog.WebApi.Models.Input
{
    public record PostInputModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public PostDto InputToDto(Guid? id = null)
            => new() 
            {
                Id = id ?? Guid.NewGuid(),
                Title = Title, 
                Content = Content,
                Comments = Comments
            };
    }
}
