namespace UniCareer.SimpleAPI.DTOs.Auth
{
    /// <summary>
    /// Kayıt (Register) isteği → POST /api/Auth/kayit
    /// Sifre düz metin gelir; AuthService BCrypt ile hash'leyip Kullanici.PasswordHash'e yazar.
    /// </summary>
    public class KayitDto
    {
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Düz metin şifre. Sadece istek anında bellekte tutulur, veritabanına yazılmaz.
        public string Sifre { get; set; } = string.Empty;
    }
}
