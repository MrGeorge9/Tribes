using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.DTOs.LeaderboardDTOs;
using Eucyon_Tribes.Models.Resources;

namespace Tribes.Tests.LeaderboardTests
{
    [Serializable]
    [Collection("Serialize")]
    public class LeaderboardServiceTests
    {
        private List<Kingdom> kingdoms;
        private ILeaderboardService _leaderboardService;
        private IResourceFactory resourceFactory;
        private IBuildingFactory buildingFactory;
        public LeaderboardServiceTests()
        {
            _leaderboardService = new LeaderboardService();
            resourceFactory = new ResourceFactory();
            buildingFactory = new BuildingFactory();
            kingdoms = CreateKingdoms();
        }

        public List<Kingdom> CreateKingdoms()
        {
            Army army1 = new Army() { Soldiers = new List<Soldier>() };
            Army army2 = new Army() { Soldiers = new List<Soldier>() };
            for (int i = 0; i < 10; i++)
            {
                Soldier soldier = resourceFactory.GetSoldierResource();
                army1.Soldiers.Add(soldier);
                Soldier soldier2 = resourceFactory.GetSoldierResource();
                soldier2.Attack += 5;
                army2.Soldiers.Add(soldier2);
            }

            List<Building> buildings1 = new List<Building>
            {
                buildingFactory.CreateBarracks(),
                buildingFactory.CreateTownHall(),
                buildingFactory.CreateSawMill(),
                buildingFactory.CreateFarm(),
                buildingFactory.CreateMine(),
            };

            List<Building> buildings2 = new List<Building>
            {
                buildingFactory.CreateBarracks(),
                buildingFactory.CreateTownHall(),
                buildingFactory.CreateSawMill(),
                buildingFactory.CreateFarm(),
                buildingFactory.CreateMine(),
            };
            foreach(Building building in buildings2)
            {
                building.Level = 2;
            }

            List<Building> buildings3 = new List<Building>
            {
                buildingFactory.CreateBarracks(),
                buildingFactory.CreateTownHall(),
                buildingFactory.CreateSawMill(),
                buildingFactory.CreateFarm(),
                buildingFactory.CreateMine(),
            };
            buildings3[0].Level = 6;

            List<Kingdom> kingdoms = new List<Kingdom>()
            {
                new Kingdom() {Name = "a", Armies = new List<Army>(){army1}, Buildings = buildings1},
                new Kingdom() {Name = "b", Armies = new List<Army>(){army2}, Buildings = buildings2},
                new Kingdom() {Name = "c", Armies = new List<Army>(){army1, army2}, Buildings = buildings3}
            };
            return kingdoms;
        }

        [Fact]
        public void CalculateSoldierScoreTestWithNotEmptyArmies()
        {
            Assert.Equal(100, _leaderboardService.CalculateSoldierScore(kingdoms[0].Armies));
            Assert.Equal(150, _leaderboardService.CalculateSoldierScore(kingdoms[1].Armies));
            Assert.Equal(250, _leaderboardService.CalculateSoldierScore(kingdoms[2].Armies));
        }

        [Fact]
        public void CalculateSoldierScoreWithNullAndEmptyArmy()
        {
            List<Army>? armies1 = null;
            List<Army> armies2 = new List<Army>();
            Assert.Equal(0, _leaderboardService.CalculateSoldierScore(armies1));
            Assert.Equal(0, _leaderboardService.CalculateSoldierScore(armies2));
        }

        [Fact]
        public void CalculateBuildingScoreWithBuildings()
        {
            
            Assert.Equal(new int[] { 5, 1 }, _leaderboardService.CalculateBuildingScore(kingdoms[0].Buildings));
            Assert.Equal(new int[] { 10, 2 }, _leaderboardService.CalculateBuildingScore(kingdoms[1].Buildings));
            Assert.Equal(new int[] { 10, 6 }, _leaderboardService.CalculateBuildingScore(kingdoms[2].Buildings));
        }

        [Fact]
        public void CalculateBuildingScoreWithNullAndEmptyBuildings()
        {
            List<Building>? buildings1 = null;
            List<Building> buildings2 = new List<Building>();
            Assert.Equal(new int[] {0, 0}, _leaderboardService.CalculateBuildingScore(buildings1));
            Assert.Equal(new int[] {0, 0}, _leaderboardService.CalculateBuildingScore(buildings2));
        }

        [Fact]
        public void GetSoldierScoreDTOsTest()
        {
            SoldierScoreDTO[] expected = new SoldierScoreDTO[]
            {
                new SoldierScoreDTO("a", 100),
                new SoldierScoreDTO("b", 150),
                new SoldierScoreDTO("c", 250)
            };
            var actual = _leaderboardService.GetSoldierScoreDTOs(kingdoms);
            for (int i = 0; i < 3; i++)
            {
                Assert.True(expected[i].KingdomName.Equals(actual[i].KingdomName));
                Assert.Equal(expected[i].SoldierScore, actual[i].SoldierScore);
            }
        }

        [Fact]
        public void GetBuildingScoreDTOsTest()
        {
            BuildingScoreDTO[] expected = new BuildingScoreDTO[]
            {
                new BuildingScoreDTO("a", new int[] {5, 1}),
                new BuildingScoreDTO("b", new int[] {10, 2}),
                new BuildingScoreDTO("c", new int[] {10, 6}),
            };
            var actual = _leaderboardService.GetBuildingScoreDTOs(kingdoms);
            for (int i = 0; i < 3; i++)
            {
                Assert.True(expected[i].KingdomName.Equals(actual[i].KingdomName));
                Assert.Equal(expected[i].BuildingScore, actual[i].BuildingScore);
            }
        }

        [Fact]
        public void GetSoldierLeaderboardWithNullAndEmptyKingdoms()
        {
            List<Kingdom>? kingdoms = null;
            List<Kingdom> kingdoms2 = new List<Kingdom>();
            var expected = new SoldierLeaderboardDTO(new List<SoldierScoreDTO>());
            Assert.Equal(expected.Leaderboard, _leaderboardService.GetSoldierLeaderboard(kingdoms).Leaderboard);
            Assert.Equal(expected.Leaderboard, _leaderboardService.GetSoldierLeaderboard(kingdoms2).Leaderboard);
        }

        [Fact]
        public void GetSoldierLeaderboardTest()
        {
            SoldierLeaderboardDTO expected = new SoldierLeaderboardDTO(new List<SoldierScoreDTO>()
            {
                new SoldierScoreDTO("c", 250),
                new SoldierScoreDTO("b", 150),
                new SoldierScoreDTO("a", 100),
            });
            var actual = _leaderboardService.GetSoldierLeaderboard(kingdoms);
            for (int i = 0; i < 3; i++)
            {
                Assert.True(expected.Leaderboard[i].KingdomName.Equals(actual.Leaderboard[i].KingdomName));
                Assert.Equal(expected.Leaderboard[i].SoldierScore, actual.Leaderboard[i].SoldierScore);
            }
        }

        public void GetBuildingLeaderboardTest()
        {
            BuildingLeaderboardDTO expected = new BuildingLeaderboardDTO(new List<BuildingScoreDTO>()
            {
                new BuildingScoreDTO("c", new int[] {10, 6}),
                new BuildingScoreDTO("b", new int[] {10, 2}),
                new BuildingScoreDTO("a", new int[] {5, 1})
            });
            var actual = _leaderboardService.GetBuildingLeaderboard(kingdoms);
            for (int i = 0; i < 3; i++)
            {
                Assert.True(expected.Leaderboard[i].KingdomName.Equals(actual.Leaderboard[i].KingdomName));
                Assert.Equal(expected.Leaderboard[i].BuildingScore, actual.Leaderboard[i].BuildingScore);
            }
        }
    }
}
