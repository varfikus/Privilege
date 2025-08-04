using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Services;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
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

        app.UseRouting();
        app.UseCors("AllowSpecificOrigin");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<XmlProcessingHub>("/xmlHub");
        });
    }
}