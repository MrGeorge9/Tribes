using Eucyon_Tribes.Models.DTOs.BuildingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TribesTest;

namespace Tribes.Tests.BuildingsTests
{
    [Serializable]
    [Collection("Serialize")]
    public class BuildingControllerIntegratitionTestsNoResources : IntegrationTests
    {
        public BuildingControllerIntegratitionTestsNoResources() : base("buildingControllerTestsNoResource")
        {
        }

        [Fact]
        public async Task Store_WithNoResources_ReturnsNoResources()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");
            BuildingRequestDto buildingRequestDto = new BuildingRequestDto(1);

            var response = await _client.PostAsync("https://localhost:7192/api/buildings", JsonContent.Create(buildingRequestDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Insufficient resources", result["error"].ToString());
        }
    }
}
