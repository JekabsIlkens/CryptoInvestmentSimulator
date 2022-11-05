using Auth0.AspNetCore.Authentication;
using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;

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
                options.Domain = AuthenticationConstants.Auth0Domain;
                options.ClientId = AuthenticationConstants.Auth0ClientId;
                options.Scope = AuthenticationConstants.Auth0Scope;
            });

            serviceCollection.Add(new ServiceDescriptor(typeof(DatabaseContext), new DatabaseContext(DatabaseConstants.Access)));

            return serviceCollection;
        }
    }
}
