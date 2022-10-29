using Eucyon_Tribes.Models.UserModels;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.UserTests
{
    [Serializable]
    [Collection("Serialize")]
    public class UserControllerIntegrationTestsWithWorld : IntegrationTests
    {
        private static string Worlds = "userControllerTestWorlds1";

        public UserControllerIntegrationTestsWithWorld() : base(Worlds)
        {
        }

        [Fact]
        public async void User_info_by_existing_name()
        {
            var expected = "Klotilda";
            var response = _client.GetAsync("users/info?name=Klotilda").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<User>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(200, (int)response.StatusCode);
            Assert.Equal(expected, result.Name);
        }

        [Fact]
        public async void User_info_by_non_existing_name()
        {
            var expected = new { Error = "User not in database" };
            var response = _client.GetAsync("users/info?name=Etela").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(404, (int)response.StatusCode);
            Assert.Equal(expected.Error, result["error"].ToString());
        }

        [Fact]
        public async void Users_list_in_not_empty_table()
        {
            var expected = 2;
            var response = _client.GetAsync("users").Result;
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<User>>(body);

            Assert.Equal(200, (int)response.StatusCode);
            Assert.Equal(expected, result.Count);
        }

        [Fact]
        public async void Create_user_in_database_with_worlds()
        {
            var response = _client.PostAsync("users/create", JsonContent.Create(new UserCreateDto("Mrochta", "h12345678", "mrochta@gmail.com"))).Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(201, (int)response.StatusCode);
            Assert.Equal("ok", result["status"].ToString());

        }

        [Fact]
        public async void User_info_by_existing_id()
        {
            var expected = "Klotilda";
            var response = _client.GetAsync("users/2").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(200, (int)response.StatusCode);
            Assert.Equal(expected, result["username"].ToString());
        }

        [Fact]
        public async void User_info_by_non_existing_id()
        {
            var expected = new { Error = "Player not found" };
            var response = _client.GetAsync("users/500").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(404, (int)response.StatusCode);
            Assert.Equal(expected.Error, result["error"].ToString());
        }

        [Fact]
        public async void User_info_by_non_valid_id()
        {
            var expected = new { Error = "Invalid id" };
            var response = _client.GetAsync("users/0").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, result["error"].ToString());
        }
    }
}