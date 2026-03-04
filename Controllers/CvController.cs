using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniCareer.SimpleAPI.Data;
using UniCareer.SimpleAPI.Models;
using UniCareer.SimpleAPI.Services;

namespace UniCareer.SimpleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CvController : ControllerBase
    {
        private readonly CvContext _context;
        private readonly PdfService _pdfService; 
        

        public CvController(CvContext context, PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

      
        [HttpGet]
        public async Task<IActionResult> TumCvleriGetir()

        { 
            try
            {
                //'Include' kullanarak User bilgisini de çekiyoruz (Eager Loading).
                var cvler = await _context.Cvler
                    .Include(c => c.Kullanici)
                    .OrderByDescending(c => c.OlusturulmaTarihi)
                    .ToListAsync();

                return Ok(cvler);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mesaj = "Listeleme sırasında hata oluştu.", hata = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> CvGuncelle(int id, [FromBody] CvIstekDto guncelVeri)
        {
            try
            {
                // 1. ADIM: Sadece varlığını kontrol et (TAKİP ETME - AsNoTracking)
                // Include falan yapmıyoruz, hafızayı kirletmiyoruz.
                var mevcutCvVarMi = await _context.Cvler.AnyAsync(c => c.Id == id);

                if (!mevcutCvVarMi)
                    return NotFound(new { mesaj = "Güncellenecek CV bulunamadı." });

                // 2. ADIM: DTO'daki verileri tertemiz, yeni bir Entity nesnesine basıyoruz.
                // Bu nesne "yabancı" (Untracked) bir nesnedir.
                var güncellenmişEntity = new CvEntity
                {
                    Id = id, // Mevcut ID'yi veriyoruz ki SQL kimi güncelleyeceğini bilsin
                    KullaniciId = 1, // Bunu gerçek projede token'dan almalısın
                    AdSoyad = guncelVeri.AdSoyad,
                    Unvan = guncelVeri.Unvan,
                    Email = guncelVeri.Email,
                    Telefon = guncelVeri.Telefon,
                    Adres = guncelVeri.Adres,
                    Özet = guncelVeri.Özet,
                    Yetenekler = guncelVeri.Yetenekler ?? new List<string>(),

                    // Alt tabloları DTO'dan direkt alıyoruz
                    Deneyim = guncelVeri.Deneyim ?? new List<Deneyim>(),
                    Eğitim = guncelVeri.Eğitim ?? new List<Eğitim>(),
                    Projeler = guncelVeri.Projeler ?? new List<Projeler>(),
                    Sertifikalar = guncelVeri.Sertifikalar ?? new List<Sertifikalar>()
                };

                // 3. ADIM: EF Core'a talimat ver: "Bu nesneyi ve içindeki her şeyi güncelle!"
                // EF Core burada hafızasında hiçbir eski kayıt (Id=5 gibi) tutmadığı için çakışma yaşamaz.
                _context.Cvler.Update(güncellenmişEntity);

                await _context.SaveChangesAsync();

                return Ok(new { mesaj = "Güncelleme başarıyla tamamlandı!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mesaj = "Hata", detay = ex.Message });
            }
        }
// Tüm Kullanıcıları getir
      
       [HttpGet("kullanicigetir")]
       public async Task<IActionResult> TumKullaniciGetir()
        {


            try
            {
                //'Include' kullanarak User bilgisini de çekiyoruz (Eager Loading).
                var kullanicilar = await _context.Kullanicilar.ToListAsync();

                return Ok(kullanicilar);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mesaj = "Listeleme sırasında hata oluştu.", hata = ex.Message });
            }
        }


        // Kullanıcı oluşturma
        [HttpPost("kullanici/olustur")]
        public async Task<IActionResult> KullaniciOlustur( [FromBody] Kullanici kullanici)
        {

            if (kullanici == null)
            {
                return BadRequest("Kullanıcı bilgisi boş olamaz");
            }

            var yeniKullanici = new Kullanici
            {
                Ad = kullanici.Ad,
                Soyad= kullanici.Soyad,
                Email = kullanici.Email,

            };

            _context.Kullanicilar.Add(yeniKullanici);
            await _context.SaveChangesAsync();

             return Ok(yeniKullanici);
        }


        // CV Oluşturma ve İndirme Endpoint'i
        [HttpPost("kullanici/{kullaniciId}/olustur-ve-indir")]
        public async Task<IActionResult> CvOlusturVeIndir(int kullaniciId, [FromBody] CvIstekDto istek)
        {
            // 1. Sağlam Hata Yönetimi (Robust Error Handling)
            try
            {
                // 2. İş Mantığı Doğrulaması (Validation)
                var kullaniciExists = await _context.Kullanicilar.AnyAsync(u => u.Id == kullaniciId);
                if (!kullaniciExists)
                {
                    return NotFound(new { mesaj = "Hata: Belirtilen kullanıcı sistemde kayıtlı değil." });
                }

                // 3. Mapping: DTO'dan Entity'ye Dönüşüm
                var yeniCv = new CvEntity
                {
                    KullaniciId = kullaniciId, // İlişkiyi burada kuruyoruz
                    AdSoyad = istek.AdSoyad,
                    Unvan = istek.Unvan,
                    Email = istek.Email,
                    Telefon = istek.Telefon,
                    Adres = istek.Adres,
                    Özet = istek.Özet,
                    Yetenekler = istek.Yetenekler ?? new List<string>(),
                    Deneyim = istek.Deneyim ?? new List<Deneyim>(),
                    Eğitim = istek.Eğitim ?? new List<Eğitim>(),
                    Projeler = istek.Projeler ?? new List<Projeler>(),
                    Sertifikalar = istek.Sertifikalar ?? new List<Sertifikalar>()
                };

                // 4. Veritabanı İşlemi (Persistence)
                _context.Cvler.Add(yeniCv);
                await _context.SaveChangesAsync();

                // 5. PDF Üretimi
                var pdfDosyasi = _pdfService.CvOlustur(yeniCv);
                
                if (pdfDosyasi == null || pdfDosyasi.Length == 0)
                {
                    throw new Exception("PDF dosyası oluşturulurken teknik bir hata oluştu.");
                }

                // 6. Dosya Yanıtı
                string temizDosyaAdi = $"cv-{istek.AdSoyad.Replace(" ", "_")}.pdf";
                return File(pdfDosyasi, "application/pdf", temizDosyaAdi);
            }
            catch (DbUpdateException ex)
            {
                // Kök Neden: Veritabanı kısıtlaması ihlali veya bağlantı sorunu
                return StatusCode(500, new { mesaj = "Veritabanına kayıt sırasında bir sorun oluştu.", detay = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                // Kök Neden: Beklenmedik uygulama içi hatalar
                return StatusCode(500, new { mesaj = "İşlem sırasında beklenmedik bir hata oluştu.", hata = ex.Message });
            }
        }
    }
}