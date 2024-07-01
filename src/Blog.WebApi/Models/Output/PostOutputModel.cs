using Blog.Application;

namespace Blog.WebApi;

public class PostOutputModel
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public IEnumerable<CommentDto> Comments { get; set; } = [];
    public IEnumerable<TagDto> Tags { get; set; } = [];
    public UserSummaryDto User { get; set; } = null!;

    public PostOutputModel(){ }

    public PostOutputModel(PostDto post)
    {
        Id = post.Id;
        CreatedTime = post.CreatedTime;
        ModifiedTime = post.ModifiedTime;
        Title = post.Title;
        Content = post.Content;
        Comments = post.Comments;
        Tags = post.Tags;
        User = post.User;
    }

    public static IEnumerable<PostOutputModel> MapRange(IEnumerable<PostDto> postsDto)
    {
        List<PostOutputModel> postsOutput = [];
        foreach (var postDto in postsDto)
            postsOutput.Add(new PostOutputModel(postDto));
        
        return postsOutput;
    }
}
