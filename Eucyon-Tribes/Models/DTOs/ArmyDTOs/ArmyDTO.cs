using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Models.DTOs.ArmyDTOs
{
    public class ArmyDTO
    {
        public int Id { get; }
        public int Owner { get; }
        public List<int> NumberOfUnitsByLevel { get; }

        public ArmyDTO(int id, int owner, List<int> numberOfUnitsByLevel)
        {
            Id = id;
            Owner = owner;
            NumberOfUnitsByLevel = numberOfUnitsByLevel;
        }
    }
}
