namespace SociePolar.WebApp.Configuration
{
    /// <summary>
    /// Estructura de configuración para Autenticación SAML 2.0 con Azure AD / Microsoft Entra ID para Empresas Polar.
    /// </summary>
    public class Saml2Settings
    {
        public bool Enabled { get; set; } = false;
        public string? TenantId { get; set; }
        public string EntityId { get; set; } = "https://societarios.empresaspolar.com";
        public string? SingleSignOnDestination { get; set; }
        public string? SingleLogoutDestination { get; set; }
        public string CallbackPath { get; set; } = "/Auth/Saml/Callback";
        public string? SigningCertificate { get; set; }
        public string? CertificateThumbprint { get; set; }
    }
}
