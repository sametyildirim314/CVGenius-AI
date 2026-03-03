using Microsoft.EntityFrameworkCore;
using UniCareer.SimpleAPI.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking; // Gerekli olan namespace

namespace UniCareer.SimpleAPI.Data
{
    public class CvContext : DbContext
    {
        public CvContext(DbContextOptions<CvContext> options) : base(options) { }

        // Tablo Tanımları
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<CvEntity> Cvler { get; set; }
        public DbSet<Deneyim> Deneyimler { get; set; }
        public DbSet<Eğitim> Eğitimler { get; set; }
        public DbSet<Projeler> Projeler { get; set; }
        public DbSet<Sertifikalar> Sertifikalar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- YETENEKLER LİSTESİ İÇİN ÖZEL AYARLAR ---

            // 1. Liste Karşılaştırıcı (ValueComparer) Tanımlama
            // Bu kısım EF Core'a listenin içeriğinin değişip değişmediğini kontrol etmeyi öğretir.
            var yetenekComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2), // İki liste aynı içeriğe mi sahip?
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Değişiklik kontrolü için benzersiz kod üret
                c => c.ToList() // Listenin kopyasını oluştur (Deep Copy)
            );

            // 2. Dönüştürücü ve Karşılaştırıcıyı Uygulama
            modelBuilder.Entity<CvEntity>()
                .Property(e => e.Yetenekler)
                .HasConversion(
                    v => string.Join(',', v), // Veritabanına yazarken: Listeyi virgüllü metne çevir
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() // Okurken: Metni listeye çevir
                )
                .Metadata.SetValueComparer(yetenekComparer); // Az önce tanımladığımız dedektifi buraya bağlıyoruz

            // --- İLİŞKİ TANIMLARI ---

            // 3. Kullanıcı ve CV İlişkisi (One-to-Many)
            modelBuilder.Entity<CvEntity>()
                .HasOne(c => c.Kullanici)
                .WithMany(u => u.Cvler)
                .HasForeignKey(c => c.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}