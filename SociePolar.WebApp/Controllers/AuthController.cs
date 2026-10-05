using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Authentication;

namespace SociePolar.WebApp.Controllers
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [Route("Saml2")]
    public class AuthController : Controller
    {
        private readonly Saml2Configuration _config;
        private readonly IConfiguration _configuration;

        public AuthController(IOptions<Saml2Configuration> config, IConfiguration configuration)
        {
            _config = config.Value;
            _configuration = configuration;
        }

        // Acción para iniciar el Login (Redirige a Microsoft)
        // URL Local: https://localhost:7090/Saml2/Login
        [Route("Login")]
        public IActionResult Login()
        {
            var binding = new Saml2RedirectBinding();
            // ReturnUrl: A dónde ir después de loguearse exitosamente (ej. Home)
            binding.SetRelayStateQuery(new Dictionary<string, string> { { "ReturnUrl", _configuration["Saml2:SP:ReturnUrl"].ToString() } });
            return binding.Bind(new Saml2AuthnRequest(_config)).ToActionResult();
        }

        // Acción de Callback (ACS): Donde Azure nos devuelve al usuario
        // URL Local: https://localhost:7090/Saml2/Acs
        [Route("Acs")]
        [HttpPost]
        public async Task<IActionResult> AssertionConsumerService()
        {
            var returnUrl = "/";
            var binding = new Saml2PostBinding();
            var saml2AuthnResponse = new Saml2AuthnResponse(_config);

            if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
            {
                throw new AuthenticationException($"Error en Login SAML. Estado: {saml2AuthnResponse.Status}");
            }
            try
            {
                // 1. Leer y validar la respuesta SAML (Firma, certificado, tiempo)
                binding.ReadSamlResponse(Request.ToGenericHttpRequest(), saml2AuthnResponse);

                // 2. Desempaquetar la identidad y crear la sesión en .NET
                binding.Unbind(Request.ToGenericHttpRequest(), saml2AuthnResponse);

                // Esto crea la cookie de autenticación de ASP.NET Core automáticamente
                await saml2AuthnResponse.CreateSession(HttpContext, claimsTransform: (claimsPrincipal) => claimsPrincipal);

                // 3. Redirigir al usuario al Home
                var relayStateQuery = binding.GetRelayStateQuery();
                returnUrl = relayStateQuery.ContainsKey("ReturnUrl") ? relayStateQuery["ReturnUrl"] : "/";

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR SAML] {ex.Message}");
            }
            return Redirect(returnUrl);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect(Url.Content("~/"));
            }

            // A. DESTRUCCIÓN LOCAL:
            // Esto borra la cookie del navegador. A partir de aquí, para tu App el usuario ya no existe.
            await HttpContext.SignOutAsync();

            // B. DESTRUCCIÓN REMOTA (SAML SLO):
            // Esto crea una petición oficial de "Logout" y envía al usuario a Azure para que cierre sesión allá también.
            var binding = new Saml2RedirectBinding();
            // 'User' contiene los claims (NameID) necesarios para que Azure sepa a quién desconectar.
            var logoutRequest = new Saml2LogoutRequest(_config, User);

            return binding.Bind(logoutRequest).ToActionResult();
        }
    }
}
