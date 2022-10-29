using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Services
{
    public class ArmyService : IArmyService
    {
        private readonly IArmyFactory _armyFactory;
        private readonly ApplicationContext _db;
        public String ErrorMessage { get; private set; }
        public int ArmySizeLimit { get; }

        public ArmyService(IArmyFactory armyFactory, ApplicationContext db)
        {
            this._db = db;
            _armyFactory = armyFactory;
        }

        //kingdom id later to be replaced by some sort of user verification
        public ArmyDTO[] GetArmies(int kingdomId)
        {
            Kingdom kingdom = _db.Kingdoms.FirstOrDefault(k => k.Id == kingdomId);
            List<Army> armies = _db.Armies.Include(a => a.Soldiers).Where(a => a.Kingdom == kingdom).ToList();
            ArmyDTO[] armyDTOs = new ArmyDTO[armies.Count()];
            for (int i = 0; i < armies.Count; i++)
            {
                armyDTOs[i] = GetArmy(armies[i].Id);
            }
            return armyDTOs;
        }

        public ArmyDTO GetArmy(int armyId)
        {
            //needs to varify token as well
            if (armyId < 1)
            {
                ErrorMessage = "Invalid id";
                return null;
            }
            Army army = _db.Armies.Include(a => a.Soldiers).FirstOrDefault(k => k.Id == armyId);
            if (army == null)
            {
                ErrorMessage = "Army not found";
                return null;
            }
            if (army.Soldiers.Count() == 0)
            {
                return new ArmyDTO(army.Id, army.KingdomId, new List<int>());
            }
            else
            {
                int[] numberOfUnitsByLevel = new int[army.Soldiers.GroupBy(s => s.Level).Select(g => g.Key).Max()];
                foreach (Soldier soldier in army.Soldiers)
                {
                    numberOfUnitsByLevel[soldier.Level - 1]++;
                }
                return new ArmyDTO(army.Id, army.KingdomId, numberOfUnitsByLevel.ToList());
            }
        }

        public Boolean RemoveArmy()
        {
            List<Army> armies = _db.Armies.Include(a => a.Soldiers).Where(k => k.DisbandsAt <= DateTime.Now).ToList();
            foreach (Army army in armies)
            {
                foreach (Soldier soldier in army.Soldiers)
                {
                    soldier.Army = null;
                }
                _db.Armies.Remove(army);
            }
            _db.SaveChanges();
            return true;
        }

        public String GetError()
        {
            String output = this.ErrorMessage;
            this.ErrorMessage = null;
            return output;
        }
        //id to be replaced with token in header
        public AvailableUnitsDTO GetAvailableUnits(int id)
        {
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Resources).FirstOrDefault(k => k.Id == id);
            List<Resource> resources = kingdom.Resources.Where(r => r.GetType() == typeof(Soldier)).ToList();
            List<Soldier> soldiers = new List<Soldier>();
            foreach (Resource resource in resources)
            {
                soldiers.Add((Soldier)resource);
            }
            int[] output = new int[soldiers.GroupBy(s => s.Level).Select(g => g.Key).Count()];
            foreach (Soldier soldier in soldiers)
            {
                if (soldier.Army == null)
                    output[soldier.Level - 1]++;
            }
            return new AvailableUnitsDTO(output.ToList());
        }
        public Army CreateArmy(List<int> numberOfUnitsByLevel, int kingdomId, int distance)
        {
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Resources).FirstOrDefault(k => k.Id == kingdomId);
            List<Resource> resources = kingdom.Resources.Where(r => r.GetType() == typeof(Soldier)).ToList();
            List<Soldier> soldiers = new List<Soldier>();
            foreach (Resource resource in resources)
            {
                soldiers.Add((Soldier)resource);
            }
            int[] availableUnits = new int[soldiers.GroupBy(s => s.Level).Select(g => g.Key).Count()];
            foreach (Soldier soldier in soldiers)
            {
                if (soldier.Army == null)
                    availableUnits[soldier.Level - 1]++;
            }
            if (numberOfUnitsByLevel.Count() > availableUnits.Length)
            {
                ErrorMessage = "You do not posses any units of requested level";
                return null;
            }
            for (int i = 0; i < numberOfUnitsByLevel.Count; i++)
            {
                if (numberOfUnitsByLevel[i] > availableUnits[i])
                {
                    ErrorMessage = "Not enugh units of level " + (i + 1);
                    return null;
                }
            }
            List<Soldier> soldiersToAdd = new List<Soldier>();
            for (int i = 0; i < numberOfUnitsByLevel.Count; i++)
            {
                soldiersToAdd.AddRange(soldiers.Where(s => s.Level == i + 1).ToList().GetRange(0, numberOfUnitsByLevel[i]));
            }
            Army army = _armyFactory.CrateArmy(soldiersToAdd, kingdom, distance);
            return army;
        }
    }
}
