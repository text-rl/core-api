﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.ApplicationCore.Read.Contracts;
using CoreApi.ApplicationCore.Write.Contracts;
using CoreApi.Domain.Users;
using MediatR;

namespace Application.Write.Users.LoginUser
{
    public record LoginUserQuery(string Password, string Email) : IRequest<TokenResponse>;

    public class LoginUserHandler : IRequestHandler<LoginUserQuery, TokenResponse>
    {
        private readonly IReadUserRepository _readUserRepository;
        private readonly ISecurityService _securityService;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public LoginUserHandler(ISecurityService securityService, ITokenService tokenService,
            IUserRepository userRepository, IReadUserRepository readUserRepository)
        {
            _securityService = securityService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _readUserRepository = readUserRepository;
        }

        public async Task<TokenResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var userId = await _readUserRepository.FindUserIdByEmail(request.Email);
            if (userId is null)
            {
                throw new ApplicationException($"User {request.Email} not found");
            }
            UserAggregate? user = await _userRepository.GetByIdAsync(new UserId(userId.Value));
            if (user is null)
            {
                throw new ApplicationException($"User {request.Email} not found");
            }
            if (!_securityService.ValidatePassword(request.Password, user.Password))
            {
                throw new ApplicationException("Invalid credentials");
            }

            var token = await _tokenService.GenerateUserTokenAsync(user.Id.Value);

            return new TokenResponse(token);
        }
    }
}