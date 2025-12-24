using System.Threading.Tasks;

using Alethic.Auth0.Operator.Options;

using KubeOps.Operator;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Alethic.Auth0.Operator
{

    public static class Program
    {

        public static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddKubernetesOperator().RegisterComponents();
            builder.Services.AddMemoryCache();
            builder.Services.Configure<OperatorOptions>(builder.Configuration.GetSection("Auth0:Operator"));

            var app = builder.Build();
            app.UseRouting();
            app.MapControllers();
            return app.RunAsync();
        }

    }

}
