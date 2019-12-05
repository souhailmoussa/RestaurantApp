using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using RestaurantApplication.Api.Configuration;
using RestaurantApplication.Api.Mongo;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace RestaurantApplication.Api.Common
{
    public static class Extension
    {
        public static bool NotEmpty(this string text) => !string.IsNullOrWhiteSpace(text);

        public static string AddIfNotEmpty(this string text, string newText, string prefix = "") =>
            !string.IsNullOrWhiteSpace(newText) ? $"{text}{prefix}{newText}" : text;

        public static bool IsEmpty(this string text) => string.IsNullOrWhiteSpace(text);

        public static bool IsEmptyOrEmptyGuid(this string text) => string.IsNullOrWhiteSpace(text) || Guid.Empty.ToString() == text || !Guid.TryParse(text, out _);

        public static IServiceProvider GetCommonServices(this IServiceCollection services, IConfiguration configuration, string appName)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelAttribute));
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                        .AddJsonOptions(options =>
                        {
                            options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                            options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        })
                        .AddMvcOptions(o => { o.Filters.Add(new HttpGlobalExceptionFilter()); });

            services.AddCors();
            services.AddOptions();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = $"{appName} API", Version = "v1" });
            });

            var module = new ConfigurationModule(configuration);
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            builder.Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }

        public static void ConfigureApp(this IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration, IEnumerable<IConfig> configs)
        {
            foreach (var config in configs)
            {
                config.LoadConfiguration(configuration);
            }

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                OnAppendCookie = options => options.CookieOptions.SameSite = SameSiteMode.None
            });

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                c.InjectOnCompleteJavaScript("/swagger-ui/auth-helper.js");
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.AllowCredentials();
            });

            app.UseMvc();

            app.UseDefaultFiles(new DefaultFilesOptions() { DefaultFileNames = new List<string>() { "index.html" } });
            app.UseStaticFiles();

            app.Run(async (context) =>
            {
                if (!context.Request.Path.Value.ContainsIgnoreCase("api/") &&
                    !context.Request.Path.Value.ContainsIgnoreCase("swagger/") &&
                    !context.Request.Path.Value.ContainsIgnoreCase("docs") &&
                    !(context.Request.Path.Value.Split('/')?.LastOrDefault()?.ContainsIgnoreCase(".") ?? true))
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
                }

            });
        }

        public static IConfiguration GetConfiguration(this IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(GetLocalFolder())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            return builder.Build();
        }

        public static string GetLocalFolder() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static TItem FirstOrDefault<TItem>(this IMongoCollection<TItem> mongoCollection, Expression<Func<TItem, bool>> predicate)
        {
            return mongoCollection.Find(predicate).FirstOrDefault();
        }

        public static Task OnRedirectToIdentityProvider(Microsoft.AspNetCore.Authentication.OpenIdConnect.RedirectContext redirectContext)
        {
            var isAjaxRequest = redirectContext.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            if (isAjaxRequest || (redirectContext.Request.Path.StartsWithSegments("/api")))
            {
                redirectContext.Response.StatusCode = 403;
            }

            return Task.CompletedTask;
        }

        public static bool ContainsIgnoreCase(this string value, string text) =>
            !string.IsNullOrWhiteSpace(value) &&
            !string.IsNullOrWhiteSpace(text) &&
            value.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;

        public static Task OnRedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> redirectContext)
        {
            var isAjaxRequest = redirectContext.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            if (isAjaxRequest)
            {
                redirectContext.HttpContext.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
                redirectContext.HttpContext.Response.Headers["Location"] = CookieAuthenticationDefaults.LoginPath.Value;
            }

            return Task.CompletedTask;
        }

        public static void SetConfigProperties<T>(this T model, T configModel) where T : class
        {
            if (model == null)
            {
                return;
            }

            var modelProperties = model.GetType().GetProperties();

            modelProperties.ForEach(property =>
            {
                var variableValue = property.GetValue(configModel);

                if (variableValue != null)
                {
                    property.SetValue(model, variableValue);
                }
            });
        }

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self != null && self.Any())
            {
                foreach (var item in self)
                {
                    action(item);
                }
            }
        }

        public static void SetEnvironmentProperties<T>(this T model) where T : class
        {
            if (model == null)
            {
                return;
            }

            var typeProperties = model.GetType().GetProperties();

            typeProperties.ForEach(property =>
            {
                var variableValue = Environment.GetEnvironmentVariable($"Ethrai_{property.Name}");

                if (variableValue.NotEmpty())
                {
                    property.SetValue(model, variableValue);
                }
            });
        }

        public static async Task<IEnumerable<T>> GetAllRecords<T>(this MongoService mongoService, string collectionName)
        {
            return await mongoService.GetCollection<T>(collectionName).Find(Builders<T>.Filter.Empty).ToListAsync();
        }
    }
}
