using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.Resources;
using Microsoft.Extensions.Configuration;

namespace Tribes.Tests
{
    [Serializable]
    [Collection("Serialize")]
    public class ArmyFactoryTests
    {
        ArmyFactory Factory;
        Kingdom Kingdom;
        IConfiguration Configuration;

        public ArmyFactoryTests()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in Configuration.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            Factory = new ArmyFactory(Configuration);
            Kingdom = new Kingdom();
        }

        [Fact]
        public void ArmyFactory_Create_DefenseArmy()
        {
            List<Soldier> soldiers = new List<Soldier>();
            for (int i = 0; i < 1000; i++)
            {
                Soldier soldier = new Soldier();
                soldier.Attack = 10;
                soldier.Defense = 10;
                soldier.TotalHP = 30;
                soldiers.Add(soldier);
            }

            Army army = Factory.CrateArmy(soldiers, Kingdom,0);

            Assert.Equal(army.Soldiers, soldiers);
            Assert.Equal(army.Kingdom, Kingdom);
        }

        [Fact]
        public void ArmyFactory_Create_AttackArmy()
        {
            List<Soldier> soldiers = new List<Soldier>();
            for (int i = 0; i < 1000; i++)
            {
                Soldier soldier = new Soldier();
                soldier.Attack = 10;
                soldier.Defense = 10;
                soldier.TotalHP = 30;
                soldiers.Add(soldier);
            }

            Army army = Factory.CrateArmy(soldiers, Kingdom, 5);

            Assert.Equal(army.Soldiers, soldiers);
            Assert.Equal(army.Kingdom, Kingdom);
            Assert.True(army.DisbandsAt < DateTime.Now.AddMinutes(5 * 2 * 20 + 1));
            Assert.True(army.DisbandsAt > DateTime.Now.AddMinutes(5 * 2 * 20-1));
        }
    }
}
