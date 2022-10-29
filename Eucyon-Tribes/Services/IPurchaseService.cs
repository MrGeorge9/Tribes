using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Services
{
    public interface IPurchaseService
    {
        Resource Wood();

        Resource Soldier();

        Resource Food();

        Resource Gold();

        Resource People();

        bool EnoughResourcesForCreatingFarm();

        bool EnoughResourcesForUpgradingFarm(Building building);

        bool EnoughResourcesForCreatingMine();

        bool EnoughResourcesForUpgradingMine(Building building);

        bool EnoughResourcesForCreatingSawmill();

        bool EnoughResourcesForUpgradingSawmill(Building building);

        bool EnoughResourcesForCreatingBarracks();

        bool EnoughResourcesForUpgradingBarracks(Building building);

        bool EnoughResourcesForUpgradingTownHall(Building building);

        bool EnoughResourcesForCreatingSoldier();

        bool EnoughResourcesForUpgradingSoldier(Soldier soldier);
    }
}
