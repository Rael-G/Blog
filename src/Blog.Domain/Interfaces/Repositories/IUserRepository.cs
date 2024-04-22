namespace Blog.Domain;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User> GetByUserName(string userName);
}
