using Microsoft.EntityFrameworkCore;
using Eucyon_Tribes.Context;
using Eucyon_Tribes.Services;
using Serilog;
using Eucyon_Tribes.Factories;
using Microsoft.OpenApi.Models;
using Eucyon_Tribes.Extensions;
using Tribes.Services;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
MapSecretsToEnvVariables();

if (env != null && env.Equals("Development"))
{
    builder.Services.AddDbContext<ApplicationContext>(dbBuilder => dbBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
    var logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), autoCreateSqlTable: true, tableName: "Logs")
      .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
}

if (env != null && env.Equals("Production"))
{
    var connectionString = builder.Configuration.GetConnectionString("AzureSql");
    var sb = new SqlConnectionStringBuilder(connectionString);
    sb.UserID = builder.Configuration["AzureUser"];
    sb.Password = builder.Configuration["AzureSqlPassword"];    
    builder.Services.AddDbContext<ApplicationContext>(builder => builder.UseSqlServer(sb.ConnectionString));

    var logger = new LoggerConfiguration()
     .ReadFrom.Configuration(builder.Configuration)
     .Enrich.FromLogContext()
     .WriteTo.MSSqlServer(sb.ConnectionString, autoCreateSqlTable: true, tableName: "Logs")
     .CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
}

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IAuthService, JWTService>();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Eucyon Tribes API",
        Description = "An ASP.NET Core Web API for online game Eucyon Tribes"
    });
});
builder.Services.AddTransient<IBuildingFactory, BuildingFactory>();
builder.Services.AddTransient<IBuildingService, BuildingService>();
builder.Services.AddTransient<IResourceFactory, ResourceFactory>();
builder.Services.AddTransient<IKingdomFactory, KingdomFactory>();
builder.Services.AddTransient<IArmyFactory, ArmyFactory>();
builder.Services.AddTransient<IArmyService, ArmyService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IKingdomService, KingdomService>();
builder.Services.AddTransient<ILeaderboardService, LeaderboardService>();
builder.Services.AddTransient<IPurchaseService, PurchaseService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IWorldService, WorldService>();
builder.Services.AddTransient<IBattleService, BattleService>();
builder.Services.AddHostedService<TimeService>();
builder.Services.AddTransient<RuleService, ConfigRuleService>();
builder.Services.AddTransient<IBattleFactory, BattleFactory>();
builder.Services.AddTransient<ITwoStepAuthService, TwoStepAuthService>();

var app = builder.Build();

app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = String.Empty;
    });
}

app.Run();

static void MapSecretsToEnvVariables()
{
    var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
    foreach (var child in config.GetChildren())
    {
        Environment.SetEnvironmentVariable(child.Key, child.Value);
    }
}

public partial class Program { }