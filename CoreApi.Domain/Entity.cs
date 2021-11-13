namespace CoreApi.Domain
{
    public abstract class Entity<TId>
    {
        public TId Id { get; }
    }
}