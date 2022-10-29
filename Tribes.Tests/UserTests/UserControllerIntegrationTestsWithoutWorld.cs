using Eucyon_Tribes.Models.UserModels;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.UserTests
{
    [Serializable]
    [Collection("Serialize")]
    public class UserControllerIntegrationTestsWithoutWorld : IntegrationTests
    {
        private static string Worlds = "userControllerTestWorlds0";

        public UserControllerIntegrationTestsWithoutWorld() : base(Worlds)
        {
        }

        [Fact]
        public async void Create_user_in_database_without_worlds()
        {
            var expected = new { Error = "No worlds in database" };
            var response = _client.PostAsync("users/create", JsonContent.Create(new UserCreateDto("Hedviga", "h", "hedviga@gmail.com"))).Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(500, (int)response.StatusCode);
            Assert.Equal(expected.Error, result["error"].ToString());
        }
    }
}
