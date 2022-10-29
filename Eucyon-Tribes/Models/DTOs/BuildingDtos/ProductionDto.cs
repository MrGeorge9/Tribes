namespace Eucyon_Tribes.Models.DTOs.BuildingDTOs
{
    public class ProductionDto
    {
        public int Id { get; }
        public string Resource { get; }
        public int Amount { get; }
        public bool Collected { get; }
        public DateTime Started_at { get; }
        public DateTime Completed_at { get; }

        public ProductionDto(int id, string resource, int amount, bool collected, DateTime started_at, DateTime completed_at)
        {
            Id = id;
            Resource = resource;
            Amount = amount;
            Collected = collected;
            Started_at = started_at;
            Completed_at = completed_at;
        }
    }
}
