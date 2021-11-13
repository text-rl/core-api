using System;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Read.Contracts;
using CoreApi.ApplicationCore.Read.Users;
using CoreApi.Infrastructure.Database;
using CoreApi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.Infrastructure.Repository.ReadRepositories
{
    public class ReadUserRepository : IReadUserRepository
    {
        private readonly CoreApiContext _context;

        public ReadUserRepository(CoreApiContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email) is not null;
        }

        public async Task<bool> UserExistsByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id) is not null;
        }

        public async Task<Guid?> FindUserIdByEmail(string email)
        {
            return (await _context.Users.FirstOrDefaultAsync(u => u.Email == email))?.Id;
        }

        public async Task<PublicUser?> FindPublicUserById(Guid id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user is null ? null : ToPublicUser(user);
        }

        private static PublicUser ToPublicUser(User user)
        {
            return new PublicUser(user.Id, user.Username, user.Email);
        }
    }
}