using Eucyon_Tribes.Models.DTOs.LeaderboardDTOs;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.LeaderboardTests
{
    [Serializable]
    [Collection("Serialize")]
    public class LeaderboardControllerWithoutKingdomsTests : IntegrationTests
    {
        public LeaderboardControllerWithoutKingdomsTests() : base("leaderboardControllerWithoutKingdomsTest")
        {
        }

        [Fact]
        public async void GetLeaderboardByBuildingsWithoutKingdomsTest()
        {
            var response = await _client.GetAsync("api/leaderboard/buildings");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BuildingLeaderboardDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(500, (int)response.StatusCode);
            Assert.Equal(new List<BuildingScoreDTO>(), result.Leaderboard);
        }

        [Fact]
        public async void GetLeadeboardBySoldiersWithoutKingdomTest()
        {
            var response = await _client.GetAsync("api/leaderboard/soldiers");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SoldierLeaderboardDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(500, (int)response.StatusCode);
            Assert.Equal(new List<SoldierScoreDTO>(), result.Leaderboard);
        }
    }
}
