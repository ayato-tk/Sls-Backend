using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sales.Core.Extensions;
using Sales.Infra.Data.Interfaces;
using Sales.Infra.Data.Repositories;
using Sales.Infra.IoC;
using Sales.RabbitMQ.Client;

namespace Sales.Api;

public class Startup
{

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {


        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                    Configuration["JwtOptions:SecretKey"] ?? throw new Exception("Secret key is not defined.")
                    )),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddControllers();

        services.AddRabbitClient(Configuration);

        services.AddInfra();

        services.AddHttpClient<IProjectRepository, ProjectRepository>();

        services.AddEndpointsApiExplorer();

        services.AddAutoMapperConfig();

        services.AddHttpContextAccessor();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales Backend", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor utiliza Bearer <TOKEN>",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            services.AddLogging(logging =>
            {
                logging.AddFilter("Microsoft.AspNetCore.Authorization", LogLevel.Debug);
                logging.AddConsole();
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
            }
            );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }
}
