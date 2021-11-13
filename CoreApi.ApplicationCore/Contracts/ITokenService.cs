using System;
using System.Threading.Tasks;

namespace CoreApi.ApplicationCore.Contracts
{
    public interface ITokenService
    {
        public Task<string> GenerateUserTokenAsync(Guid userId);
    }
}