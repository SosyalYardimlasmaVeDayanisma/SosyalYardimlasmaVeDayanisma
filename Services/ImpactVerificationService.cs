using SosyalYardim.Models;
using SosyalYardim.Data;
using Microsoft.EntityFrameworkCore;

namespace SosyalYardim.Services;

public class ImpactVerificationService : IImpactVerificationService
{
    private readonly AppDbContext _context;

    public ImpactVerificationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ImpactVerification?> GetVerificationAsync(string entityId, string entityType)
    {
        return await _context.ImpactVerifications
            .FirstOrDefaultAsync(v => v.EntityId == entityId && v.EntityType == entityType);
    }

    public async Task<List<ImpactVerification>> GetVerificationsAsync(string entityType)
    {
        return await _context.ImpactVerifications
            .Where(v => v.EntityType == entityType)
            .OrderByDescending(v => v.VerificationDate)
            .ToListAsync();
    }

    public async Task CreateVerificationAsync(ImpactVerification verification)
    {
        _context.ImpactVerifications.Add(verification);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVerificationAsync(ImpactVerification verification)
    {
        verification.UpdatedAt = DateTime.Now;
        _context.ImpactVerifications.Update(verification);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVerificationAsync(string verificationId)
    {
        var verification = await _context.ImpactVerifications.FindAsync(verificationId);
        if (verification != null)
        {
            _context.ImpactVerifications.Remove(verification);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsVerifiedAsync(string entityId, string entityType)
    {
        return await _context.ImpactVerifications
            .AnyAsync(v => v.EntityId == entityId && v.EntityType == entityType && v.IsVerified);
    }
}
