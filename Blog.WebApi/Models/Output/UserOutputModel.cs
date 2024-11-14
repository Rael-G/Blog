using Blog.Application;

namespace Blog.WebApi;

public class UserOutputModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public IEnumerable<string>? Roles { get; set; } = null;
    public IEnumerable<PostDto> Posts{ get; set; } = [];

    public UserOutputModel(){ }

    public UserOutputModel(UserDto user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles;
        Posts = user.Posts;
    }

    public static IEnumerable<UserOutputModel> MapRange(IEnumerable<UserDto> usersDto)
    {
        List<UserOutputModel> usersOutput = [];
        foreach (var userDto in usersDto)
        {
            usersOutput.Add(new UserOutputModel(userDto));
        }

        return usersOutput;
    }
}
