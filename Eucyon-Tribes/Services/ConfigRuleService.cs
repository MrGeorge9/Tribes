using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;

namespace Eucyon_Tribes.Services
{
    public class ConfigRuleService : RuleService
    {
        private readonly int _initGold;
        private readonly int _initFood;
        private readonly int _crticalChance;
        private readonly int _foodConsumptionDistance;
        private readonly int _canBeAttacked;
        private readonly int _canPlunder;
        private readonly ApplicationContext _db;
        private readonly IConfiguration _config;

        public ConfigRuleService(ApplicationContext applicationContext, IConfiguration config)
        {
            _config = config;
            _initGold = _config.GetValue<int>("GameVariables:initGold");
            _initFood = _config.GetValue<int>("GameVariables:initFood");
            _crticalChance = _config.GetValue<int>("GameVariables:crticalChance"); ;
            _foodConsumptionDistance = _config.GetValue<int>("GameVariables:foodConsumptionDistance");
            _db = applicationContext;
            _canBeAttacked = _config.GetValue<int>("GameVariables:canAttack");
            _canPlunder = _config.GetValue<int>("GameVariables:canPlunder");

        }

        public int CanBeAttacked()
        {
            return _canBeAttacked;
        }

        public bool canBuildBuiding(Kingdom kingdom, Building building, Building townhall)
        {
            if (townhall == null || kingdom == null || building == null)
                return false;

            if (townhall.KingdomId.Equals(kingdom.Id)
                && building.KingdomId.Equals(kingdom.Id)
                && townhall.Level > building.Level)
                return true;

            return false;
        }

        public int CanPlunder()
        {
            return _canPlunder;
        }

        public bool canProductUnit(Kingdom kingdom, Building barracks)
        {
            if (kingdom == null || barracks == null
                || !barracks.KingdomId.Equals(kingdom.Id))
                return false;

            return true; ;
        }

        public bool CanUpgrade(Building building)
        {
            if (building == null) return false;
            Building townhall = _db.Buildings.FirstOrDefault(b => b is TownHall && b.KingdomId == building.KingdomId);
            if (townhall == null) return false;
            return (townhall.Level > building.Level);
        }

        public int getKingdomInitialFoodAmount()
        {
            return _initFood;
        }

        public int getKingdomInitialGoldAmount()
        {
            return _initGold;
        }

        public int getRequiredAmountOfFoodForBattle(Kingdom attacker, Kingdom defender, Army attackArmy)
        {
            if (attacker == null || defender == null || attackArmy == null)
                return -1;
                if( !attacker.Id.Equals(attackArmy.Kingdom.Id))
                 return -1;
                if (!attacker.WorldId.Equals(defender.World.Id))
                 return -1;
                if( attacker.Id.Equals(defender.Id))
                 return -1;
                if( attackArmy.Soldiers.Count==0)
                return -1;

            int x1 = attacker.Location.XCoordinate;
            int y1 = attacker.Location.YCoordinate;
            int x2 = defender.Location.XCoordinate;
            int y2 = defender.Location.YCoordinate;
            var distance = Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            return Convert.ToInt32(distance * getUnitFoodConsumptionRate() * attackArmy.Soldiers.Count);
        }

        public int getUnitCriticalHitChance()
        {
            return _crticalChance;
        }

        public int getUnitFoodConsumptionRate()
        {
            return _foodConsumptionDistance;
        }
    }
}
