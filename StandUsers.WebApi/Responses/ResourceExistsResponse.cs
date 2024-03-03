namespace StandUsers.WebApi.Responses;

public class ResourceExistsResponse : GenericResponse
{
    public bool Exists { get; set; }

    public static ResourceExistsResponse Build(bool exists, string message)
    {
        return new ResourceExistsResponse
        {
            Exists = exists,
            Message = message
        };
    }
}
