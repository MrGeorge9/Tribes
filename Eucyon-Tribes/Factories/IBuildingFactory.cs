using Eucyon_Tribes.Models.Buildings;

namespace Eucyon_Tribes.Factories
{
    public interface IBuildingFactory
    {
        TownHall CreateTownHall();
        Farm CreateFarm();
        Mine CreateMine();
        Sawmill CreateSawMill();
        Barracks CreateBarracks();
    }
}
