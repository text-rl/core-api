using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreApi.ApplicationCore.Contracts
{
    public interface IAggregateRepository<TId, TAggregate>
    {
        public Task<IEnumerable<TAggregate>> GetAllAsync();
        public Task<TAggregate?> GetByIdAsync(TId id);
        public Task DeleteAsync(TAggregate aggregate);
        public Task<TId> SetAsync(TAggregate aggregate);
        public Task<TId> GetNewIdAsync();
    }
}