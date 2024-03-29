﻿namespace Blog.Application;

public class CommentDto
{
    public Guid? Id { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public Guid PostId { get; set; }

    public CommentDto(string author, string content, Guid postId)
    {
        Author = author;
        Content = content;
        PostId = postId;
    }

    public CommentDto()
    {
        
    }
}
