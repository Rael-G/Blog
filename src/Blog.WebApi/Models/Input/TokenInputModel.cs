using System.ComponentModel.DataAnnotations;
using Blog.Application;

namespace Blog.WebApi;

public class TokenInputModel : IInputModel<Token>
    {
        [Required(AllowEmptyStrings = false)]
        public string AccessToken { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string RefreshToken { get; set; } = string.Empty;

    public Token InputToDto()
        => new Token() { AccessToken = AccessToken, RefreshToken = RefreshToken};
    

    public void InputToDto(Token token)
    {
        token.AccessToken = AccessToken;
        token.RefreshToken = RefreshToken;
    }
}
