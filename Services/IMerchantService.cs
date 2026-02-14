using SosyalYardim.Models;

namespace SosyalYardim.Services;

public interface IMerchantService
{
    List<Merchant> GetAllMerchants();
    List<Merchant> GetMerchantsByType(MerchantType type);
    Dictionary<MerchantType, List<Merchant>> GetMerchantsGroupedByType();
}



