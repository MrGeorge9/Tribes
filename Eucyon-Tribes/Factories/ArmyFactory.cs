using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Factories
{
    public class ArmyFactory : IArmyFactory
    {
        private readonly IConfiguration _config;

        public ArmyFactory(IConfiguration config)
        {
            _config = config;
        }

        public Army CrateArmy(List<Soldier> soldiers, Kingdom kingdom, int distance)
        {
            Army army = new Army();
            foreach (Soldier soldier in soldiers)
            {
                soldier.Army = army;
            }
            army.Soldiers = soldiers;
            army.Kingdom = kingdom;
            TimeSpan time = new TimeSpan(0, 0, 2 * (distance * int.Parse(Environment.GetEnvironmentVariable("DistanceMultiplier"))), 0);
            army.DisbandsAt = DateTime.Now.Add(time);
            return army;
        }
    }
}

