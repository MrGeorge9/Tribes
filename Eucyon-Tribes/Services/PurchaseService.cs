using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly ApplicationContext _db;
        public Kingdom kingdom;

        public PurchaseService(ApplicationContext db)
        {
            _db = db;
        }


        public Resource Wood()
        {
            return kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Wood"));
        }
        public Resource Soldier()
        {
            return kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Soldier"));
        }
        public Resource Food()
        {
            return kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Food"));
        }
        public Resource Gold()
        {
            return kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("Gold"));
        }
        public Resource People()
        {
            return kingdom.Resources.FirstOrDefault(r => r.GetType().Name.Equals("People"));
        }

        public bool EnoughResourcesForCreatingFarm()
        {
            if (Wood().Amount > 35 && Gold().Amount > 15)
            {
                Wood().Amount -= 35;
                Gold().Amount -= 15;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForUpgradingFarm(Building building)
        {
            if (Wood().Amount > 35 * (building.Level + 1) && Gold().Amount > 15 * (building.Level + 1))
            {
                Wood().Amount -= 35 * (building.Level + 1);
                Gold().Amount -= 15 * (building.Level + 1);
                building.Level++;
                building.Hp = building.Hp * building.Level;
                building.Production = building.Production * building.Level;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForCreatingMine()
        {
            if (Wood().Amount > 30 && Food().Amount > 10)
            {
                Wood().Amount -= 30;
                Food().Amount -= 10;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForUpgradingMine(Building building)
        {
            if (Wood().Amount > 30 * (building.Level + 1) && Food().Amount > 10 * (building.Level + 1))
            {
                Wood().Amount -= 30 * (building.Level + 1);
                Food().Amount -= 10 * (building.Level + 1);
                building.Level++;
                building.Hp = building.Hp * building.Level;
                building.Production = building.Production * building.Level;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForCreatingSawmill()
        {
            if (Gold().Amount > 15 && Food().Amount > 10)
            {
                Gold().Amount -= 15;
                Food().Amount -= 10;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForUpgradingSawmill(Building building)
        {
            if (Gold().Amount > 15 * (building.Level + 1) && Food().Amount > 10 * (building.Level + 1))
            {
                Gold().Amount -= 15 * (building.Level + 1);
                Food().Amount -= 10 * (building.Level + 1);
                building.Level++;
                building.Hp = building.Hp * building.Level;
                building.Production = building.Production * building.Level;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForCreatingBarracks()
        {
            if (Gold().Amount > 50 && Food().Amount > 40 && Wood().Amount > 100)
            {
                Gold().Amount -= 50;
                Food().Amount -= 40;
                Wood().Amount -= 100;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForUpgradingBarracks(Building building)
        {
            if (Gold().Amount > 50 * (building.Level + 1) && Food().Amount > 40 * (building.Level + 1) && Wood().Amount > 100 * (building.Level + 1))
            {
                Gold().Amount -= 50 * (building.Level + 1);
                Food().Amount -= 40 * (building.Level + 1);
                Wood().Amount -= 100 * (building.Level + 1);
                building.Level++;
                building.Hp = building.Hp * building.Level;
                building.Production = building.Production * building.Level;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForUpgradingTownHall(Building building)
        {
            if (Gold().Amount > 100 * (building.Level + 1) && Food().Amount > 85 * (building.Level + 1)
                && Wood().Amount > 200 * (building.Level + 1) && Soldier().Amount > 25 * (building.Level + 1))
            {
                Gold().Amount -= 100 * (building.Level + 1);
                Food().Amount -= 85 * (building.Level + 1);
                Wood().Amount -= 200 * (building.Level + 1);
                building.Level++;
                building.Hp = building.Hp * building.Level;
                building.Production = building.Production * building.Level;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForCreatingSoldier()
        {
            if (Gold().Amount > 10 && Food().Amount > 5 && People().Amount > 0)
            {
                Gold().Amount -= 10;
                Food().Amount -= 5;
                People().Amount -= 1;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EnoughResourcesForUpgradingSoldier(Soldier soldier)
        {
            if (Gold().Amount > 10 * (soldier.Level + 1) && Food().Amount > 5 * (soldier.Level + 1))
            {
                Gold().Amount -= 10 * (soldier.Level + 1);
                Food().Amount -= 5 * (soldier.Level + 1);
                soldier.Level++;
                soldier.TotalHP = soldier.TotalHP * soldier.Level;
                soldier.Attack = soldier.Attack * soldier.Level;
                soldier.Defense = soldier.Defense * soldier.Level;
                _db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
