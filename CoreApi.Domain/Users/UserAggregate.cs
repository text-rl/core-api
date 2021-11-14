using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreApi.Domain.Users
{
    public record UserId(Guid Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class UserAggregate : Aggregate<UserId>
    {
        public UserAggregate(UserId id, string password, string email, string username) : base(id)
        {
            Password = password;
            Email = email;
            Username = username;
        }

        public string Email { get; private set; }
        public string Username { get; private set; }

        public string Password { get; private set; }

        public void UpdateUser(string? email, string? username, string? password)
        {
            if (email is not null)
            {
                Email = email;
            }

            if (username is not null)
            {
                Username = username;
            }

            if (password is not null)
            {
                Password = password;
            }
        }
    }
}