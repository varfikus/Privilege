using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Interfaces;
using PrivilegeAPI.Models;
using PrivilegeAPI.Services;
using System.Text;
using FtpSettings = PrivilegeAPI.Services.FtpSettings;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));
        var jwtSettings = Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["SigningKey"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        services.AddAuthorization();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"))));

        services.AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
        });

        services.AddSignalR();

        services.AddSingleton<FtpSettings>(provider =>
        {
            return new FtpSettings
            {
                Server = Configuration["Ftp:Server"],
                Port = int.Parse(Configuration["Ftp:Port"]),
                Username = Configuration["Ftp:Username"],
                Password = Configuration["Ftp:Password"]
            };
        });

        services.AddSingleton<FtpService>();
        services.AddSingleton<AnswerService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IApplicationService, ApplicationService>();

        services.AddHostedService<XmlListenerService>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("https://localhost:7227", "https://192.168.1.100:7227")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRouting();
        app.UseCors("AllowSpecificOrigin");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<XmlProcessingHub>("/xmlHub");
        });
    }
}