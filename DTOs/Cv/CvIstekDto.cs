using UniCareer.SimpleAPI.Models;

namespace UniCareer.SimpleAPI.DTOs.Cv
{
    /// <summary>
    /// CV oluşturma/güncelleme isteği.
    /// DTO → veritabanı tablosu DEĞİLDİR; sadece HTTP istek gövdesinde taşınır.
    /// Child alanlar (Deneyim, Eğitim vb.) entity sınıflarını yeniden kullanır.
    /// </summary>
    public class CvIstekDto
    {
        public string AdSoyad { get; set; } = string.Empty;
        public string Unvan { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public string Özet { get; set; } = string.Empty;

        public List<Deneyim> Deneyim { get; set; } = new();
        public List<Eğitim> Eğitim { get; set; } = new();
        public List<string> Yetenekler { get; set; } = new();
        public List<Projeler> Projeler { get; set; } = new();
        public List<Sertifikalar> Sertifikalar { get; set; } = new();
    }
}
