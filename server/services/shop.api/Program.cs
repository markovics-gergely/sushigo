using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using shared.Api.Extensions;
using shared.dal.Configurations.Interfaces;
using shop.api.Extensions;
using shop.dal;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add database context from connection string
builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opt =>
    {
        opt.EnableRetryOnFailure();
    }).EnableSensitiveDataLogging();
});

builder.Services.AddAutoMapperExtensions();
builder.Services.AddExceptionExtensions();

builder.Services.AddAuthenticationExtensions(configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddRabbitExtensions();
builder.Services.AddServiceExtensions();
builder.Services.AddConfigurations(configuration);
builder.Services.Configure<shared.dal.Settings.CacheSettings>(configuration.GetSection("CacheSettings"));
builder.Services.AddSwaggerExtension(configuration);

builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = configuration.GetConnectionString("Redis");
    options.InstanceName = "localRedis_";
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

builder.Configuration
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json", true, true);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
    dbContext.Database.Migrate();
}

app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
    // Add Swagger UI
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

// Add static file storage
var configService = app.Services.GetRequiredService<IFileConfigurationService>();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(configService.GetStaticFilePhysicalPath()),
    RequestPath = $"/{configService.GetStaticFileRequestPath()}"
});

// Add endpoint routing and protection
app.UseRouting();
app.UseCors("CorsPolicy");
app.Use(AuthenticationExtension.AuthQueryStringToHeader);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
