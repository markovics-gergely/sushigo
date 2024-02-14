using game.api.Extensions;
using game.api.Hubs;
using game.dal;
using Hangfire;
using Hellang.Middleware.ProblemDetails;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using shared.Api.Extensions;
using shared.dal.Configurations.Interfaces;
using shared.dal.Settings;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<GameDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opt =>
    {
        opt.EnableRetryOnFailure();
    }).EnableSensitiveDataLogging();
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddAutoMapperExtensions();
builder.Services.AddExceptionExtensions();

builder.Services.AddAuthenticationExtensions(configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddRabbitExtensions();
builder.Services.AddServiceExtensions();
builder.Services.AddConfigurations(configuration);
builder.Services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
builder.Services.AddSwaggerExtension(configuration);

builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = configuration.GetConnectionString("Redis");
    options.InstanceName = "localRedis_";
});

builder.Services.AddHangfireExtension(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ICorsPolicyService>((container) => {
    var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
    return new DefaultCorsPolicyService(logger)
    {
        AllowAll = true
    };
});

var app = builder.Build();

builder.Configuration
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json", true, true);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    dbContext.Database.Migrate();
}

app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", configuration.GetValue<string>("IdentityServer:Name"));
        options.OAuthClientId(configuration.GetValue<string>("IdentityServer:ClientId"));
        options.OAuthClientSecret(configuration.GetValue<string>("IdentityServer:ClientSecret"));
        options.OAuthAppName(configuration.GetValue<string>("IdentityServer:Name"));
        options.OAuthUsePkce();
    });
}

var configService = app.Services.GetRequiredService<IFileConfigurationService>();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(configService.GetStaticFilePhysicalPath()),
    RequestPath = $"/{configService.GetStaticFileRequestPath()}"
});

app.UseRouting();
app.UseCors("CorsPolicy");
app.Use(AuthenticationExtension.AuthQueryStringToHeader);
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});
var backgroundJobs = app.Services.GetRequiredService<IBackgroundJobClient>();
backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

app.MapControllers();
app.MapHangfireDashboard();

app.MapHub<GameEventsHub>("/game-hub").RequireCors("CorsPolicy");
app.MapHub<HandEventsHub>("/hand-hub").RequireCors("CorsPolicy");

app.Run();
