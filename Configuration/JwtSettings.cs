namespace UniCareer.SimpleAPI.Configuration
{
    /// <summary>
    /// appsettings.json içindeki "JwtSettings" bölümünün C# karşılığı.
    ///
    /// Nasıl dolar?
    ///   1. appsettings.json     → Issuer, Audience, ExpireMinutes
    ///   2. User Secrets (dev)   → SecretKey  (GitHub'a gitmez)
    ///   3. Ortam değişkeni      → JwtSettings__SecretKey (production)
    ///
    /// Program.cs'te şu şekilde bağlanır:
    ///   builder.Services.Configure&lt;JwtSettings&gt;(builder.Configuration.GetSection("JwtSettings"));
    ///
    /// AuthService içinde şu şekilde kullanılır:
    ///   public AuthService(IOptions&lt;JwtSettings&gt; jwtOptions) { _jwt = jwtOptions.Value; }
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Configuration section adı. appsettings.json'daki JSON key ile aynı olmalı.
        /// </summary>
        public const string SectionName = "JwtSettings";

        /// <summary>
        /// JWT imzalama anahtarı. En az 32 karakter olmalı.
        /// User Secrets veya ortam değişkeninden gelir; appsettings.json'da tutulmaz.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Token'ı kim üretti? → JWT "iss" (issuer) claim'ine yazılır.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token kime yönelik? → JWT "aud" (audience) claim'ine yazılır.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Token geçerlilik süresi (dakika). Süre dolunca kullanıcı tekrar giriş yapmalı.
        /// </summary>
        public int ExpireMinutes { get; set; } = 60;

        /// <summary>
        /// SecretKey'in yapılandırılıp yapılandırılmadığını kontrol eder.
        /// Uygulama başlarken veya token üretmeden önce çağrılabilir.
        /// </summary>
        public bool GecerliMi()
        {
            return !string.IsNullOrWhiteSpace(SecretKey)
                && SecretKey.Length >= 32
                && !string.IsNullOrWhiteSpace(Issuer)
                && !string.IsNullOrWhiteSpace(Audience)
                && ExpireMinutes > 0;
        }
    }
}
