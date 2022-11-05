using Auth0.AspNetCore.Authentication;
using CryptoInvestmentSimulator.Constants;

namespace CryptoInvestmentSimulator
{
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Extension method that registers services for builder in Program.cs
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllersWithViews();
            serviceCollection.AddRouting(options => options.LowercaseUrls = true);

            serviceCollection.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = AuthorizationConstants.Auth0Domain;
                options.ClientId = AuthorizationConstants.Auth0ClientId;
                options.Scope = AuthorizationConstants.Auth0Scope;
            });

            return serviceCollection;
        }
    }
}
