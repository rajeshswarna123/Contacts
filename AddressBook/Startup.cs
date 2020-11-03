using AddressBook.API.Filters;
using AddressBook.Core.Common.AutoMapperProfile;
using AddressBook.Core.Common.Core;
using AddressBook.Core.Common.JwtHelper;
using AddressBook.Data.Contracts;
using AddressBook.Data.Respositories;
using AddressBook.Domain.Contracts;
using AddressBook.Domain.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace AddressBook
{
    public class Startup
    {
        /// <summary>
        /// The secret key.
        /// </summary>
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure

        /// <summary>
        /// The signing key.
        /// </summary>
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            this.ConfigureSwagger(services);

            this.ConfigureAuth(services);

            this.ConfigureSettings(services);

            this.ConfigureIOC(services);

            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseCookiePolicy();
            
            app.UseAuthentication();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddressBook API - V1");
                c.SwaggerEndpoint("/swagger/public/swagger.json", "AddressBook API - Public");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureSettings(IServiceCollection services)
        {

            services.Configure<MongoApiDatabaseSettings>(
                this.Configuration.GetSection(nameof(MongoApiDatabaseSettings)));

            services.AddSingleton<IMongoApiDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoApiDatabaseSettings>>().Value);

            services.Configure<JwtIssuerOptions>(
                this.Configuration.GetSection(nameof(JwtIssuerOptions)));

            services.AddSingleton(r => r.GetRequiredService<IOptions<JwtIssuerOptions>>().Value);

        }

        public void ConfigureSwagger(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AddressBook API", Version = "V1" });

                c.SwaggerDoc("public", new OpenApiInfo { Title = "AddressBook API - Public", Version = "Public" });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer "
                });


                c.OperationFilter<SecurityRequirementsOperationFilter>();
                //c.OperationFilter<AddHeaderOperationFilter>("TenantId", "", false);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }


        public void ConfigureAuth(IServiceCollection services)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = this.Configuration["JwtIssuerOptions:Issuer"],

                ValidateAudience = true,
                ValidAudience = this.Configuration["JwtIssuerOptions:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = this._signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = this.Configuration["JwtIssuerOptions:Issuer"];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.HttpOnly = HttpOnlyPolicy.None;
            });
        }

        public void ConfigureIOC(IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<EnsureUserLoggedIn>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IContactService, ContactService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
        }
    }
}
