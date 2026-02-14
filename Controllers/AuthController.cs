using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SosyalYardim.Models;

namespace SosyalYardim.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "E-posta ve şifre gereklidir";
                ViewBag.Email = email;
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "E-posta veya şifre hatalı";
                ViewBag.Email = email;
                return View();
            }
            
            // Lockout kontrolü
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                var remainingMinutes = lockoutEnd.HasValue 
                    ? (int)(lockoutEnd.Value - DateTimeOffset.Now).TotalMinutes + 1
                    : 5;
                ViewBag.Error = $"Hesabınız geçici olarak kilitlendi. Lütfen {remainingMinutes} dakika sonra tekrar deneyin.";
                ViewBag.Email = email;
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: true);
            
            if (result.IsLockedOut)
            {
                ViewBag.Error = "Hesabınız geçici olarak kilitlendi. Lütfen 5 dakika sonra tekrar deneyin.";
                ViewBag.Email = email;
                return View();
            }
            
            if (!result.Succeeded)
            {
                var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                var remainingAttempts = 5 - failedCount;
                
                if (remainingAttempts > 0)
                {
                    ViewBag.Error = $"E-posta veya şifre hatalı. Kalan deneme hakkı: {remainingAttempts}";
                }
                else
                {
                    ViewBag.Error = "E-posta veya şifre hatalı";
                }
                
                ViewBag.Email = email;
                return View();
            }

            // Admin kullanıcısıysa admin paneline yönlendir
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Giriş hatası: {ex.Message}");
            ViewBag.Error = "Giriş işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
            ViewBag.Email = email;
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string name, string email, string password, string confirmPassword)
    {
        try
        {
            // Validation kontrolleri
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Tüm alanlar gereklidir";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Şifreler eşleşmiyor";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }

            if (password.Length < 8)
            {
                ViewBag.Error = "Şifre en az 8 karakter olmalıdır";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }
            
            // Şifre güç kontrolü
            if (!password.Any(char.IsUpper))
            {
                ViewBag.Error = "Şifre en az bir büyük harf içermelidir";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }
            
            if (!password.Any(char.IsLower))
            {
                ViewBag.Error = "Şifre en az bir küçük harf içermelidir";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }
            
            if (!password.Any(char.IsDigit))
            {
                ViewBag.Error = "Şifre en az bir rakam içermelidir";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }
            
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ViewBag.Error = "Şifre en az bir özel karakter içermelidir";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ViewBag.Error = "Bu e-posta adresi zaten kayıtlı";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }

            var user = new User
            {
                UserName = email,
                Email = email,
                FullName = name.Trim(),
                EmailConfirmed = true, // Şimdilik e-posta doğrulaması yok
                CreatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                ViewBag.Error = $"Kayıt başarısız: {errors}";
                ViewBag.Name = name;
                ViewBag.Email = email;
                return View();
            }

            // Yeni kullanıcıyı "User" rolüne ekle
            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Kayıt hatası: {ex.Message}");
            ViewBag.Error = "Kayıt işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
            ViewBag.Name = name;
            ViewBag.Email = email;
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}


