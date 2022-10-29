using Eucyon_Tribes.Models.DTOs.BuildingDTOs;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.BuildingsTests
{
    [Serializable]
    [Collection("Serialize")]
    public class BuildingControllerIntegrationTests : IntegrationTests
    {
        public BuildingControllerIntegrationTests() : base("buildingControllerTests")
        {
        }

        [Fact]
        public async Task Index_WithRightUser_ReturnsListOfBuildings()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");
            var response = await _client.GetAsync("https://localhost:7192/api/buildings");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<List<BuildingsResponseDto>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task Index_WithWrongUser_ReturnsWrongUser()
        {
            _client.DefaultRequestHeaders.Add("userId", "5");
            var response = await _client.GetAsync("https://localhost:7192/api/buildings");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Invalid user", result["error"].ToString());
        }

        [Fact]
        public async Task Show_WithRightUser_BuildingsDetails()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");
            var response = await _client.GetAsync("https://localhost:7192/api/buildings/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("Mine", result["type"].ToString());
        }

        [Fact]
        public async Task Show_WithWrongUser_ReturnsWrongUser()
        {
            _client.DefaultRequestHeaders.Add("userId", "6");
            var response = await _client.GetAsync("https://localhost:7192/api/buildings/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Invalid user", result["error"].ToString());
        }

        [Fact]
        public async Task Show_WithWrongBuilding_ReturnsWrongBuilding()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");
            var response = await _client.GetAsync("https://localhost:7192/api/buildings/7");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Invalid building", result["error"].ToString());
        }

        [Fact]
        public async Task Destroy_WithRightUserAndBuilding_ReturnsBuildingDeleted()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");
            var response = await _client.DeleteAsync("https://localhost:7192/api/buildings/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("Deleted", result["status"].ToString());
        }

        [Fact]
        public async Task Destroy_WithWrongUser_ReturnsWrongUser()
        {
            _client.DefaultRequestHeaders.Add("userId", "5");
            var response = await _client.DeleteAsync("https://localhost:7192/api/buildings/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Invalid user", result["error"].ToString());
        }

        [Fact]
        public async Task Destroy_WithWrongBuilding_ReturnsWrongBuilding()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");
            var response = await _client.DeleteAsync("https://localhost:7192/api/buildings/6");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Invalid building", result["error"].ToString());
        }

        [Fact]
        public async Task Store_WithRightUserAndBuildingAndResources_ReturnsBuildingCreated()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");
            BuildingRequestDto buildingRequestDto = new BuildingRequestDto(1);

            var response = await _client.PostAsync("https://localhost:7192/api/buildings", JsonContent.Create(buildingRequestDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("Building has been created", result["status"].ToString());
        }

        [Fact]
        public async Task Store_WithWrongUser_ReturnsWrongUser()
        {
            _client.DefaultRequestHeaders.Add("userId", "6");
            BuildingRequestDto buildingRequestDto = new BuildingRequestDto(1);

            var response = await _client.PostAsync("https://localhost:7192/api/buildings", JsonContent.Create(buildingRequestDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Invalid user", result["error"].ToString());
        }

        [Fact]
        public async Task Store_WithWrongBuilding_ReturnsWrongBuilding()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");
            BuildingRequestDto buildingRequestDto = new BuildingRequestDto(6);

            var response = await _client.PostAsync("https://localhost:7192/api/buildings", JsonContent.Create(buildingRequestDto));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Invalid building", result["error"].ToString());
        }

        [Fact]
        public async Task Upgrade_WithRightUserAndBuildingAndResources_ReturnsBuildingUpgraded()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");            

            var response = await _client.GetAsync("https://localhost:7192/api/buildings/upgrade/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal("Building has been upgraded", result["status"].ToString());
        }

        [Fact]
        public async Task Upgrade_WithWrongBuilding_ReturnsInvalidBuilding()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");

            var response = await _client.GetAsync("https://localhost:7192/api/buildings/upgrade/5");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Invalid building", result["error"].ToString());
        }

        [Fact]
        public async Task Upgrade_WithTownHallLow_ReturnsTownHallLow()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");

            var response = await _client.GetAsync("https://localhost:7192/api/buildings/upgrade/4");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Cannot upgrade. Building is at the same level as Townhall", result["error"].ToString());
        }

        [Fact]
        public async Task Upgrade_WithNoResources_ReturnsInsufficientResources()
        {
            _client.DefaultRequestHeaders.Add("userId", "1");

            var response = await _client.GetAsync("https://localhost:7192/api/buildings/upgrade/3");
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal("Insufficient resources", result["error"].ToString());
        }
    }
}