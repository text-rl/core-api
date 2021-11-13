using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.Domain.Users;

namespace CoreApi.ApplicationCore.Write.Contracts
{
    public interface IUserRepository : IAggregateRepository<UserId, UserAggregate>
    {
    }
}