namespace Eucyon_Tribes.Models.DTOs.BuildingDTOs
{
    public class BuildingRequestDto
    {
        public int Building_type_id { get; }

        public BuildingRequestDto(int building_type_id)
        {
            Building_type_id = building_type_id;
        }
    }
}
