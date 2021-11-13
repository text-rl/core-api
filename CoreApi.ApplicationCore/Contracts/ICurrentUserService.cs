using CoreApi.Domain.Users;

namespace CoreApi.ApplicationCore.Contracts
{
    public interface ICurrentUserService
    {
        public UserId UserId { get; set; }
    }
}