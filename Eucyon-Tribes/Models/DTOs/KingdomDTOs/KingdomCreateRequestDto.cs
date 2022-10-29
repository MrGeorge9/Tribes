namespace Eucyon_Tribes.Models.DTOs.KingdomDTOs
{
    public class KingdomCreateRequestDTO
    {
        public int UserId { get; }
        public string Name { get; }
        public int WorldId { get; }
        public int Coordinate_X { get; }
        public int Coordinate_Y { get; }

        public KingdomCreateRequestDTO(int userId, string name, int worldId, int coordinate_X, int coordinate_Y)
        {
            UserId = userId;
            Name = name;
            WorldId = worldId;
            Coordinate_X = coordinate_X;
            Coordinate_Y = coordinate_Y;
        }
    }
}
