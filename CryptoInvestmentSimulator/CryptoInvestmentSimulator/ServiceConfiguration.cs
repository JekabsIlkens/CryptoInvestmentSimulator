using Auth0.AspNetCore.Authentication;
using CryptoInvestmentSimulator.Constants;
using CryptoInvestmentSimulator.Database;

namespace CryptoInvestmentSimulator
{
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Registers required services for program builder.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns>
        /// <see cref="IServiceCollection"/> with registered services.
        /// </returns>
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllersWithViews();
            serviceCollection.AddRouting(options => options.LowercaseUrls = true);

            // Registers Auth0 service with domain, client id and access scopes.
            serviceCollection.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = AuthenticationConstants.Auth0Domain;
                options.ClientId = AuthenticationConstants.Auth0ClientId;
                options.Scope = AuthenticationConstants.Auth0Scope;
            });

            // Registers database context with access key from appsettings.json.
            serviceCollection.Add(new ServiceDescriptor(typeof(DatabaseContext), new DatabaseContext(DatabaseConstants.Access)));

            return serviceCollection;
        }
    }
}
