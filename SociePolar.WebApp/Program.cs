using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using QuestPDF.Infrastructure;
using SociePolar.Application.Interfaces;
using SociePolar.Infrastructure.DataContext;
using SociePolar.Infrastructure.Repositories;
using SociePolar.Infrastructure.Services;
using SociePolar.WebApp.Components;
using SociePolar.WebApp.Services;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseStaticWebAssets();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<IGoogleDrive>(builder.Configuration.GetSection("GoogleDrive"));
builder.Services.Configure<SociePolar.WebApp.Configuration.Saml2Settings>(builder.Configuration.GetSection("Saml2"));


// AUTENTICACIÓN SAML 2.0 AZURE AD / ENTRA ID ACTIVA PARA QA Y PRODUCCIÓN
// builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//     .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // options.FallbackPolicy = options.DefaultPolicy; // DEV BYPASS
});

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddDbContextFactory<SociedadDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn"), sqlServerOptions =>
           {
               sqlServerOptions.CommandTimeout(300); // 5 minutos de margen para operaciones masivas de conciliación
           })
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

builder.Services.Configure<Saml2Configuration>(options =>
{
    var samlConfig = builder.Configuration.GetSection("Saml2");
    var idpConfig = samlConfig.GetSection("IdP");
    var spConfig = samlConfig.GetSection("SP");

    // A. Configuración SP (Nosotros)
    // En ITfoxtec, 'Issuer' es NUESTRO EntityID
    options.Issuer = spConfig["EntityId"].Trim();
    //options.Issuer = "FactorPolar.Webapp"; //options.Issuer = spConfig["EntityId"].Trim();
    //options.Issuer = "FactorPolar.Webapp.Des"; //options.Issuer = spConfig["EntityId"].Trim();

    // B. Configuración IdP (Polar / Azure)
    // 'AllowedIssuer' es el EntityID de ELLOS
    options.AllowedAudienceUris.Add(spConfig["EntityId"].Trim());
    //options.AllowedAudienceUris.Add("FactorPolar.Webapp"); //options.AllowedIssuer = spConfig["EntityId"].Trim();
    //options.AllowedAudienceUris.Add("FactorPolar.Webapp.Des"); //options.AllowedIssuer = spConfig["EntityId"].Trim();

    options.SingleSignOnDestination = new Uri(idpConfig["SingleSignOnDestination"]);
    options.SingleLogoutDestination = new Uri(idpConfig["SingleLogoutDestination"]);

    // Configuración de validación (Importante para evitar errores de certificado en local)
    options.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;// ChainTrust;
    options.RevocationMode = X509RevocationMode.NoCheck;

    // C. Carga del Certificado de Polar
    // Asegúrate de que el archivo "Polar_Azure.cer" esté en la raíz del proyecto  
    try
    {
        // Cargar Certificado Público de Polar
        options.SignatureValidationCertificates.Add(new X509Certificate2("Societarios_Azure.cer"));
    }
    catch (Exception ex)
    {
        // En desarrollo, esto ayuda a saber si no encuentra el archivo .cer
        Console.WriteLine($"ERROR FATAL CARGANDO CERTIFICADO: {ex.Message}");
    }
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => // <--- ESTA LÍNEA ES CRÍTICA
    {
        options.LoginPath = "/Saml2/Login";
        options.AccessDeniedPath = "/Error";
    });


builder.Services.AddSaml2();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();

app.UseSaml2();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


