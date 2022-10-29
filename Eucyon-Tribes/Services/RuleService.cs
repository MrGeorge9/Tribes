using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;

namespace Eucyon_Tribes.Services
{
    public interface RuleService
    {
        int getKingdomInitialGoldAmount();
        int getKingdomInitialFoodAmount();
        int getUnitFoodConsumptionRate();
        int getUnitCriticalHitChance();
        bool canBuildBuiding(Kingdom kingdom, Building building, Building townhall);
        bool canProductUnit(Kingdom kingdom, Building barrack);
        int getRequiredAmountOfFoodForBattle(
  Kingdom attacker, Kingdom defender, Army attackArmy);
        bool CanUpgrade(Building building);
        int CanPlunder();
        int CanBeAttacked();
    }
}
