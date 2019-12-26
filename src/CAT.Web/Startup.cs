using CAT.Web.Filters;
using CAT.Core.Identity;
using CAT.Core.Validators;
using CAT.Web.Configurations;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.AngularCli;

using FluentValidation.AspNetCore;

namespace CAT.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigurDbContext(Configuration.GetConnectionString("DbContext"));
            services.ConfigureIdentity(Configuration.GetSection(nameof(JwtIssuerOptions)));
            services.ConfigureServices();
            services.ConfigureSwagger();

            services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services
                .AddMvc(setup =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    setup.Filters.Add(new AuthorizeFilter(policy));
                    setup.Filters.Add(typeof(GlobalExceptionFilter));
                })
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssemblyContaining<RegisterModelValidator>();
                    configuration.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts();
                app.UseSpaStaticFiles();
            }

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // It's important that UseAuthentication() is placed exactly here.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
