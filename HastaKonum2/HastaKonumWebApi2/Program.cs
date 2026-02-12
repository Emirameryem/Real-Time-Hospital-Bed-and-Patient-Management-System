using HastaKonumWebApi2.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Veritaban� ba�lant�s� ekle ---
builder.Services.AddDbContext<AppDbContext>();

// --- 2. Controller servislerini ekle ---
builder.Services.AddControllers();

// --- CORS ayarları (WebUI'dan istekler için) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebUI", policy =>
    {
        policy.WithOrigins("http://localhost:5200", "https://localhost:7299")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// --- 3. Authentication (Kimlik Do�rulama) ayarlar� ---
builder.Services.AddAuthentication(options =>
{
    // JWT Bearer standart�n� kullan
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Token do�rulama ayarlar�
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Token'�n issuer kontrol�
        ValidateAudience = true, // Token'�n audience kontrol�
        ValidateLifetime = true, // Token s�resi kontrol�
        ValidateIssuerSigningKey = true, // �mza anahtar� do�rulamas�

        ValidIssuer = builder.Configuration["Jwt:Issuer"], // appsettings.json'dan
        ValidAudience = builder.Configuration["Jwt:Audience"], // appsettings.json'dan
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Gizli anahtar
    };
});

// --- 4. Swagger ayarlar� ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HastaKonum API",
        Version = "v1",
        Description = "Hasta yatak konum izleme sistemi API"
    });

    // Swagger'da JWT Authentication'� tan�mla
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. �rnek: 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// --- 5. Middleware ---

// Geli�tirme ortam�ndaysa Swagger'� aktif et
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS'u UseRouting'den önce ekle
app.UseCors("AllowWebUI");

// **Burada �nemli: Authentication �nce gelmeli**
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

