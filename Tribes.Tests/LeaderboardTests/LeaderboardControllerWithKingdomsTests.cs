using Eucyon_Tribes.Models.DTOs.LeaderboardDTOs;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.LeaderboardTests
{
    [Serializable]
    [Collection("Serialize")]
    public class LeaderboardControllerWithKingdomsTests : IntegrationTests
    {
        public LeaderboardControllerWithKingdomsTests() : base("leaderboardControllerWithKingdomsTest")
        {
        }

        [Fact]
        public async void GetLeaderboardByBuildingsTest()
        {
            int[][] expected = new int[][] { new int[] { 10, 6 }, new int[] { 10, 2 }, new int[] { 5, 1 } };
            var response = await _client.GetAsync("api/leaderboard/buildings");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BuildingLeaderboardDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(200, (int)response.StatusCode);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(expected[i], result.Leaderboard[i].BuildingScore);
            }
        }

        [Fact]
        public async void GetLeadeboardBySoldiersTest()
        {
            int[] expected = new int[] { 250, 150, 100 };
            var response = await _client.GetAsync("api/leaderboard/soldiers");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SoldierLeaderboardDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(200, (int)response.StatusCode);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(expected[i], result.Leaderboard[i].SoldierScore);
            }
        }
    }
}
