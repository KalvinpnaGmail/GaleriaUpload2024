using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using UPLOAD.WEB;
using UPLOAD.WEB.AuthenticationProviders;
using UPLOAD.WEB.Repositories;
using UPLOAD.WEB.Services;
using UPLOAD.WEB.Servicios;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

///la uri es la que me das Swager 7207
///la uri es la que me das Swager
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7207/") });
//builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("http://192.168.1.100:7207/") });

//formas de inyectar serviios
//scoped: la usamos cuando quiero que cree una nueva instancia cada vez que lo llamo
//Transient:usamos solouna vez se injecta una vez---en el ciclo de vida del program
//Singleton:la primera vez lo crea y lo deja en memoria
builder.Services.AddScoped<IRepository, Repository>();
//3 formas de injectar
// transient:solo una vez en el ciclo de vida del programa
//singleton: solo lo crea una solo vez pero queda en memoria ( peligroso de usar consumen memoria) general brecha de seguridad
//scoped: cunado quiero que me cre una nueva instancia cada vez que
///
builder.Services.AddScoped<IImageService, ImageService>();
//builder.Services.AddSingleton<MenuService>();
builder.Services.AddSweetAlert2();

builder.Services.AddMudServices();
//autorizaciones
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
;

//builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderTest>();

//autorizaciones

await builder.Build().RunAsync();