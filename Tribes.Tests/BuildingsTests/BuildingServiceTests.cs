using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.DTOs.BuildingDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Tribes.Tests.BuildingsTests
{
    [Serializable]
    [Collection("Serialize")]
    public class BuildingServiceTests
    {
        private readonly DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "BuildingsTests").Options;
        private ApplicationContext _db;
        private BuildingService _buildingService;

        public BuildingServiceTests()
        {
            _db = new ApplicationContext(options);
            _buildingService = new BuildingService(_db);
        }

        [Fact]
        public void SavingABuildingTest()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            TownHall townHall = new TownHall();
            _buildingService.SaveBuilding(townHall);

            Assert.Equal(1, _db.Buildings.Count());
            Assert.NotEqual(2, _db.Buildings.Count());
        }

        [Fact]
        public void GetBuildingByIdTest()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var buildings = new List<Building>()
            {
                new TownHall(),
                new Farm(),
                new Mine()
            };

            _db.Buildings.AddRange(buildings);
            _db.SaveChanges();

            Assert.Equal(buildings[0], _buildingService.GetBuildingById(1));
        }

        public void SeedDatabase()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var user = new User() { Email = "john@john.com", PasswordHash = "Johny123", Name = "John", ForgottenPasswordToken = "", VerificationToken = "" };
            _db.Users.Add(user);

            var kingdom = new Kingdom() { Name = "Aden" };
            _db.Kingdoms.Add(kingdom);

            user.Kingdom = kingdom;

            var tonwhall = new TownHall()
            {
                Level = 2,
                Hp = 1000,
                StartedAt = DateTime.Today,
                Production = 4,
            };
            var barracks = new Barracks()
            {
                Level = 1,
                Hp = 300,
                StartedAt = DateTime.Today,
                Production = 1,
            };
            var mine = new Mine()
            {
                Level = 1,
                Hp = 100,
                StartedAt = DateTime.Today,
                Production = 7,
            };
            var sawmill = new Sawmill()
            {
                Level = 1,
                Hp = 75,
                StartedAt = DateTime.Today,
                Production = 15,
            };
            _db.Buildings.Add(tonwhall);
            _db.Buildings.Add(barracks);
            _db.Buildings.Add(mine);
            _db.Buildings.Add(sawmill);

            var gold = new Gold() { Amount = 500 };
            var wood = new Wood() { Amount = 500 };
            var food = new Food() { Amount = 500 };
            var soldier = new Soldier() { Amount = 500 };
            var people = new People() { Amount = 500 };
            _db.Resources.Add(gold);
            _db.Resources.Add(wood);
            _db.Resources.Add(food);
            _db.Resources.Add(soldier);
            _db.Resources.Add(people);

            kingdom.Buildings = new List<Building>();
            kingdom.Buildings.Add(tonwhall);
            kingdom.Buildings.Add(barracks);
            kingdom.Buildings.Add(mine);
            kingdom.Buildings.Add(sawmill);
            kingdom.Resources = new List<Resource>();
            kingdom.Resources.Add(gold);
            kingdom.Resources.Add(wood);
            kingdom.Resources.Add(food);
            kingdom.Resources.Add(soldier);
            kingdom.Resources.Add(people);
            _db.SaveChanges();
        }

        [Fact]
        public void GetAllBuldingsOfKingdomTest()
        {
            SeedDatabase();

            List<BuildingsResponseDto> buildingsResponseDtos = new List<BuildingsResponseDto>();

            var productionDto = new ProductionDto(
                1,
                "Gold",
                500,
                false,
                DateTime.Today,
                DateTime.Today);

            var buildingsResponseDto = new BuildingsResponseDto(
                1,
                1,
                "Mine",
                1,
                productionDto);

            buildingsResponseDtos.Add(buildingsResponseDto);

            var productionDto2 = new ProductionDto(
                2,
                "Wood",
                500,
                false,
                DateTime.Today,
                DateTime.Today);

            var buildingsResponseDto2 = new BuildingsResponseDto(
                2,
                1,
                "Sawmill",
                1,
                productionDto2);

            buildingsResponseDtos.Add(buildingsResponseDto2);

            Assert.Equal(buildingsResponseDtos[0].Production.Resource, _buildingService.GetAllBuldingsOfKingdom(1)[2].Production.Resource);
            Assert.Equal(buildingsResponseDtos[0].Production.Amount, _buildingService.GetAllBuldingsOfKingdom(1)[2].Production.Amount);
            Assert.Equal(buildingsResponseDtos[0].Production.Started_at, _buildingService.GetAllBuldingsOfKingdom(1)[2].Production.Started_at);
            Assert.Equal(buildingsResponseDtos[0].Kingdom, _buildingService.GetAllBuldingsOfKingdom(1)[2].Kingdom);
            Assert.Equal(buildingsResponseDtos[0].Type, _buildingService.GetAllBuldingsOfKingdom(1)[2].Type);
            Assert.Equal(buildingsResponseDtos[0].Level, _buildingService.GetAllBuldingsOfKingdom(1)[2].Level);
            Assert.Equal(buildingsResponseDtos[1].Production.Resource, _buildingService.GetAllBuldingsOfKingdom(1)[3].Production.Resource);
            Assert.Equal(buildingsResponseDtos[1].Production.Amount, _buildingService.GetAllBuldingsOfKingdom(1)[3].Production.Amount);
            Assert.Equal(buildingsResponseDtos[1].Production.Started_at, _buildingService.GetAllBuldingsOfKingdom(1)[3].Production.Started_at);
            Assert.Equal(buildingsResponseDtos[1].Kingdom, _buildingService.GetAllBuldingsOfKingdom(1)[3].Kingdom);
            Assert.Equal(buildingsResponseDtos[1].Type, _buildingService.GetAllBuldingsOfKingdom(1)[3].Type);
            Assert.Equal(buildingsResponseDtos[1].Level, _buildingService.GetAllBuldingsOfKingdom(1)[3].Level);
            Assert.Equal("WrongUser", _buildingService.GetAllBuldingsOfKingdom(8)[0].Type);
        }

        [Fact]
        public void SetResourceTypeTest()
        {
            SeedDatabase();

            Assert.Equal("Gold", _buildingService.SetResourceType(1, "Mine").GetType().Name);
            Assert.Equal("Wood", _buildingService.SetResourceType(1, "Sawmill").GetType().Name);
        }

        [Fact]
        public void GetBuildingDetailsTest()
        {
            SeedDatabase();

            var productionDto = new ProductionDto(
                1,
                "Gold",
                500,
                false,
                DateTime.Today,
                DateTime.Today);

            var levelingCostDto = new LevelingCostDto(0, 0);

            var buildingResponseDto = new BuildingResponseDto(
                1,
                1,
                "Mine",
                1,
                productionDto,
                levelingCostDto);

            Assert.Equal(buildingResponseDto.Kingdom, _buildingService.GetBuildingDetails(3, 1).Kingdom);
            Assert.Equal(buildingResponseDto.Type, _buildingService.GetBuildingDetails(3, 1).Type);
            Assert.Equal(buildingResponseDto.Level, _buildingService.GetBuildingDetails(3, 1).Level);
            Assert.Equal(buildingResponseDto.Production.Resource, _buildingService.GetBuildingDetails(3, 1).Production.Resource);
            Assert.Equal(buildingResponseDto.Production.Amount, _buildingService.GetBuildingDetails(3, 1).Production.Amount);
            Assert.Equal(buildingResponseDto.Production.Collected, _buildingService.GetBuildingDetails(3, 1).Production.Collected);
            Assert.Equal(buildingResponseDto.Leveling_Cost.Gold, _buildingService.GetBuildingDetails(3, 1).Leveling_Cost.Gold);
            Assert.Equal("WrongUser", _buildingService.GetBuildingDetails(1, 8).Type);
            Assert.Equal("WrongBuilding", _buildingService.GetBuildingDetails(8, 1).Type);
        }

        [Fact]
        public void DeleteBuildingByIdTest()
        {
            SeedDatabase();

            _buildingService.DeleteBuildingById(1, 1);

            Assert.Equal("Barracks", _db.Buildings.FirstOrDefault().GetType().Name);
            Assert.Equal("WrongBuilding", _buildingService.DeleteBuildingById(5, 1).Status);
            Assert.Equal("WrongUser", _buildingService.DeleteBuildingById(2, 5).Status);
        }

        [Fact]
        public void StoreNewBuldingTest()
        {
            SeedDatabase();

            BuildingRequestDto buildingRequestDto = new BuildingRequestDto(1);
            var kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources).FirstOrDefault(p => p.UserId == 1);
            Barracks barracks = new Barracks()
            {
                Level = 1,
                Hp = 300,
                StartedAt = DateTime.Today,
                Production = 1,
            };

            _buildingService.StoreNewBulding(buildingRequestDto, 1);

            Assert.Equal(barracks.Level, kingdom.Buildings[1].Level);
            Assert.Equal(barracks.Hp, kingdom.Buildings[1].Hp);
            Assert.Equal(barracks.Production, kingdom.Buildings[1].Production);

            BuildingRequestDto wrongBuilding = new BuildingRequestDto(6);
            Assert.Equal("WrongBuilding", _buildingService.StoreNewBulding(wrongBuilding, 1).Status);
            Assert.Equal("WrongUser", _buildingService.StoreNewBulding(buildingRequestDto, 5).Status);

            var gold = kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Gold"));
            gold.Amount -= 500;
            _db.SaveChanges();
            Assert.Equal("NoResources", _buildingService.StoreNewBulding(buildingRequestDto, 1).Status);
        }

        [Fact]
        public void CreateRightBuildingTest()
        {
            Barracks barracks = new Barracks()
            {
                Level = 1,
                Hp = 300,
                StartedAt = DateTime.Today,
                Production = 1,
            };
            var barracks1 = _buildingService.CreateRightBuilding(1);

            Assert.Equal(barracks.Hp, barracks1.Hp);
            Assert.Equal(barracks.Level, barracks1.Level);
            Assert.Equal(barracks.Production, barracks1.Production);
        }

        [Fact]
        public void KingdomHasResourcesForBuildingCreationTest()
        {
            SeedDatabase();

            var kingdom = _db.Kingdoms.FirstOrDefault();
            Barracks barracks = new Barracks()
            {
                Level = 1,
                Hp = 300,
                StartedAt = DateTime.Today,
                Production = 1,
            };

            Assert.True(_buildingService.KingdomHasResourcesForBuildingCreation(kingdom, barracks));

            var gold = kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Gold"));
            gold.Amount -= 500;
            _db.SaveChanges();

            Assert.False(_buildingService.KingdomHasResourcesForBuildingCreation(kingdom, barracks));
        }

        [Fact]
        public void UpgradeBuildingTest()
        {
            SeedDatabase();

            var kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources).FirstOrDefault(p => p.UserId == 1);

            Barracks barracks = new Barracks()
            {
                Level = 2,
                Hp = 600,
                StartedAt = DateTime.Today,
                Production = 2,
            };
            _buildingService.UpgradeBuilding(2, 1);

            Assert.Equal(barracks.Hp, kingdom.Buildings.FirstOrDefault(p => p.GetType().Name.Equals("Barracks")).Hp);           
            Assert.Equal(barracks.Level, kingdom.Buildings.FirstOrDefault(p => p.GetType().Name.Equals("Barracks")).Level);           
        }
    }
}