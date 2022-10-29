namespace Eucyon_Tribes.Models
{
    public class World
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Kingdom> Kingdoms { get; set; } = null!;
        public List<Location> Locations { get; set; } = null!;

        public World()
        {
            Kingdoms = new List<Kingdom>();
        }
    }
}

