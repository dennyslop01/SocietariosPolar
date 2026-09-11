using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SociePolar.Application.Interfaces;
using SociePolar.Infrastructure.DataContext;
using SociePolar.Infrastructure.Repositories;
using SociePolar.Infrastructure.Services;
using SociePolar.WebApp.Components;
using SociePolar.WebApp.Services;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<IGoogleDrive>(builder.Configuration.GetSection("GoogleDrive"));
builder.Services.Configure<SociePolar.WebApp.Configuration.Saml2Settings>(builder.Configuration.GetSection("Saml2"));

// =========================================================================
// CONFIGURACIÓN SAML 2.0 AZURE AD / ENTRA ID (PREPARADA Y LISTA PARA ACTIVAR)
// =========================================================================
// Para activar SAML 2.0 en producción:
// 1. Establecer "Saml2:Enabled": true en appsettings.json con el Certificado X.509 de Azure y URLs.
// 2. Descomentar el bloque AddCookie() + AddSaml2() o usar el condicional.
/*
var samlSettings = builder.Configuration.GetSection("Saml2").Get<SociePolar.WebApp.Configuration.Saml2Settings>();
if (samlSettings != null && samlSettings.Enabled)
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "Saml2";
    })
    .AddCookie(options =>
    {
        options.Cookie.Name = "SocietariosPolar.AuthCookie";
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
    // .AddSaml2(options => {
    //     options.SPOptions.EntityId = new Sustainsys.Saml2.Metadata.EntityId(samlSettings.EntityId);
    //     options.SPOptions.ReturnUrl = new Uri("/authsessionuser", UriKind.Relative);
    //     var idp = new Sustainsys.Saml2.IdentityProvider(new Sustainsys.Saml2.Metadata.EntityId(samlSettings.SingleSignOnDestination!), options.SPOptions)
    //     {
    //         SingleSignOnServiceUrl = new Uri(samlSettings.SingleSignOnDestination!),
    //         Binding = Sustainsys.Saml2.WebSso.Saml2BindingType.HttpRedirect
    //     };
    //     if (!string.IsNullOrEmpty(samlSettings.SigningCertificate))
    //     {
    //         var certBytes = Convert.FromBase64String(samlSettings.SigningCertificate.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Trim());
    //         idp.SigningKeys.AddConfiguredKey(new System.Security.Cryptography.X509Certificates.X509Certificate2(certBytes));
    //     }
    //     options.IdentityProviders.Add(idp);
    // });
}
*/

// AUTENTICACIÓN ACTIVA ACTUAL (Windows Negotiate / Local Dev Fallback)
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // options.FallbackPolicy = options.DefaultPolicy; // DEV BYPASS
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
builder.Services.AddScoped<IAuditoria, AuditoriaRepository>();
builder.Services.AddScoped<IConciliacion, ConciliacionRepository>();
builder.Services.AddScoped<GoogleDriveService>();
builder.Services.AddScoped<PdfReportService>();

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
QuestPDF.Settings.License = LicenseType.Community;


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


