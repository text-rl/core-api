using System;
using System.Threading;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.ApplicationCore.Read.Contracts;
using CoreApi.ApplicationCore.Write.Contracts;
using CoreApi.Domain.Users;
using MediatR;

namespace CoreApi.ApplicationCore.Write.RegisterUser
{
    public record RegisterUserCommand(string Username, string Password, string Email) : IRequest<string>;

    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IReadUserRepository _readUserRepository;
        private readonly ISecurityService _securityService;
        private readonly IUserRepository _userRepository;

        public RegisterUserHandler(
            ISecurityService securityService, IUserRepository userRepository, IReadUserRepository readUserRepository)
        {
            _securityService = securityService;
            _userRepository = userRepository;
            _readUserRepository = readUserRepository;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var password = _securityService.HashPassword(request.Password);
            var userExists = await _readUserRepository.UserExistsByEmailAsync(request.Email);
            UserId? id = await _userRepository.GetNewIdAsync();
            if (userExists)
            {
                throw new ApplicationException("Username or email already exists");
            }

            var user = new UserAggregate(id, password, request.Email, request.Username);
            await _userRepository.SetAsync(user);
            return id.ToString();
        }
    }
}