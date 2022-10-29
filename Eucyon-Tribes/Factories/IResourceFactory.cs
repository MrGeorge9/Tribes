using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Factories
{
    public interface IResourceFactory
    {
        Food GetFoodResource();
        Gold GetGoldResource();
        People GetPeopleResource();
        Soldier GetSoldierResource();
        Wood GetWoodResource();
    }
}