namespace Eucyon_Tribes.Models.DTOs.ArmyDTOs
{
    public class AvailableUnitsDTO
    {
        public List<int> NumberOfUnitsByLevel { get; }

        public AvailableUnitsDTO(List<int> numberOfUnitsByLevel)
        {
            NumberOfUnitsByLevel = numberOfUnitsByLevel;
        }
    }
}
