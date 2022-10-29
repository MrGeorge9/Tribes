namespace Eucyon_Tribes.Models.DTOs.WorldDTOs
{
    public class UpdateWorldDTO
    {
        public string Name { get; }

        public UpdateWorldDTO(string name)
        {
            Name = name;
        }
    }
}
