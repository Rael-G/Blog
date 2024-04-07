﻿using Blog.Domain;

namespace Blog.UnitTest.Domain.Entities;

public class PostTagTests
{
    [Fact]
    public void PostTag_Initialization_WithValidValues_Success()
    {
        var postId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        var postTag = new PostTag(postId, tagId);

        Assert.Equal(postId, postTag.PostId);
        Assert.Equal(tagId, postTag.TagId);
    }

    [Fact]
    public void PostTag_Initialization_WithInvalidPostId_ShouldThrowArgumentNullException()
    {
        var postId = Guid.Empty;
        var tagId = Guid.NewGuid();

        Assert.Throws<ArgumentNullException>(() => new PostTag(postId, tagId));
    }

    [Fact]
    public void PostTag_Initialization_WithInvalidTagId_ShouldThrowArgumentNullException()
    {
        var postId = Guid.NewGuid();
        var tagId = Guid.Empty;

        Assert.Throws<ArgumentNullException>(() => new PostTag(postId, tagId));
    }
}
