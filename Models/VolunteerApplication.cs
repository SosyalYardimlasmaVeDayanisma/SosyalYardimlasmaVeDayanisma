using System.ComponentModel.DataAnnotations;

namespace AfetPuan.Models
{
    public enum ApplicationStatus
    {
        Pending,    // Beklemede
        Approved,   // Onaylandı
        Rejected    // Reddedildi
    }

    public class VolunteerApplication
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "İl seçimi zorunludur")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gönüllülük alanı seçimi zorunludur")]
        public string VolunteerArea { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public string? AdminNotes { get; set; }
    }
}
