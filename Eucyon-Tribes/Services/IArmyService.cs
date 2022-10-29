using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;

namespace Eucyon_Tribes.Services
{
    public interface IArmyService
    {
        public ArmyDTO[] GetArmies(int kingdomId);

        public ArmyDTO GetArmy(int armyId);

        public Boolean RemoveArmy();

        public String GetError();

        public AvailableUnitsDTO GetAvailableUnits(int id);

        public Army CreateArmy(List<int> numberOfUnitsByLevel, int kingdomId, int distance);
    }
}
