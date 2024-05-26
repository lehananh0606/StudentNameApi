using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModel.Response
{
    namespace Service.ViewModel.Response
    {
        public class RoomInformationResponse
        {
            public int RoomId { get; set; }

            public string RoomNumber { get; set; } = null!;

            public string? RoomDetailDescription { get; set; }

            public int? RoomMaxCapacity { get; set; }

            public int RoomTypeId { get; set; }

            public byte? RoomStatus { get; set; }

            public decimal? RoomPricePerDay { get; set; }
        }
    }

}
