namespace Eucyon_Tribes.Models.DTOs.WorldDTOs
{
    public class WorldDetailDTO
    {
        public int Id { get;}
        public string Name { get;}
        public List<string> KingdomNames { get;}

        public WorldDetailDTO(int id, string name, List<string> kingdomNames)
        {
            Id = id;
            Name = name;
            KingdomNames = kingdomNames;
        }
    }
}
