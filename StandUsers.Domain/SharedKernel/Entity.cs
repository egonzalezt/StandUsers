namespace StandUsers.Domain.SharedKernel;

public class Entity : Audit
{
    protected Entity() { }
    protected Entity(Guid id) : this() 
    {
        Id = id;
    }

    public Guid Id { get; }
}
