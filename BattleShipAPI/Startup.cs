using System;
using Azure.Core.Extensions;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using BattleShipAPI.SignalRHubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BattleShipAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration["ConnectionStrings:Storage:blob"], true);
                builder.AddQueueServiceClient(Configuration["ConnectionStrings:Storage:queue"], true);
            });


//            services.AddCors(options =>
//            {
//                options.AddPolicy("CorsPolicy",
//                    builder =>
//                        builder.WithOrigins("https://battleshipuiapp.azurewebsites.net",
//                                "http://localhost:4200")
//                            .AllowAnyMethod()
//                            .AllowAnyHeader()
//                            .AllowCredentials());
//            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            
            app.UseCors(options => 
                options.WithOrigins("https://battleshipuiapp.azurewebsites.net",
                    "http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("/hub");
            });
        }
    }

    internal static class StartupExtensions
    {
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(
            this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out var serviceUri))
                return builder.AddBlobServiceClient(serviceUri);
            return builder.AddBlobServiceClient(serviceUriOrConnectionString);
        }

        public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(
            this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out var serviceUri))
                return builder.AddQueueServiceClient(serviceUri);
            return builder.AddQueueServiceClient(serviceUriOrConnectionString);
        }
    }
}