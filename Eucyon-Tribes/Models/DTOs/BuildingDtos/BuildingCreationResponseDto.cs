namespace Eucyon_Tribes.Models.DTOs.BuildingDTOs
{
    public class BuildingCreationResponseDto
    {
        public string Status { get; }

        public BuildingCreationResponseDto(string status)
        {
            Status = status;
        }
    }
}
