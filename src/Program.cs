using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleStatusIcons.Models;

namespace SimpleStatusIcons {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            ConfigureServices().GetService<StatusIconHandler>().Run();

            while (true) {
                Thread.Sleep(120000);
            }
        }

        private static IServiceProvider ConfigureServices() {
            var services = new ServiceCollection();

            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            services.AddOptions();
            services.Configure<AppSettings>(configuration);

            // configure logging
            services.AddLogging(builder =>
                builder
                    .AddDebug()
                    .AddConsole()
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .SetMinimumLevel(LogLevel.Information)
            );

            // find all public class's that implement IconHandler & IconCondition
            // add to the service collection
            List<Type> handlersAndValidators = new List<Type>();
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (var t in assembly.GetTypes()) {
                if (t.IsClass && !t.IsAbstract) {
                    if (t.GetInterfaces().Any(z => z == typeof(IconHandler)) || t.GetInterfaces().Any(z => z == typeof(IconValidator))) {
                        services.AddSingleton(t);
                        handlersAndValidators.Add(t);
                    }
                }
            }

            // build service collection so we can get access to validators
            var serviceProvider = services.BuildServiceProvider();
            List<IconValidator> validators = new List<IconValidator>();
            foreach (var t in handlersAndValidators) {
                if (t.IsClass && !t.IsAbstract) {
                    if (t.GetInterfaces().Any(z => z == typeof(IconValidator))) {
                        validators.Add(serviceProvider.GetService(t) as IconValidator);
                    }
                }
            }

            // add IconDisplayContext & IconConditionFactory
            services.AddSingleton<IconDisplayContext>();
            services.AddSingleton(new IconValidatorFactory(validators));
            serviceProvider = services.BuildServiceProvider();

            List<IconHandler> handlers = new List<IconHandler>();
            foreach (var t in handlersAndValidators) {
                if (t.IsClass && !t.IsAbstract) {
                    if (t.GetInterfaces().Any(z => z == typeof(IconHandler))) {
                        handlers.Add(serviceProvider.GetService(t) as IconHandler);
                    }
                }
            }

            services.AddSingleton(new IconHandlerFactory(handlers));
            services.AddSingleton<StatusIconHandler>();

            return services.BuildServiceProvider();
        }
    }
}
