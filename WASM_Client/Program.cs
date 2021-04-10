using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WASM_Client.Services;

namespace WASM_Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IAlertService, AlertService>()
                .AddScoped<IHttpService, HttpService>()

                .AddScoped<ILocalStorageService, LocalStorageService>()
                .AddSingleton<AppState>();


            var baseUrl = new Uri(builder.Configuration["baseUrl"]);
            // configure http client
            builder.Services.AddScoped(x =>
            {
                var baseUrl = new Uri(builder.Configuration["baseUrl"]);
                return new HttpClient() { BaseAddress = baseUrl };
            });

           // builder.Services.AddHttpClient<ITempService, TempService>("DatingAppClient", client => client.BaseAddress = baseUrl);

            builder.Services.AddBlazoredToast();

            var host = builder.Build();

            var accountService = host.Services.GetRequiredService<IAccountService>();
            await accountService.Initialize();

            await host.RunAsync();
        }
    }
}
