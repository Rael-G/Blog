﻿namespace Blog.Application;

public class PostDto
{
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public IEnumerable<CommentDto> Comments { get; set; }

    public PostDto(Guid id, string title, string content, IEnumerable<CommentDto> comments)
    {
        Id = id;
        Title = title;
        Content = content;
        Comments = comments;
    }
}
