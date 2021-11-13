using System;
using System.Security.Claims;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace CoreApi.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private UserId? _userId;

        public UserId UserId
        {
            get
            {
                if (_userId is not null)
                {
                    return _userId;
                }
                var id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.UserData);
                if (id is null)
                {
                    throw new ApplicationException("User not authenticated");
                }

                return new UserId(Guid.Parse(id));
            }
            set => _userId = value;
        }
    }
}