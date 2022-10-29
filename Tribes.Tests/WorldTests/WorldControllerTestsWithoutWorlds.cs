using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.WorldDTOs;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.WorldTests
{
    [Serializable]
    [Collection("Serialize")]
    public class WorldControllerTestsWithoutWorlds : IntegrationTests
    {
        public WorldControllerTestsWithoutWorlds() : base("worldControllerWithoutWorldsTest")
        {
        }

        [Fact]
        public async void IndexTest()
        {
            var expected = new WorldResponseDTO[0];
            var response = await _client.GetAsync("https://localhost:7192/api/worlds");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WorldResponseDTO[]>(body);
            Assert.Equal(500, (int)response.StatusCode);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async void CreateTest_WithEmptyName()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/create?name=");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Given name is unsuitable.", result.Error);
        }

        [Fact]
        public async void CreateTest_WithCorrectName()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/create?name=Sikastan");
            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task StoreTest_WithWrongDTO()
        {
            var world = new StoreWorldDTO(String.Empty, new List<Kingdom>(), new List<Location>());
            var response = await _client.PostAsync("https://localhost:7192/api/worlds", JsonContent.Create(world));
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Given world cannot be stored.", result.Error);
        }

        [Fact]
        public async Task StoreTest_WithCorrectDTO()
        {
            var world = new StoreWorldDTO("Sikastan", new List<Kingdom>(), new List<Location>());
            var response = await _client.PostAsync("https://localhost:7192/api/worlds", JsonContent.Create(world));
            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task ShowTest()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/1");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Given ID was not found.", result.Error);
        }

        [Fact]
        public async Task EditTest()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/1/edit");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be edited.", result.Error);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var world = new UpdateWorldDTO("Sikastan");
            var response = await _client.PutAsync("https://localhost:7192/api/worlds/1", JsonContent.Create(world));
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be updated.", result.Error);
        }

        [Fact]
        public async Task DestroyTest()
        {
            var response = await _client.DeleteAsync("https://localhost:7192/api/worlds/1");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be destroyed.", result.Error);
        }
    }
}
