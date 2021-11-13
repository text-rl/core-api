using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.ApplicationCore.Write.Contracts;
using CoreApi.Domain.Users;
using CoreApi.Infrastructure.Database;
using CoreApi.Infrastructure.Extensions;
using CoreApi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.Infrastructure.Repository.AggregateRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CoreApiContext _context;

        public UserRepository(CoreApiContext context)
        {
            _context = context;
        }

        async Task<IEnumerable<UserAggregate>> IAggregateRepository<UserId, UserAggregate>.GetAllAsync()
        {
            return await _context.Users
                .Select(u => ToEntity(u))
                .ToListAsync();
        }

        public async Task<UserAggregate?> GetByIdAsync(UserId id)
        {
            User? user = await FindAsync(id);
            return user is null ? null : ToEntity(user);
        }

        private async Task<User?> FindAsync(UserId id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id.Value);
        }

        public async Task DeleteAsync(UserAggregate aggregate)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == aggregate.Id.Value);
            if (user is not null)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
            }
        }


        public Task<UserId> GetNewIdAsync()
        {
            return Task.FromResult(new UserId(Guid.NewGuid()));
        }

        public async Task<UserId> SetAsync(UserAggregate aggregate)
        {
            var user = await ToModel(aggregate);
            _context.Users.Upsert(user);
            await _context.SaveChangesAsync();
            return new UserId(user.Id);
        }

        private async Task<User> ToModel(UserAggregate aggregate)
        {
            User? user = await FindAsync(aggregate.Id) ?? new User();
            user.Email = aggregate.Email;
            user.Password = aggregate.Password;
            user.Username = aggregate.Username;
            user.Id = aggregate.Id.Value;
            return user;
        }

        private static UserAggregate ToEntity(User model)
        {
            return new UserAggregate(
                new UserId(model.Id),
                model.Password,
                model.Email,
                model.Username);
        }
    }
}