using System.ComponentModel.DataAnnotations;

namespace Service.ViewModel.Requet
{
    public class RoomTypeRequestCreate
    {
        [Required]
        public string RoomTypeName { get; set; } = null!;

        public string? TypeDescription { get; set; }

        public string? TypeNote { get; set; }
    }
}
