using AfetPuan.Models;

namespace AfetPuan.Services;

public interface IMerchantService
{
    List<Merchant> GetAllMerchants();
    List<Merchant> GetMerchantsByType(MerchantType type);
    Dictionary<MerchantType, List<Merchant>> GetMerchantsGroupedByType();
}



