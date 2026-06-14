namespace UniCareer.SimpleAPI.DTOs.Auth
{
    /// <summary>
    /// Giriş (Login) isteği → POST /api/Auth/giris
    /// </summary>
    public class GirisDto
    {
        public string Email { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
    }
}
