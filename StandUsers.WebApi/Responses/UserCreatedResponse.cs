namespace StandUsers.WebApi.Responses;

public class UserCreatedResponse : GenericResponse
{
    public Guid Id { get; set; }

    public static UserCreatedResponse Build(Guid id)
    {
        var message = "User Successfully created on the system";
        return new UserCreatedResponse { Id = id, Message = message };
    }
}
