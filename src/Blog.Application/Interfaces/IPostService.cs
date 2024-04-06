namespace Blog.Application;

public interface IPostService : IBaseService<PostDto>
{
    new Task Update(PostDto postDto);
}
