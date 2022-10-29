using Eucyon_Tribes.Factories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.BattlesTest
{
    [Serializable]
    [Collection("Serialize")]
    public class BattleFactoryTest
    {
        public IConfiguration Config { get; set; }
        public IBattleFactory BattleFactory;

        public BattleFactoryTest()
        {
            var Config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            foreach (var child in Config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            BattleFactory = new BattleFactory(Config);
        }
        [Fact]
        public void BattleFactory_Create_Battle() 
        {
            Kingdom attacker = new Kingdom();
            Kingdom defender = new Kingdom();
            Army army = new Army();

            Battle battle = BattleFactory.CreateBattle(attacker, defender, army.Id, 5);

            Assert.Equal(attacker , battle.Attacker);
            Assert.Equal(defender , battle.Defender);
            Assert.Equal(attacker.Id, battle.AttackerId);
            Assert.Equal(defender.Id, battle.DefenderId);
            Assert.True(battle.RequestedAt<DateTime.Now && battle.RequestedAt > DateTime.Now.AddMinutes(-1));
            Assert.Equal(battle.RequestedAt.AddMinutes(100), battle.Fought_at);
            Assert.True(battle.WinnerId == null);
        }
    }
}
