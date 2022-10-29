using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.WorldDTOs;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.WorldTests
{
    [Serializable]
    [Collection("Serialize")]
    public class WorldControllerTestsWithOneWorld : IntegrationTests
    {
        public WorldControllerTestsWithOneWorld() : base("worldControllerWithOneWorldTest")
        {
        }

        [Fact]
        public async void IndexTest()
        {
            var expected = new WorldResponseDTO[1] {new WorldResponseDTO(1, "MyWorld", 3)};
            var response = await _client.GetAsync("https://localhost:7192/api/worlds");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WorldResponseDTO[]>(body);
            Assert.Equal(200, (int)response.StatusCode);
            Assert.Equal(expected[0].Id, result[0].Id);
            Assert.Equal(expected[0].Name, result[0].Name);
            Assert.Equal(expected[0].KingdomCount, result[0].KingdomCount);
        }

        [Fact]
        public async void CreateTest_WithWrongName()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/create?name=");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Given name is unsuitable.", result.Error);
        }

        [Fact]
        public async void CreateTest_WithExistingName()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/create?name=MyWorld");
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
        public async Task ShowTest_WithWrongId()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/5");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Given ID was not found.", result.Error);
        }

        [Fact]
        public async Task ShowTest_WithCorrectId()
        {
            var expected = new WorldDetailDTO(1, "MyWorld", new List<string>(){ "kingdom00", "kingdom01", "kingdom02"});
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/1");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WorldDetailDTO>(body);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Name, result.Name);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(expected.KingdomNames[i], result.KingdomNames[i]);
            }
        }

        [Fact]
        public async Task EditTest_WithWrongId()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/5/edit");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be edited.", result.Error);
        }

        [Fact]
        public async Task EditTest_WithEmptyName()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/1/edit");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be edited.", result.Error);
        }

        [Fact]
        public async Task EditTest_WithExistingName()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/1/edit?name=MyWorld");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be edited.", result.Error);
        }

        [Fact]
        public async Task EditTest_WithCorrectIdAndName()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/worlds/1/edit?name=Sikastan");
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateTest_WithWrongId()
        {
            var world = new UpdateWorldDTO("Sikastan");
            var response = await _client.PutAsync("https://localhost:7192/api/worlds/5", JsonContent.Create(world));
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be updated.", result.Error);
        }

        [Fact]
        public async Task UpdateTest_WithCorrectId()
        {
            var world = new UpdateWorldDTO("Sikastan");
            var response = await _client.PutAsync("https://localhost:7192/api/worlds/1", JsonContent.Create(world));
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task DestroyTest_WithWrongId()
        {
            var response = await _client.DeleteAsync("https://localhost:7192/api/worlds/5");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be destroyed.", result.Error);
        }
        [Fact]
        public async Task DestroyTest_WithCorrectId()
        {
            var response = await _client.DeleteAsync("https://localhost:7192/api/worlds/1");
            Assert.Equal(200, (int)response.StatusCode);
        }
    }
}
