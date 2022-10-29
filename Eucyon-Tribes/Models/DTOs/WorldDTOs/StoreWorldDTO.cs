namespace Eucyon_Tribes.Models.DTOs.WorldDTOs
{
    public class StoreWorldDTO
    {
        public string Name { get; }
        public List<Kingdom> Kingdoms { get; }
        public List<Location> Locations { get; }

        public StoreWorldDTO(string name, List<Kingdom> kingdoms, List<Location> locations)
        {
            Name = name;
            Kingdoms = kingdoms;
            Locations = locations;
        }
    }
}
