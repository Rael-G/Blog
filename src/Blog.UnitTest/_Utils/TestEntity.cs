using Blog.Domain;

namespace Blog.UnitTest;

public class TestEntity(Guid id) : BaseEntity(id) 
{
    public static Guid TestValidateId(Guid id, string parameterName)
        => ValidateId(id, parameterName);

    public static string TestValidateStringNullOrWhiteSpace(string value, string parameterName)
        => ValidateStringNullOrWhiteSpace(value, parameterName);
}