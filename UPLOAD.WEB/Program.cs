using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UPLOAD.WEB;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7207/") });
builder.Services.AddScoped<IRepository, Repository>();
//3 formas de injectar
// transient:solo una vez en el ciclo de vida del programa
//singleton: solo lo crea una solo vez pero queda en memoria ( peligroso de usar consumen memoria) general brecha de seguridad
//scoped: cunado quiero que me cre una nueva instancia cada vez que 
///
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();
