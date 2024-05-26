using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RoomTypeRepository
    {
        private List<RoomType> _roomTypes;

        public RoomTypeRepository()
        {
            _roomTypes = new List<RoomType>();
        }

        public IEnumerable<RoomType> GetAllRoomTypes()
        {
            return _roomTypes;
        }

        public RoomType GetRoomTypeById(int id)
        {
            return _roomTypes.FirstOrDefault(r => r.RoomTypeId == id);
        }

        public void AddRoomType(RoomType roomType)
        {
            _roomTypes.Add(roomType);
        }

        public void UpdateRoomType(RoomType roomType)
        {
            var existingRoomType = _roomTypes.FirstOrDefault(r => r.RoomTypeId == roomType.RoomTypeId);
            if (existingRoomType != null)
            {
                existingRoomType.RoomTypeName = roomType.RoomTypeName;
                existingRoomType.TypeDescription = roomType.TypeDescription;
                existingRoomType.TypeNote = roomType.TypeNote;
            }
        }

        public void DeleteRoomType(int id)
        {
            var existingRoomType = _roomTypes.FirstOrDefault(r => r.RoomTypeId == id);
            if (existingRoomType != null)
            {
                _roomTypes.Remove(existingRoomType);
            }
        }
    }
}
