namespace Eucyon_Tribes.Models.DTOs.KingdomDTOs
{
    public class KingdomDTO
    {
        public int Id { get; }
        public int World { get; }
        public int Owner { get; }
        public int CoordinateX { get; }
        public int CoordinateY { get; }

        public KingdomDTO(int id, int world, int owner, int coordinateX, int coordinateY)
        {
            Id = id;
            World = world;
            Owner = owner;
            CoordinateX = coordinateX;
            CoordinateY = coordinateY;
        }
    }
}
