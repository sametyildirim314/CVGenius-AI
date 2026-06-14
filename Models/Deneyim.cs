namespace UniCareer.SimpleAPI.Models
{
    /// <summary>
    /// İş deneyimi kaydı. CvEntity ile One-to-Many ilişkilidir → Deneyimler tablosu.
    /// </summary>
    public class Deneyim
    {
        public int Id { get; set; }
        public string Pozisyon { get; set; } = string.Empty;
        public string Sirket { get; set; } = string.Empty;
        public string BaslangicTarihi { get; set; } = string.Empty;
        public string BitisTarihi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
    }
}
