using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.BattleDTOs;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.KingdomControllerTests
{
    [Serializable]
    [Collection("Serialize")]
    public class KingdomControllerTests : IntegrationTests
    {
        public KingdomControllerTests() : base("kingdomControllerTest")
        {
        }

        [Fact]
        public async Task KingdomController_Index_List()
        {
            KingdomsDTO[] expected = new KingdomsDTO[1];
            KingdomsDTO kingdom = new KingdomsDTO(1, 1, 1, "Kingdom1");
            expected[0] = kingdom;


            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<KingdomsDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task KingdomController_Store_Add()
        {
            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms", JsonContent.Create(new CreateKingdomDTO(4, 1, "memes")));

            Assert.Equal(200, (int)response.StatusCode);

        }

        [Fact]
        public async Task KingdomController_Store_Error1()
        {
            ErrorDTO expected = new ErrorDTO("User already has a kingdom");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms", JsonContent.Create(new CreateKingdomDTO(1, 1, "memes")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());
        }

        [Fact]
        public async Task KingdomController_Store_Error2()
        {
            ErrorDTO expected = new ErrorDTO("Invalid world Id");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms", JsonContent.Create(new CreateKingdomDTO(1, 2, "memes")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());

        }

        [Fact]
        public async Task KingdomController_Store_Error3()
        {
            ErrorDTO expected = new ErrorDTO("Invalid name");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms", JsonContent.Create(new CreateKingdomDTO(2, 1, "")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());

        }

        [Fact]
        public async Task KingdomController_Store_Error4()
        {
            ErrorDTO expected = new ErrorDTO("Kingdom with this name already exists");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms", JsonContent.Create(new CreateKingdomDTO(2, 1, "kingdom1")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());

        }


        [Fact]
        public async Task KingdomController_Store_Error5()
        {
            ErrorDTO expected = new ErrorDTO("Invalid user Id");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms", JsonContent.Create(new CreateKingdomDTO(5, 1, "memes")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());

        }

        [Fact]
        public async Task KingdomController_Show_KingdomDTO()
        {
            KingdomDTO expected = new KingdomDTO(1, 1, 1, 0, 0);

            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<KingdomDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected.Owner, outputCheck.Owner);
            Assert.Equal(expected.Id, outputCheck.Id);
            Assert.Equal(expected.CoordinateY, outputCheck.CoordinateY);
            Assert.Equal(expected.CoordinateX, outputCheck.CoordinateX);
            Assert.Equal(expected.World, outputCheck.World);
        }

        [Fact]
        public async Task KingdomController_Show_Error1()
        {
            ErrorDTO expected = new ErrorDTO("Invalid kingdom Id");

            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms/-3");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());
        }

        [Fact]
        public async Task KingdomController_Show_Error2()
        {
            ErrorDTO expected = new ErrorDTO("Kingdom with this Id doesn't exist");

            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms/10");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(404, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());
        }

        [Fact]
        public async Task KingdomController_Cost_Error1()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 6, 6 });
            ErrorDTO expected = new ErrorDTO("Invalid kingdom Id");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/0/battles/cost", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error.ToString());
        }

        [Fact]
        public async Task KingdomController_Cost_Error2()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 6, 6 });
            ErrorDTO expected = new ErrorDTO("Kingdom not found");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/5/battles/cost", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(404, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error.ToString());
        }

        [Fact]
        public async Task KingdomController_Cost_Error3()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 5 });
            ErrorDTO expected = new ErrorDTO("You cannot attack yourself");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/1/battles/cost", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error.ToString());
        }

        [Fact]
        public async Task KingdomController_Cost_Error4()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 5 });
            ErrorDTO expected = new ErrorDTO("Kingdom can not be attacked yet, as it was attacked recently");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/3/battles/cost", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error.ToString());
        }

        [Fact]
        public async Task KingdomController_Cost_Error5()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 5, 5 });
            ErrorDTO expected = new ErrorDTO("You do not posses any units of requested level");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/2/battles/cost", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error.ToString());
        }

        [Fact]
        public async Task KingdomController_Cost_Error6()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 6});
            ErrorDTO expected = new ErrorDTO("Not enugh units of level 2");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/2/battles/cost", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error.ToString());
        }

        [Fact]
        public async Task KingdomController_Cost_CostDTO()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 5 });
            BattleCostDTO expected = new BattleCostDTO(424);

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/2/battles/cost", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<BattleCostDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(200, (int)response.StatusCode);
            Assert.Equal(expected.food, outputCheck.food);
        }

        [Fact]
        public async Task KingdomController_Attack_StatusDTO()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 5 });
            StatusDTO expected = new StatusDTO("Attack order issued");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/2/battles/attack", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<StatusDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(200, (int)response.StatusCode);
            Assert.Equal(expected.status, outputCheck.status);
        }

        [Fact]
        public async Task KingdomController_Attack_Error1()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(2, new List<int> { 1});
            ErrorDTO expected = new ErrorDTO("Insufficient food to attack");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/1/battles/attack", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error);
        }

        [Fact]
        public async Task KingdomController_Attack_Error2()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 6, 6 });
            ErrorDTO expected = new ErrorDTO("Invalid kingdom Id");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/0/battles/attack", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error);
        }

        [Fact]
        public async Task KingdomController_Attack_Error3()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 6, 6 });
            ErrorDTO expected = new ErrorDTO("Kingdom not found");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/10/battles/attack", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(404, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error);
        }

        [Fact]
        public async Task KingdomController_Attack_Error4()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 5 });
            ErrorDTO expected = new ErrorDTO("You cannot attack yourself");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/1/battles/attack", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error);
        }

        [Fact]
        public async Task KingdomController_Attack_Error5()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 5 });
            ErrorDTO expected = new ErrorDTO("Kingdom can not be attacked yet, as it was attacked recently");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/3/battles/attack", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error);
        }

        [Fact]
        public async Task KingdomController_Attack_Error6()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 5,5 });
            ErrorDTO expected = new ErrorDTO("You do not posses any units of requested level");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/3/battles/attack", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error);
        }

        [Fact]
        public async Task KingdomController_Attack_Error7()
        {
            BattleRequestDTO battleRequestDTO = new BattleRequestDTO(1, new List<int> { 5, 6 });
            ErrorDTO expected = new ErrorDTO("Not enugh units of level 2");

            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/3/battles/attack", JsonContent.Create(battleRequestDTO));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck.Error);
        }
    }
}
