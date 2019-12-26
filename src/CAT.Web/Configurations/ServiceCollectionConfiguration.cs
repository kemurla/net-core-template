using System;
using System.Text;
using System.IO;
using System.Reflection;

using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using CAT.Core.Identity;
using CAT.Core.Entities;
using CAT.DataAccess.ORM;
using CAT.Business.Services;
using CAT.Business.Identity;
using CAT.Core.Abstractions.Services;

namespace CAT.Web.Configurations
{
    public static class ServiceCollectionConfiguration
    {
        /// <summary>
        /// Register services into the default ASP.NET Core DI.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IJwtFactory, JwtFactory>();
        }

        /// <summary>
        /// Configures the EF context with a given connection string.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        public static void ConfigurDbContext(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            services
                .AddDbContext<CATDbContext>(options =>
                    options.UseSqlServer(connectionString,
                                         x => x.MigrationsAssembly(typeof(CATDbContext).Assembly.FullName)))
                .AddEntityFrameworkSqlServer();
        }

        /// <summary>
        /// Configures Swagger to include xml comments and adds a security definition.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My awesome API", Version = "v1" });
                c.IncludeXmlComments($"{Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name)}.xml");
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Add 'Bearer {token}' into the field. Don't forget to add the Bearer keywoard before the token.",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        /// <summary>
        /// Adds Identity and JWT configurations.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jwtAppSettingOptions"></param>
        public static void ConfigureIdentity(this IServiceCollection services, IConfigurationSection jwtAppSettingOptions)
        {
            if (jwtAppSettingOptions == null)
                throw new ArgumentNullException(nameof(jwtAppSettingOptions));

            services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<CATDbContext>();

            // NOTE: Don't forget to change the secret in appsettings.json
            var secretKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwtAppSettingOptions["Secret"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });
        }
    }
}
