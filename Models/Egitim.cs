namespace UniCareer.SimpleAPI.Models
{
    /// <summary>
    /// Eğitim kaydı. CvEntity ile One-to-Many ilişkilidir → Eğitimler tablosu.
    /// </summary>
    public class Eğitim
    {
        public int Id { get; set; }
        public string OkulAdi { get; set; } = string.Empty;
        public string Bolum { get; set; } = string.Empty;
        public string BaşlangıcTarihi { get; set; } = string.Empty;
        public string BitisTarihi { get; set; } = string.Empty;
        public string Ortlama { get; set; } = string.Empty;
    }
}
