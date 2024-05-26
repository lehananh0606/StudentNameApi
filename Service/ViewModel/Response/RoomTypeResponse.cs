namespace Service.ViewModel.Response
{
    public class RoomTypeResponse
    {
        public int RoomTypeId { get; set; }

        public string RoomTypeName { get; set; } = null!;

        public string? TypeDescription { get; set; }

        public string? TypeNote { get; set; }
    }
}
