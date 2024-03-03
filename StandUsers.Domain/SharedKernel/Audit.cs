namespace StandUsers.Domain.SharedKernel;

public class Audit
{
    public DateTime? Created { get; protected set; } = DateTime.UtcNow;
    public DateTime? Updated { get; private set;}

    protected void SetUpdated()
    {
        Updated = DateTime.UtcNow;
    }
}
