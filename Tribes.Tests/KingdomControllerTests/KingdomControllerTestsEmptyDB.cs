using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TribesTest;

namespace Tribes.Tests.KingdomControllerTests
{
    [Serializable]
    [Collection("Serialize")]
    public class KingdomControllerTestsEmptyDB : IntegrationTests
    {

        public KingdomControllerTestsEmptyDB() : base("")
        {
        }

        [Fact]
        public async Task KingdomControllerEmpty_Index_List()
        {
            KingdomsDTO[] expected = new KingdomsDTO[0];

            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<KingdomsDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(expected, outputCheck);
        }

        [Fact]
        public async Task KingdomControllerEmpty_Show_Error()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms/1");

            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task KingdomControllerEmpty_Store_Error()
        {
            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/", JsonContent.Create(new CreateKingdomDTO(1, 1, "test")));

            Assert.Equal(400, (int)response.StatusCode);
        }
    }
}
