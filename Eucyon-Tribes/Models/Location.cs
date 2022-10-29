namespace Eucyon_Tribes.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; } = null!;

        public int WorldId { get; set; }
        public World World { get; set; } = null!;
        
        public Location()
        {
        }

        public Location(int xCoordinate, int yCoordinate, int kingdomId, int worldId)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            KingdomId = kingdomId;
            WorldId = worldId;
        }
    }
}

