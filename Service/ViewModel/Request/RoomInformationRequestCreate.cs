using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Service.ViewModel.Request
    {
        public class RoomInformationRequestCreate
        {
            public string RoomNumber { get; set; } = null!;

            public string? RoomDetailDescription { get; set; }

            public int? RoomMaxCapacity { get; set; }

            public int RoomTypeId { get; set; }

            public byte? RoomStatus { get; set; }

            public decimal? RoomPricePerDay { get; set; }
        }
    }

