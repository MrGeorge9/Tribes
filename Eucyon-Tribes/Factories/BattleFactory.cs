using Eucyon_Tribes.Models;

namespace Eucyon_Tribes.Factories
{
    public class BattleFactory : IBattleFactory
    {
        private readonly IConfiguration _config;

        public BattleFactory(IConfiguration configuration) 
        {
            _config = configuration;
        }
        public Battle CreateBattle(Kingdom attacker, Kingdom defender, int armyId, int distance)
        {
            Battle battle = new Battle();
            battle.Attacker =attacker;
            battle.AttackerId =attacker.Id;
            battle.AttackerArmyId=armyId;
            battle.Defender =defender;
            battle.DefenderId =defender.Id;
            battle.RequestedAt =DateTime.Now;
            TimeSpan time = new TimeSpan(0, 0, (distance * int.Parse(Environment.GetEnvironmentVariable("DistanceMultiplier"))),0);
            battle.Fought_at = battle.RequestedAt.Add(time);
            return battle;
        }
    }
}
