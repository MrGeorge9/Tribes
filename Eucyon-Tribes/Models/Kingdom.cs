namespace Eucyon_Tribes.Models
{
	public class Kingdom
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;

		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public List<Resource> Resources { get; set; } = null!;

		public List<Building> Buildings { get; set; } = null!;

		public Location Location { get; set; } = null!;

		public int WorldId { get; set; }
		public World World { get; set; } = null!;

		public List<Army> Armies { get; set; } = null!;

		public List<Battle> AttackBattles { get; set; } = null!;
		public List<Battle> DefendBattles { get; set; } = null!;
		public DateTime CanBeAttackedAt {get; set;}
		public Kingdom()
		{
		}

        public Kingdom(string name, int userId, int worldId)
        {
            Name = name;
            UserId = userId;
            WorldId = worldId;
        }
    }
}