using DocumentRepositoryApi.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentRepositoryApi.Services
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            return services.AddTransient<ICompressionService, CompressionService>()
                       .AddTransient<IEncryptionService, EncryptionService>()
                       .AddTransient<IUserService, UserService>()
                       .AddTransient<IAuthService, AuthService>()
                       .AddTransient<IDocumentService, DocumentService>()
                       .AddTransient<IDocumentContentService, DocumentContentService>();
        }
    }
}
