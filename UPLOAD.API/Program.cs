using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using UPLOAD.API.Data;
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
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin(); // add the allowed origins
                      });
});



builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"),
        (a) => a.MigrationsAssembly("UPLOAD.API"));
},
ServiceLifetime.Transient);
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountryRepository>();
builder.Services.AddScoped<ICountriesUnitofWork, CountriesUnitOfWork>();
builder.Services.AddScoped<IProvinciasRepository, ProvinciasRepository>();
builder.Services.AddScoped<IProvinciasUnitOfWork, ProvinciasUnitOfWork>();
builder.Services.AddScoped<ICitiesRepository,CitiesRepository>();
builder.Services.AddScoped<ICitiesUnitOfWork, CitiesUnitOfWork>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();
builder.Services.AddScoped<IClinicaService, ClinicaService>();

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();


//scoped: la usamos cuando quiero que cree una nueva instancia cada vez que lo llamo
//Transient:usamos solouna vez se injecta una vez---en el ciclo de vida del program
//Singleton:la primera vez lo crea y lo deja en memoria  
///alimientador base datos trnasiente si no lo usamos nunca que no quede en memoria

builder.Services.AddTransient<AlimentadorBaseDeDatos>();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.Run();
