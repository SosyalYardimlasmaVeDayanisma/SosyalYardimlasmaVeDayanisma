using SosyalYardim.Models;

namespace SosyalYardim.Services;

public interface IImpactVerificationService
{
    Task<ImpactVerification?> GetVerificationAsync(string entityId, string entityType);
    Task<List<ImpactVerification>> GetVerificationsAsync(string entityType);
    Task CreateVerificationAsync(ImpactVerification verification);
    Task UpdateVerificationAsync(ImpactVerification verification);
    Task DeleteVerificationAsync(string verificationId);
    Task<bool> IsVerifiedAsync(string entityId, string entityType);
}
