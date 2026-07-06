using System.Threading.Tasks;

using Alethic.Auth0.Operator.Options;

using KubeOps.Operator;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Alethic.Auth0.Operator
{

    public static class Program
    {

        public static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var operatorOptions = builder.Configuration.GetSection("Auth0:Operator").Get<OperatorOptions>() ?? new OperatorOptions();
            builder.Services.AddKubernetesOperator(s => s.ParallelReconciliation.MaxParallelReconciliations = operatorOptions.Reconciliation.MaxParallelReconciliations).RegisterComponents();
            builder.Services.AddMemoryCache();
            builder.Services.AddRouting();
            builder.Services.AddControllers();
            builder.Services.Configure<OperatorOptions>(builder.Configuration.GetSection("Auth0:Operator"));

            var app = builder.Build();
            app.UseRouting();
            app.MapControllers();
            return app.RunAsync();
        }

    }

}
