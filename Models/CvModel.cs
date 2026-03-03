using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UniCareer.SimpleAPI.Models
{
    
    /// Sisteme kayıtlı ana kullanıcı. CV'lerin sahibidir.
    
    public class Kullanici
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation Property: Kullanıcının birden fazla CV'si olabilir.
        [JsonIgnore] // Kullanıcıyı çekerken CV'lerini JSON'a dahil etme, döngüyü kır!
        public List<CvEntity> Cvler { get; set; } = new();
    }

  
    public class CvEntity
    {
        public int Id { get; set; }

        // Foreign Key (Fiziksel sütun)
        public int KullaniciId { get; set; }

        // Navigation Property (Kod tarafındaki köprü)
        public Kullanici? Kullanici { get; set; }// İlişkili Kullanıcı Kullanici?: null olabilir.

        public string AdSoyad { get; set; } = string.Empty;
        public string Unvan { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public string Özet { get; set; } = string.Empty;

        // İlişkili Alt Tablolar
        public List<Deneyim> Deneyim { get; set; } = new();
        public List<Eğitim> Eğitim { get; set; } = new();
        public List<Projeler> Projeler { get; set; } = new();
        public List<Sertifikalar> Sertifikalar { get; set; } = new();

        // SQL'de nvarchar(max) olarak saklanacak
        public List<string> Yetenekler { get; set; } = new();

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;
    }

    #region Alt Sınıflar (Child Entities) 
    // Alt tabloları temsil eder.

    public class Deneyim
    {
        public int Id { get; set; }
        public string Pozisyon { get; set; } = string.Empty;
        public string Sirket { get; set; } = string.Empty;
        public string BaslangicTarihi { get; set; } = string.Empty;
        public string BitisTarihi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
    }

    public class Eğitim
    {
        public int Id { get; set; }
        public string OkulAdi { get; set; } = string.Empty;
        public string Bolum { get; set; } = string.Empty;
        public string BaşlangıcTarihi { get; set; } = string.Empty;
        public string BitisTarihi { get; set; } = string.Empty;
        public string Ortlama { get; set; } = string.Empty;
    }

    public class Projeler
    {
        public int Id { get; set; }
        public string ProjeAdi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
    }

    public class Sertifikalar
    {
        public int Id { get; set; }
        public string SertifikaAdi { get; set; } = string.Empty;
        public string VerenKurum { get; set; } = string.Empty;
        public string AlisTarihi { get; set; } = string.Empty;
        public string BitisTarihi { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
    }

    #endregion 


    /// Frontend'den gelecek olan veri kalıbı.

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