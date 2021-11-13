namespace CoreApi.Domain
{
    public abstract class Aggregate<TId> : Entity<TId>
    {
        protected Aggregate(TId id) : base(id)
        {
        }
    }
}