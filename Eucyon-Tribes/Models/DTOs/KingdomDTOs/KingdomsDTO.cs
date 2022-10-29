namespace Eucyon_Tribes.Models.DTOs.KingdomDTOs
{
    public class KingdomsDTO
    {
        public int Id { get; }
        public int World { get; }
        public int Owner { get; }
        public string Name { get; }

        public KingdomsDTO(int id, int world, int owner, string name)
        {
            Id = id;
            World = world;
            Owner = owner;
            Name = name;
        }
    }
}
