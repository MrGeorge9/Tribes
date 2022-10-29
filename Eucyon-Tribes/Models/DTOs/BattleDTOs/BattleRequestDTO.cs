namespace Eucyon_Tribes.Models.DTOs.BattleDTOs
{
    public class BattleRequestDTO
    {
        public int AttackerId { get; }
        public List<int> NumberOfUnitsByLevel { get; }

        public BattleRequestDTO(int attackerId , List<int> numberOfUnitsByLevel)
        {
            AttackerId = attackerId;
            NumberOfUnitsByLevel = numberOfUnitsByLevel;
        }
    }
}
