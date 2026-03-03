


using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore; 
using UniCareer.SimpleAPI.Data;
using UniCareer.SimpleAPI.Services;



var builder = WebApplication.CreateBuilder(args);

// 1. Controllerları ekle
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Döngüsel referansları tespit et ve durdur
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // İsteğe bağlı: JSON'ın daha okunaklı görünmesini sağlar (Pretty Print)
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddDbContext<CvContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2 OpenAPI ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//builder.Services.AddDbContext<CvContext>(options =>
  //  options.UseInMemoryDatabase("GeciciCvDB"));

// 4. PDF Servisini Tanıt
builder.Services.AddScoped<PdfService>();

// ✅ CORS SERVİSİNİ EKLE
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactIzin", policy =>
    {
        policy.AllowAnyOrigin() // Herhangi bir kaynaktan (origin) istek kabul et
              .AllowAnyMethod()                     // GET, POST, PUT, DELETE...
              .AllowAnyHeader();                    // Authorization, Content-Type...
    });
});


var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Eğer Scalar paketini yüklemediysen burayı silebilirsin veya
    // app.UseSwaggerUI(); kullanabilirsin.
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
// ✅ CORS'U AKTİF ET (UseAuthorization'dan ÖNCE olmalı)
app.UseCors("ReactIzin");
app.UseAuthorization();
app.MapControllers();



app.Run();