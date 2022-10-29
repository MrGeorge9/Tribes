using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tribes.Tests.SeedData;

namespace TribesTest
{ 
    public class IntegrationTests
    {
        protected readonly HttpClient _client;
        protected readonly IAuthService authService;
        protected readonly IConfiguration configuration;

        protected IntegrationTests(String dataSeed)
        {
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in configuration.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            authService = new JWTService(configuration);

            var appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(ApplicationContext));
                    services.AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseInMemoryDatabase(databaseName: "IntegrationTests");
                    });
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var appDb = scopedServices.GetRequiredService<ApplicationContext>();
                        appDb.Database.EnsureDeleted();
                        appDb.Database.EnsureCreated();

                        switch (dataSeed)
                        {
                            case "kingdomControllerTest":
                                SeedDataKingdomController.PopulateForKingdomControllerTest(appDb);
                                break;
                                
                            case "buildingControllerTests":
                                BuildingControllerTestsSeedData.PopulateTestData(appDb);
                                break;

                            case "armyControllerTests":
                                SeedDataArmyController.PopulateDataForArmyControllerTest(appDb);
                                break;

                            
                            case "buildingControllerTestsNoResource":
                                BuildingControllerTestsSeedDataNoResources.PopulateTestData(appDb);
                                break;

                            case "userControllerTestWorlds0":
                                UserSeedData.PopulateTestData(appDb, dataSeed);
                                break;

                            case "userControllerTestWorlds1":
                                UserSeedData.PopulateTestData(appDb, dataSeed);
                                break;

                            case "leaderboardControllerWithKingdomsTest":
                                SeedDataLeaderboardController.PopulateTestData(appDb, true);
                                break;

                            case "leaderboardControllerWithoutKingdomsTest":
                                SeedDataLeaderboardController.PopulateTestData(appDb, false);
                                break;

                            case "worldControllerWithoutWorldsTest":
                                SeedDataWorldController.PopulateTestData(appDb, "0");
                                break;

                            case "worldControllerWithOneWorldTest":
                                SeedDataWorldController.PopulateTestData(appDb, "1");
                                break;

                            case "worldControllerWithMultipleWorldsTest":
                                SeedDataWorldController.PopulateTestData(appDb, "3");
                                break;

                            case "emailControllerTests":                                
                                EmailControllerTestSeedData.PopulateTestData(appDb);
                                var user = appDb.Users.FirstOrDefault();
                                user.VerificationToken = authService.GenerateToken(user, "verify");
                                var user1 = appDb.Users.FirstOrDefault(p => p.Id == 2);
                                user1.ForgottenPasswordToken = authService.GenerateToken(user1, "forgotten password");
                                var user2 = appDb.Users.FirstOrDefault(p => p.Id == 3);
                                user2.ForgottenPasswordToken = authService.GenerateToken(user1, "forgotten password");
                                appDb.SaveChanges();
                                break;

                            default:
                                break;
                        }
                    }
                });
            });
            _client = appFactory.CreateClient();
        }
    }
}