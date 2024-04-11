namespace Blog.Application;


public interface ITagService : IBaseService<TagDto>
{
    Task<TagDto?> GetTagPage(Guid id, int page);
    Task<int> GetPageCount(Guid id);
}