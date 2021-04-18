using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using AutoMapper;
using KittenFlipper.Contracts;
using KittenFlipper.Helpers;
using KittenFlipper.Infrastructure;
using KittenFlipper.Infrastructure.Jwt;
using KittenFlipper.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;

namespace KittenFlipper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataBaseContext>();
            
            services.AddCors();

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddSingleton(token);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = token.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidAudience = token.Audience,
                    ValidateAudience = false
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Kitten Flipper Api",
                    Description = "Kitten Flipper with JWT Auth APIs and Basic Auth APIs",
                    Contact = new OpenApiContact
                    {
                        Name = @"Hiba Debibi"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);

                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                // add Basic Authentication
                var basicSecurityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
                };
                c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {basicSecurityScheme, new string[] { }}
                });
            });

            //HTTP Client Factory
            services.AddHttpClient(Constants.Constants.HttpClient)
                  .ConfigureHttpClient(i => { i.BaseAddress = new Uri(Configuration[Constants.Constants.BaseAddress]); })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            ConfigureDependencyInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.DocumentTitle = "Kitten Flipper API";
                c.DefaultModelsExpandDepth(0);
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Add dependency injections
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            services.AddTransient<IImageHandler, ImageHandler>();
            services.AddTransient<IImageLoader, ImageLoader>();
            services.AddTransient<IKittenFlipperService, KittenFlipperService>();
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              // Handle HttpRequestExceptions, 408 and 5xx status codes
              .HandleTransientHttpError()
              // Handle 404 not found
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              // Handle 401 Unauthorized
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              // Handle 429 Too many requests
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
              // What to do if any of the above erros occur:
              // Retry 3 times, each time wait 1,2 and 4 seconds before retrying.
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            //Further external requests are blocked for 30 seconds if five failed attempts occur sequentially.
            //Circuit breaker policies are stateful.All calls through this client share the same circuit state.
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}

