using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AfetPuan.Models;
using AfetPuan.Services;

namespace AfetPuan.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMerchantController : Controller
    {
        private readonly IMerchantService _merchantService;

        public AdminMerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        public IActionResult Index()
        {
            var merchants = _merchantService.GetAllMerchants();
            return View(merchants);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                // TODO: Implement save logic
                TempData["SuccessMessage"] = "İşletme başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            return View(merchant);
        }

        public IActionResult Edit(string id)
        {
            var merchant = _merchantService.GetAllMerchants().FirstOrDefault(m => m.Id == id);
            if (merchant == null)
            {
                return NotFound();
            }
            return View(merchant);
        }

        [HttpPost]
        public IActionResult Edit(Merchant merchant)
        {
            if (ModelState.IsValid)
            {
                // TODO: Implement update logic
                TempData["SuccessMessage"] = "İşletme başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            return View(merchant);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            // TODO: Implement delete logic
            TempData["SuccessMessage"] = "İşletme başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}
