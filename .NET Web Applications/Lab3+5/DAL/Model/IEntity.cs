namespace DAL.Model
{
    // exists to make sure all entities have Id property
    // (it is not required for EF Core proper queries)
    public interface IEntity
    {
        int Id { get; set; }
    }
}