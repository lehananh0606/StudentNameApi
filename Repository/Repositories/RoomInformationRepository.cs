using Data.Models;
using Repository.GenericRepository;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class RoomInformationRepository : GenericRepository<RoomInformation>, IRoomInformationRepository
    {
        
        public RoomInformationRepository(FuminiHotelManagementContext context) : base(context) { }
       
    }
}
