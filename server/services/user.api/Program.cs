using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using shared.dal.Configurations.Interfaces;
using System.Text.Json.Serialization;
using user.api.Extensions;
using user.api.Hubs;
using user.dal;
using user.dal.Domain;
using shared.Api.Extensions;
using shared.dal.Settings;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opt =>
    {
        opt.EnableRetryOnFailure();
    }).EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimsFactory>();
builder.Services.AddAutoMapperExtensions();
builder.Services.AddExceptionExtensions();
builder.Services.AddIdentityExtensions(configuration);

builder.Services.AddAuthenticationExtensions(configuration);

builder.Services.AddMediatR(cfgg => { cfgg.RegisterServicesFromAssembly(typeof(Program).Assembly); });
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

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

builder.Configuration
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json", true, true);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
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
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.MapHub<FriendEventsHub>("/friend-hub").RequireCors("CorsPolicy");

app.Run();
