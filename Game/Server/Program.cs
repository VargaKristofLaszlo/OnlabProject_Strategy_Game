﻿using Game.Server.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;

namespace Game.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            (await CreateHostBuilder(args).Build().SeedData()).Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    /*  string uriString = Environment.GetEnvironmentVariable("VaultUri");
                      var keyVaultEndpoint = new Uri(uriString ?? "https://strategygame.vault.azure.net/");
                      config.AddAzureKeyVault(
                      keyVaultEndpoint,
                      new DefaultAzureCredential());*/
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
