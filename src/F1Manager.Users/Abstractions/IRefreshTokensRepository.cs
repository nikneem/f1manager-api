using System;
using System.Threading.Tasks;

namespace F1Manager.Users.Abstractions
{
    public interface IRefreshTokensRepository
    {
        Task<Guid> ValidateRefreshToken(string token);
        Task<string> GenerateRefreshToken(Guid userId, string ipAddress);
        Task RevokeAll(Guid userId);
    }
}