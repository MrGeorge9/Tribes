namespace Eucyon_Tribes.Models
{
    public class Building
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Hp { get; set; }
        public int Production { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; } = null!;
    }
}