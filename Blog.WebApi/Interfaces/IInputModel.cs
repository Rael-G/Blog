namespace Blog.WebApi;

public interface IInputModel<TDto>
{
    TDto InputToDto();

    void InputToDto(TDto dto);
}
