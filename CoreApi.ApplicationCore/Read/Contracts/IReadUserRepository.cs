using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Read.Users;

namespace CoreApi.ApplicationCore.Read.Contracts
{
    public interface IReadUserRepository
    {
        public Task<bool> UserExistsByEmailAsync(string email);
        public Task<bool> UserExistsByIdAsync(Guid id);
        public Task<Guid?> FindUserIdByEmail(string email);
        public Task<PublicUser?> FindPublicUserById(Guid id);
    }
}