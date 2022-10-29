using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.ArmyControllerTests
{
    [Serializable]
    [Collection("Serialize")]
    public class ArmyControllerIntegrationTests : IntegrationTests
    {
        public ArmyControllerIntegrationTests() : base("armyControllerTests")
        {
        }

        [Fact]
        public async Task ArmyController_GetArmies_ArmyDTOArray()
        {
            var response = await _client.GetAsync("api/armies/kingdom/1");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ArmyDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(outputCheck[0].Id, 1);
            Assert.Equal(outputCheck[0].Owner, 1);
            Assert.Equal(outputCheck[0].NumberOfUnitsByLevel.Count(), 2);
            Assert.Equal(outputCheck[0].NumberOfUnitsByLevel[0], 6);
            Assert.Equal(outputCheck[0].NumberOfUnitsByLevel[1], 6);
            Assert.Equal(outputCheck[1].Id, 2);
            Assert.Equal(outputCheck[1].Owner, 1);
            Assert.Equal(outputCheck[1].NumberOfUnitsByLevel.Count(), 0);
        }

        [Fact]
        public async Task ArmyController_GetArmy_Error1()
        {
            var response = await _client.GetAsync("api/armies/-3");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 400);
            Assert.Equal(outputCheck.Error, "Invalid id");
        }

        [Fact]
        public async Task ArmyController_GetArmy_Error2()
        {
            var response = await _client.GetAsync("api/armies/10");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 404);
            Assert.Equal(outputCheck.Error, "Army not found");
        }

        [Fact]
        public async Task ArmyController_GetArmy_ArmyDTO()
        {
            ArmyDTO expected = new ArmyDTO(1, 1, new List<int> { 6, 6 });

            var response = await _client.GetAsync("api/armies/1");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ArmyDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(outputCheck.Id, expected.Id);
            Assert.Equal(outputCheck.Owner, expected.Owner);
            Assert.Equal(outputCheck.NumberOfUnitsByLevel.Count(), expected.NumberOfUnitsByLevel.Count());
            Assert.Equal(outputCheck.NumberOfUnitsByLevel[0], expected.NumberOfUnitsByLevel[0]);
            Assert.Equal(outputCheck.NumberOfUnitsByLevel[1], expected.NumberOfUnitsByLevel[1]);
        }
    }
}
