using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests
{
    [Serializable]
    [Collection("Serialize")]
    public class RuleServiceTests
    {
        public ConfigRuleService ruleService;
        public IConfiguration config;
        private readonly DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "PurchaseTests").Options;
        private readonly ApplicationContext _db;

        public RuleServiceTests()
        {
            _db = new ApplicationContext(options);
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            ruleService = new ConfigRuleService(_db, config);
        }

        [Theory]
        [InlineData(1, 1, 2, 2, 1, 2, 1, 1, 1, 6)]
        [InlineData(10, 10, 0, 0, 1, 2, 1, 1, 1, 57)]
        [InlineData(10, 10, 0, 0, 1, 1, 1, 1, 1, -1)]
        [InlineData(10, 10, 0, 0, 1, 2, 1, 2, 1, -1)]
        [InlineData(1, 1, 2, 2, 1, 2, 1, 1, 2, -1)]
        public void DistanceTestCalculation(int x1, int y1, int x2, int y2
            , int attackerId, int defenderId, int attackerWorldId, int defenderWorldId
            , int armyKingdomID, int expected)
        {
            World world1 = new World();
            world1.Id = attackerWorldId;
            World world2 = new World();
            world2.Id = defenderWorldId;
            Location l1 = new Location();
            l1.XCoordinate = x1;
            l1.YCoordinate = y1;
            l1.KingdomId = attackerId;
            Location l2 = new Location();
            l2.XCoordinate = x2;
            l2.YCoordinate = y2;
            l2.KingdomId = defenderId;
            Kingdom attacker = new Kingdom();
            attacker.Id = attackerId;
            attacker.Location = l1;
            attacker.WorldId = attackerWorldId;
            Kingdom defender = new Kingdom();
            defender.Id = defenderId;
            defender.Location = l2;
            defender.WorldId = defenderWorldId;
            defender.World = world2;
            attacker.World=world1;
            Army attackArmy = new Army();
            attackArmy.KingdomId = armyKingdomID;
            Soldier soldier1 = new Soldier();
            Soldier soldier2 = new Soldier();
            List<Soldier> soldierList = new List<Soldier>();
            soldierList.Add(soldier1);
            soldierList.Add(soldier2);
            attackArmy.Soldiers = soldierList;
            if (armyKingdomID == 2)
                attackArmy.Kingdom = defender;
            else
                attackArmy.Kingdom = attacker;

            var result = ruleService.getRequiredAmountOfFoodForBattle(attacker, defender, attackArmy);

            Assert.Equal(expected, result);
        }
    }
}
