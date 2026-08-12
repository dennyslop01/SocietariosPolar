using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SociePolar.Application.Interfaces;
using SociePolar.Infrastructure.DataContext;
using SociePolar.Infrastructure.Repositories;
using SociePolar.Infrastructure.Services;
using SociePolar.WebApp.Components;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<IGoogleDrive>(builder.Configuration.GetSection("GoogleDrive"));

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddDbContextFactory<SociedadDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn"))
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors());

builder.Services.AddDistributedMemoryCache(); // Requerido para almacenar la sesión en memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddMudServices();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<IUnidadNegocio, UnidadNegocioRepository>();
builder.Services.AddScoped<IMoneda, MonedaRepository>();
builder.Services.AddScoped<ISociedad, SociedadRepository>();
builder.Services.AddScoped<IAutoridad, AutoridadRepository>();
builder.Services.AddScoped<IAsamblea, AsambleaRepository>();
builder.Services.AddScoped<ICertificacion, CertificacionRepository>();
builder.Services.AddScoped<ILibroSocietario, LibroSocietarioRepository>();
builder.Services.AddScoped<IAccionista, AccionistaRepository>();
builder.Services.AddScoped<IAccionistaSociedad, AccionistaSociedadRepository>();
builder.Services.AddScoped<ITitulo, TituloRepository>();
builder.Services.AddScoped<ITipoDocumentoSoporte, TipoDocumentoSoporteRepository>();
builder.Services.AddScoped<IDocumentoModulo, DocumentoModuloRepository>();
builder.Services.AddScoped<IDividendoPreliminar, DividendoPreliminarRepository>();
builder.Services.AddScoped<IDividendoDefinitivo, DividendoDefinitivoRepository>();
builder.Services.AddScoped<GoogleDriveService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpClient();

builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.KeepAliveInterval = TimeSpan.FromSeconds(10);
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        options.HandshakeTimeout = TimeSpan.FromSeconds(15);
        options.MaximumReceiveMessageSize = 512 * 1024 * 1024;
    });

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
