using System.Text.Json.Serialization;

namespace UniCareer.SimpleAPI.Models
{
    
    public sealed class Kullanici
    {
        public Kullanici(){
            Cvler = new List<CvEntity>();
        }
        // Birincil anahtar (Primary Key). Her kullanıcıya benzersiz Id atanır.
        public Id { get; set; }

        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;

        // Login için kullanılacak e-posta. Kayıt sırasında benzersiz olmalı.
        public string Email { get; set; } = string.Empty;

        // --- LOGIN ALANLARI (Migration ile Kullanicilar tablosuna eklenecek) ---

        // BCrypt ile hash'lenmiş şifre. Düz metin şifre ASLA buraya yazılmaz.
        [JsonIgnore] // API yanıtında şifre hash'i dışarı sızmamalı
        public string PasswordHash { get; set; } = string.Empty;

        // Yetkilendirme: "User" veya "Admin". Varsayılan "User".
        public string Rol { get; set; } = "User";

        // Hesabın oluşturulma tarihi.
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        // Navigation: One-to-Many → bir kullanıcının birden fazla CV'si olabilir.
        [JsonIgnore] // JSON serileştirmede sonsuz döngüyü önler
        public List<CvEntity> Cvler { get; set; } = new();
    }
}
