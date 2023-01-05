namespace CryptoInvestmentSimulator.Constants
{
    public static class AuthenticationConstants
    {
        // Auth0 configuration and access constants.
        public static readonly string Auth0Domain = "dev-vsd5hzar.eu.auth0.com";
        public static readonly string Auth0ClientId = "qZWlBO51jKx7bdmw4SKJNYW3plzOf8Vw";
        public static readonly string Auth0Scope = "openid profile email";

        // Login process constants.
        public static readonly string LoginRedirect = "/home";

        // Logout process constants.
        public static readonly string LogoutRedirect = "/";
    }
}
