﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using UPLOAD.API.Data;
using UPLOAD.API.Helpers;
using UPLOAD.API.Repositories.Implementations;
using UPLOAD.API.Repositories.Interfaces;
using UPLOAD.API.Service;
using UPLOAD.API.UnitsOfWork.Implementations;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;

var builder = WebApplication.CreateBuilder(args);

//importan pra que no hayas un ciclo en program agregar
//cuando adiciono los controladores que me igrnore las referencias circulares
builder.Services.
    AddControllers().
    AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//importan pra que no hayas un ciclo en program agregar

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
///modificamos swagger para poder mandarle el swagger y no usar postman
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

///modificamos swagger para poder mandarle el swagger y no usar postman

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.AllowAnyOrigin(); // add the allowed origins
//                      });
//});

//nuevo 25/02/2025

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://192.168.1.100:7183", "https://localhost:7265", "https://192.168.1.10:7265", "https://181.228.28.10") // Permitir solo este origen
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Habilita cookies/autenticación en CORS
    });
});

///
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"),
        (a) => a.MigrationsAssembly("UPLOAD.API"));
    // Solo habilitar durante el desarrollo
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
},
ServiceLifetime.Transient);
//scoped: la usamos cuando quiero que cree una nueva instancia cada vez que lo llamo
//Transient:usamos solouna vez se injecta una vez---en el ciclo de vida del program
//Singleton:la primera vez lo crea y lo deja en memoria
///alimientador base datos trnasiente si no lo usamos nunca que no quede en memoria

builder.Services.AddTransient<AlimentadorBaseDeDatos>();

builder.Services.AddScoped<IFileStorage, FileStorage>();
builder.Services.AddScoped<ImailHelpers, MailHelper>();

builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountryRepository>();
builder.Services.AddScoped<ICountriesUnitofWork, CountriesUnitOfWork>();
builder.Services.AddScoped<IProvinciasRepository, ProvinciasRepository>();
builder.Services.AddScoped<IProvinciasUnitOfWork, ProvinciasUnitOfWork>();
builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();
builder.Services.AddScoped<ICitiesUnitOfWork, CitiesUnitOfWork>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoriesUnitOfWork, CategoriesUnitOfWork>();

builder.Services.AddScoped<ICabeceraImagenesRepository, CabeceraImagenesRepository>();
builder.Services.AddScoped<ICabeceraImagenesUnitOfWork, CabeceraImagenesUnitOfWork>();

//servicio soap acler

builder.Services.AddHttpClient<IApiServiceAcler, ApiServiceAcler>();
// Registrar ApiService en el contenedor de dependencias
//builder.Services.AddHttpClient<ApiService>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    //confirmar registro de usuario
    x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    x.SignIn.RequireConfirmedEmail = true;
    ///confirmar registro de usuario
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
    // mas seguridad al sistema ..si me equivoco 3 veces me bloquea 5 minutos
    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    x.Lockout.MaxFailedAccessAttempts = 3;
    x.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

/////inyectamos para token autenticacion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

// Configuración del logging para consola
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

/////inyectamos para token autenticacion

var app = builder.Build();
///como esta clase no tienen inyectcion lo hacemos manualmente
SeedData(app);

async void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory!.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<AlimentadorBaseDeDatos>();
        await service!.SeedAsync();
    }
}

///como esta clase no tienen inyectcion lo hacemos manualmente
///
/////IMPORTANTE: Asegúrate de llamar a UseCors ANTES de UseAuthentication y UseAuthorization
//nuvevo 25/02/2025
app.UseCors(MyAllowSpecificOrigins);
//app.UseCors(x => x
//    .AllowAnyMethod()
//    .AllowAnyHeader()
//    .SetIsOriginAllowed(origin => true)
//    .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();