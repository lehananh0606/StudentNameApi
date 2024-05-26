using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RoomInformationRepository
    {
        private List<RoomInformation> _roomInformations;

        public RoomInformationRepository()
        {
            _roomInformations = new List<RoomInformation>();
        }

        public IEnumerable<RoomInformation> GetAllRoomInformations()
        {
            return _roomInformations;
        }

        public RoomInformation GetRoomInformationById(int id)
        {
            return _roomInformations.FirstOrDefault(r => r.RoomId == id);
        }

        public void AddRoomInformation(RoomInformation roomInformation)
        {
            _roomInformations.Add(roomInformation);
        }

        public void UpdateRoomInformation(RoomInformation roomInformation)
        {
            var existingRoomInformation = _roomInformations.FirstOrDefault(r => r.RoomId == roomInformation.RoomId);
            if (existingRoomInformation != null)
            {
                existingRoomInformation.RoomNumber = roomInformation.RoomNumber;
                existingRoomInformation.RoomDetailDescription = roomInformation.RoomDetailDescription;
                existingRoomInformation.RoomMaxCapacity = roomInformation.RoomMaxCapacity;
                existingRoomInformation.RoomTypeId = roomInformation.RoomTypeId;
                existingRoomInformation.RoomStatus = roomInformation.RoomStatus;
                existingRoomInformation.RoomPricePerDay = roomInformation.RoomPricePerDay;
            }
        }

        public void DeleteRoomInformation(int id)
        {
            var existingRoomInformation = _roomInformations.FirstOrDefault(r => r.RoomId == id);
            if (existingRoomInformation != null)
            {
                _roomInformations.Remove(existingRoomInformation);
            }
        }
    }
}
