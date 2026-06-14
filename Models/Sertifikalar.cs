namespace UniCareer.SimpleAPI.Models
{
    /// <summary>
    /// Sertifika kaydı. CvEntity ile One-to-Many ilişkilidir → Sertifikalar tablosu.
    /// </summary>
    public class Sertifikalar
    {
        public int Id { get; set; }
        public string SertifikaAdi { get; set; } = string.Empty;
        public string VerenKurum { get; set; } = string.Empty;
        public string AlisTarihi { get; set; } = string.Empty;
        public string BitisTarihi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
    }
}
