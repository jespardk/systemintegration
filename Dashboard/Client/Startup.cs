using Client.Middleware;
using Domain.Caching;
using Domain.Configuration;
using Domain.DanishEnergyPrices;
using Domain.KafkaBroker;
using Domain.PowerMeasurement;
using Domain.TemperatureReporting;
using Domain.WeatherForecast;
using Infrastructure.Caching;
using Infrastructure.Configuration;

namespace Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<ICacheService, InMemoryCache>();
            services.AddSingleton<IConfigurationRetriever, ConfigurationRetriever>();
            services.AddSingleton<KafkaBroker>();

            services.AddTransient<PowerMeasurementRetriever>();
            services.AddTransient<WeatherForecastRetriever>();
            services.AddTransient<TemperatureReporter>();
            services.AddTransient<DanishEnergyPriceRetriever>();
            services.AddTransient<IncomingDanishEnergyPriceHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            app.BootKafkaConsumer();
            app.StartSubscriptions();
        }
    }
}
