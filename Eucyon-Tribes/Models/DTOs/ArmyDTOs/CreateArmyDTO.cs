namespace Eucyon_Tribes.Models.DTOs.ArmyDTOs
{
    public class CreateArmyDTO
    {
        public List<int> NumberOfUnitsByLevel { get; }

        public CreateArmyDTO(List<int> numberOfUnitsByLevel)
        {
            NumberOfUnitsByLevel = numberOfUnitsByLevel;
        }
    }
}
