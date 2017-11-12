using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KubTest.EventSourcing;
using KubTest.EventStore.MongoDB;
using KubTest.EventPublisher.RabbitMQ;
using KubTest.Model;

namespace KubTest.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"secrets/appsettings.secrets.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitMQOptions>(Configuration.GetSection("RabbitMQ"));
            services.Configure<MongoDBOptions>(Configuration.GetSection("MongoDB"));

            // Add framework services.
            services.AddMvc();
            services.Configure<RouteOptions>(options => {
                options.ConstraintMap.Add("command", typeof(CommandNameConstraint));
            });

            services.AddTransient<IRepository<Foo>, EventSourceRepository<Foo>>();
            services.AddTransient<IEventPublisher, RabbitMQEventPublisher>();
            services.AddSingleton<IEventStore, MongoDBEventStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
