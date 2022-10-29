using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models.Buildings;

namespace Eucyon_Tribes.Factories
{
    public class BuildingFactory : IBuildingFactory
    {
        public BuildingFactory()
        {
        }

        public TownHall CreateTownHall ()
        {
            TownHall townHall = new TownHall()
            {
                Level = 1,
                Hp = 500,
                StartedAt = DateTime.Today,
                Production = 2,
            };
            return townHall;
        }

        public Farm CreateFarm()
        {
            Farm farm = new Farm()
            {
                Level = 1,
                Hp = 50,
                StartedAt = DateTime.Today,
                Production = 5,
            };
            return farm;
        }

        public Mine CreateMine()
        {
            Mine mine = new Mine()
            {
                Level = 1,
                Hp = 100,
                StartedAt = DateTime.Today,
                Production = 7,
            };
            return mine;
        }

        public Sawmill CreateSawMill()
        {
            Sawmill sawmill = new Sawmill()
            {
                Level = 1,
                Hp = 75,
                StartedAt = DateTime.Today,
                Production = 15,
            };
            return sawmill;
        }

        public Barracks CreateBarracks()
        {
            Barracks barracks = new Barracks()
            {
                Level = 1,
                Hp = 300,
                StartedAt = DateTime.Today,
                Production = 1,
            };
            return barracks;
        }
    }
}
