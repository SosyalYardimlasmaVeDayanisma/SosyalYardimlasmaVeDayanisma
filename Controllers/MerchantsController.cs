using Microsoft.AspNetCore.Mvc;
using AfetPuan.Services;
using AfetPuan.Models;

namespace AfetPuan.Controllers;

[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class MerchantsController : Controller
{
    private readonly IMerchantService _merchantService;

    public MerchantsController(IMerchantService merchantService)
    {
        _merchantService = merchantService;
    }

    public IActionResult Index(MerchantType? type)
    {
        if (type.HasValue)
        {
            var merchants = _merchantService.GetMerchantsByType(type.Value);
            ViewBag.SelectedType = type.Value;
            return View(merchants);
        }
        
        var groupedMerchants = _merchantService.GetMerchantsGroupedByType();
        return View("Index", groupedMerchants);
    }
}



