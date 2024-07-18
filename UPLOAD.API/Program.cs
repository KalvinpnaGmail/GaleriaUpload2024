using Microsoft.EntityFrameworkCore;
using UPLOAD.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));

//app.UseCors(builder =>
//{
//    builder.AllowAnyOrigin()
//           .AllowAnyHeader()
//           .AllowAnyMethod();
//});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

///para que me habilite las peticiones
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.Run();
