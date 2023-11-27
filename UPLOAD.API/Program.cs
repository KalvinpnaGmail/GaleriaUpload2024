using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;
using UPLOAD.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<IApiService, ApiService>();



builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"),
        (a) => a.MigrationsAssembly("UPLOAD.API"));
},
ServiceLifetime.Transient);
builder.Services.AddCors();
var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));

app.UseCors(builder => {
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
