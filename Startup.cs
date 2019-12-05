using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestaurantApplication.Api.Common;
using RestaurantApplication.Api.Configuration;

namespace RestaurantApplication.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = env.GetConfiguration();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //var configService = new ConfigService();
            //services.AddSingleton<IConfigService>(configService);

            //var databaseConfig = configService.Get<DatabaseConfig>(Constants.ConfigSections.Database);
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddCookie("Cookies", options =>
              {
                  options.Events.OnRedirectToAccessDenied = Common.Extension.OnRedirectToAccessDenied;
                  options.Events.OnRedirectToLogin = Common.Extension.OnRedirectToAccessDenied;
              });

            return services.GetCommonServices(Configuration, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IEnumerable<IConfig> configs)
        {
            //Console.WriteLine(env.EnvironmentName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.ConfigureApp(env, Configuration, configs);
        }
    }
}
