

using QuestPDF.Fluent; // Fluent API: PDF oluşturmayı kolaylaştırır
using QuestPDF.Helpers; // Yardımcı sınıflar ve renkler
using QuestPDF.Infrastructure;// PDF yapısını tanımlar
using System.Reflection.PortableExecutable;
using UniCareer.SimpleAPI.Models;
using static System.Net.Mime.MediaTypeNames;

namespace UniCareer.SimpleAPI.Services
{
    public class PdfService
    {
        public byte[] CvOlustur(CvEntity veri) // byte[]: PDF dosyasını temsil eder
        {
            // Lisans ayarı (Ücretsiz)
            QuestPDF.Settings.License = LicenseType.Community;

            // document: PDF belgesini temsil eder
            var document = Document.Create(container => // container: PDF içeriğini tanımlar
            {
                container.Page(page => // page: PDF sayfasını tanımlar
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily(Fonts.Arial));



                    page.Content().Column(col => // İçerik kısmı
                    {
                        // BAŞLIK BİLGİLERİ
                        col.Item().AlignCenter().Text(veri.AdSoyad).FontSize(24).Bold().FontColor(Colors.Blue.Medium);
                        col.Item().AlignCenter().Text(veri.Unvan).FontSize(16).FontColor(Colors.Black);
                        col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        var iletisimListesi = new List<string>
      {
        veri.Email,
        veri.Telefon,
        veri.Adres
    };

                        // Boş (null veya empty) olanları filtrele ve aralarına çizgi koy
                        string iletisimMetni = string.Join("  |  ", iletisimListesi.Where(x => !string.IsNullOrEmpty(x)));

                        col.Item().PaddingTop(5).Text(iletisimMetni)
                              .FontSize(10).FontColor(Colors.Black).AlignCenter();
                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        // ÖZET BİLGİSİ
                        col.Item().PaddingBottom(5).Text("ÖZET").FontSize(14).FontColor(Colors.Blue.Medium).Bold();
                        col.Item().Text(veri.Özet);
                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().PaddingBottom(5).Text("YETENEKLER").FontSize(14).FontColor(Colors.Blue.Medium).Bold();

                        // 1. Listeyi aralarına virgül koyarak tek bir cümleye çevir
                        // Örnek Çıktı: "C#, .NET 8, SQL Server, Docker, Git"
                        string yetenekMetni = string.Join(", ", veri.Yetenekler);

                        // 2. Tek seferde yazdır.
                        // QuestPDF bunu paragraf gibi algılar ve sığmazsa aşağı kaydırır.
                        col.Item().Text(yetenekMetni).FontSize(11).FontColor(Colors.Black);
                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);


                        col.Item().PaddingBottom(10).Text("DENEYİM").FontSize(14).FontColor(Colors.Blue.Medium).Bold();
                        foreach (Deneyim deneyim in veri.Deneyim)
                        {
                            col.Item().Text(deneyim.Pozisyon).Bold().FontSize(11);
                            col.Item().Text(deneyim.BaslangicTarihi + " - " + deneyim.BitisTarihi).Italic().FontSize(10);
                            col.Item().Text(deneyim.Sirket);

                            col.Item().Text(deneyim.Aciklama);
                            col.Item().PaddingBottom(5);
                        }
                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().PaddingBottom(5).Text("EĞİTİM").FontSize(14).FontColor(Colors.Blue.Medium).Bold();
                        foreach (Eğitim eğitim in veri.Eğitim)
                        {
                            col.Item().Text(eğitim.OkulAdi).Bold().FontSize(11);
                            col.Item().Text(eğitim.BaşlangıcTarihi + " - " + eğitim.BitisTarihi).Italic().FontSize(10);

                            col.Item().Text(eğitim.Bolum);
                            col.Item().Text("Ortlama: " + eğitim.Ortlama);



                        }
                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        col.Item().PaddingBottom(5).Text("PROJELER").FontSize(14).FontColor(Colors.Blue.Medium).Bold();
                        foreach (Projeler proje in veri.Projeler)
                        {
                            col.Item().Text(proje.ProjeAdi).Bold().FontSize(11);
                            col.Item().Text(proje.Aciklama);

                        }

                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        col.Item().PaddingBottom(10).Text("SERTİFİKALAR").FontSize(14).FontColor(Colors.Blue.Medium).Bold();
                        foreach (Sertifikalar sertifika in veri.Sertifikalar)
                        {
                            col.Item().Text(sertifika.SertifikaAdi).Bold().FontSize(11);
                            col.Item().Text(sertifika.VerenKurum);
                            col.Item().Text(sertifika.AlisTarihi + " - " + sertifika.BitisTarihi).Italic().FontSize(10);
                            col.Item().Text(sertifika.Aciklama);
                        }
                    });

                    // ALT BİLGİ
                    page.Footer().AlignCenter().Text(x =>
                    {

                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();


                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}

