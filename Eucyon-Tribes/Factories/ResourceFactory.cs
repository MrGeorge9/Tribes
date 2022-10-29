using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Factories
{
    public class ResourceFactory : IResourceFactory
    {
        public Food GetFoodResource()
        {
            return new Food { Amount = 0, UpdatedAt = DateTime.Now };
        }

        public Gold GetGoldResource()
        {
            return new Gold { Amount = 0, UpdatedAt = DateTime.Now };
        }

        public People GetPeopleResource()
        {
            return new People { Amount = 0, UpdatedAt = DateTime.Now };
        }

        public Wood GetWoodResource()
        {
            return new Wood { Amount = 0, UpdatedAt = DateTime.Now };
        }

        public Soldier GetSoldierResource()
        {
            return new Soldier
            {
                Amount = 1,
                UpdatedAt = DateTime.Now,
                Level = 1,
                TotalHP = 25,
                Attack = 10,
                Defense = 10
            };
        }
    }
}