namespace UniCareer.SimpleAPI.Models
{
    /// <summary>
    /// Proje kaydı. CvEntity ile One-to-Many ilişkilidir → Projeler tablosu.
    /// </summary>
    public class Projeler
    {
        public int Id { get; set; }
        public string ProjeAdi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
    }
}
