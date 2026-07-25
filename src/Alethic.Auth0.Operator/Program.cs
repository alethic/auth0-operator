using System.Threading.Tasks;

using Alethic.Auth0.Operator.Options;
using Alethic.Auth0.Operator.RateLimiting;

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
            builder.Services.AddKubernetesOperator(s =>
            {
                s.ParallelReconciliation.MaxParallelReconciliations = operatorOptions.Reconciliation.MaxParallelReconciliations;

                // KubeOps would otherwise attach every registered finalizer on each reconciliation, including the
                // retired identifiers that stay registered only so entities already carrying them can drain. Finalizer
                // attachment is owned by ControllerBase instead; see EntityFinalizers.
                s.AutoAttachFinalizers = false;
            }).RegisterComponents();
            builder.Services.AddSingleton<Auth0HttpClientProvider>();
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
