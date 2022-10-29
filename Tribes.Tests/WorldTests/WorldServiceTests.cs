using Eucyon_Tribes.Models.DTOs.WorldDTOs;
using Microsoft.EntityFrameworkCore;

namespace Tribes.Tests.WorldTests
{
    [Serializable]
    [Collection("Serialize")]
    public class WorldServiceTests
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "WorldServiceTest").Options;
        public ApplicationContext _db;
        private IWorldService _worldService;

        public WorldServiceTests()
        {
            _db = new ApplicationContext(options);
            _worldService = new WorldService(_db);
            CreateWorlds();
        }

        public void CreateWorlds()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            var worlds = new List<World>();
            for (int i = 0; i < 3; i++)
            {
                worlds.Add(new World() { Name = $"world{i}"});
            }
            List<Kingdom> kingdoms1 = new List<Kingdom>();
            List<Kingdom> kingdoms2 = new List<Kingdom>();
            List<Kingdom> kingdoms3 = new List<Kingdom>();
            for (int i = 0; i < 3; i++)
            {
                if (i % 2 == 0)
                {
                    kingdoms1.Add(new Kingdom() { WorldId = worlds[0].Id, Name = i + "a" });
                }
                kingdoms2.Add(new Kingdom() { WorldId = worlds[1].Id, Name = i + "b" });
            }
            worlds[0].Kingdoms = kingdoms1;
            worlds[1].Kingdoms = kingdoms2;
            worlds[2].Kingdoms = kingdoms3;
            _db.Worlds.AddRange(worlds);
            _db.Kingdoms.AddRange(kingdoms1);
            _db.Kingdoms.AddRange(kingdoms2);
            _db.Kingdoms.AddRange(kingdoms3);
            _db.SaveChanges();
        }

        [Fact]
        public void GetWorldsWithKingdomsTest_WithWorlds()
        {
            var expectedId = new int[] { 1, 2, 3 };
            var expectedName = new string[] { "world0", "world1", "world2" };
            var expectedCount = new int[] { 2, 3, 0 };
            var actual = _worldService.GetWorldsWithKingdoms();
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(expectedId[i], actual[i].Id);
                Assert.Equal(expectedName[i], actual[i].Name);
                Assert.Equal(expectedCount[i], actual[i].KingdomCount);
            }
        }

        [Fact]
        public void GetWorldsWithKingdomsTest_WithoutWorlds()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            var expected = new WorldResponseDTO[0];
            var actual = _worldService.GetWorldsWithKingdoms();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("world0", false)]
        [InlineData("Good name", true)]
        public void CreateWorldTest(string? kingdomName, bool result)
        {
            var actual = _worldService.CreateWorld(kingdomName);
            Assert.Equal(result, actual);
        }

        [Fact]
        public void StoreWorldTest_WithNull()
        {
            var expected = false;
            var actual = _worldService.StoreWorld(null);
            Assert.Equal(expected, actual);
            Assert.Equal(3, _db.Worlds.Count());
        }

        [Fact]
        public void StoreWorldTest_WithWorld()
        {
            var newWorld = new StoreWorldDTO("Johnny", new List<Kingdom>(), new List<Location>());
            var expected = true;
            var actual = _worldService.StoreWorld(newWorld);
            Assert.Equal(expected, actual);
            Assert.Equal(4, _db.Worlds.Count());
        }

        [Theory]
        [InlineData(-1, null)]
        [InlineData(4, null)]
        public void GetWorldDetailsTest_WithWrongInputs(int id, WorldDetailDTO? result)
        {
            Assert.Equal(result, _worldService.GetWorldDetails(id));
        }

        [Theory]
        [InlineData(1, 2, "world0")]
        [InlineData(2, 3, "world1")]
        [InlineData(3, 0, "world2")]
        public void GetWorldDetailsTest_WithCorrectInputs(int id, int kingdomCount, string worldName)
        {
            var world = _worldService.GetWorldDetails(id);
            Assert.Equal(kingdomCount, world.KingdomNames.Count());
            Assert.Equal(worldName, world.Name);
        }

        [Fact]
        public void EditWorldTest_WithWrongId()
        {
            var expected = false;
            var actual = _worldService.EditWorld(4, "Sikastan");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "Lala", true, "Lala")]
        [InlineData(1, "", false, "world0")]
        [InlineData(1, null, false, "world0")]
        public void EditWorldNameTest_WithCorrectId(int id, string? name, bool result, string worldName)
        {
            var actual = _worldService.EditWorld(id, name);
            Assert.Equal(result, actual);
            Assert.Equal(worldName, _db.Worlds.FirstOrDefault(w => w.Id == id).Name);
        }

        [Fact]
        public void UpdateWorldTest_WithWrongId()
        {
            var expected = false;
            var actualWithWrongId = _worldService.UpdateWorld(4, new UpdateWorldDTO("'Murica"));
            var actualWithNullDTO = _worldService.UpdateWorld(1, null);
            Assert.Equal(expected, actualWithWrongId);
            Assert.Equal(expected, actualWithNullDTO);
        }

        [Fact]
        public void UpdateWorldTest_WithCorrectInput()
        {
            var expected = true;
            var actual = _worldService.UpdateWorld(1, new UpdateWorldDTO("'Murica"));
            Assert.Equal(expected, actual);
            Assert.Equal("'Murica", _db.Worlds.FirstOrDefault(w => w.Id == 1).Name);
        }

        [Theory]
        [InlineData(4, false, 3)]
        [InlineData(1, true, 2)]
        public void DestroyWorldTest(int id, bool expected, int worldCount)
        {
            var actual = _worldService.DestroyWorld(id);
            Assert.Equal(expected, actual);
            Assert.Equal(worldCount, _db.Worlds.Count());
        }
    }
}
