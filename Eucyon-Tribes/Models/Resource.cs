namespace Eucyon_Tribes.Models
{
    public class Resource
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; } = null!;

        public Resource()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}