using Hellang.Middleware.ProblemDetails;
using lobby.api.Extensions;
using lobby.api.Hubs;
using lobby.bll.Settings;
using lobby.dal;
using lobby.dal.Configurations.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<LobbyDbContext>(options =>
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
builder.Services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
builder.Services.AddSwaggerExtension(configuration);

builder.Services.AddSignalR();
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

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
    var dbContext = scope.ServiceProvider.GetRequiredService<LobbyDbContext>();
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

var configService = app.Services.GetRequiredService<ILobbyConfigurationService>();
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

app.MapControllers();

app.MapHub<LobbyListEventsHub>("/lobby-list-hub").RequireCors("CorsPolicy");
app.MapHub<LobbyEventsHub>("/lobby-hub").RequireCors("CorsPolicy");

app.Run();
