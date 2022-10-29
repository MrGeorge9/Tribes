namespace Eucyon_Tribes.Models
{
    public class Battle
    {
        public int Id { get; set; }

        public int AttackerId { get; set; }
        public Kingdom Attacker { get; set; } = null!;

        public int DefenderId { get; set; }
        public Kingdom Defender { get; set; } = null!;

        public int? WinnerId { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime Fought_at { get; set; }
        public int AttackerArmyId { get; set; }

    }
}
