using System;
using System.Collections.Generic;

namespace CoreApi.ApplicationCore.Read.Users
{
    public record PublicUser(Guid Id, string Username, string Email);
}