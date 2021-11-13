using CoreApi.ApplicationCore.Contracts;

namespace CoreApi.Infrastructure.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly int _workFactor;
        public const int WorkFactor = 10;

        public SecurityService(int workFactor = WorkFactor)

        {
            _workFactor = workFactor;
        }

        private string Salt => BCrypt.Net.BCrypt.GenerateSalt(_workFactor);

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, Salt);
        }

        public bool ValidatePassword(string clearPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(clearPassword, hashedPassword);
        }
    }
}