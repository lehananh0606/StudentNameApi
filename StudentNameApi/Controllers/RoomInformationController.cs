using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.ViewModel.Request;
using Service.ViewModel.Requet;
using System.Net;
using System.Threading.Tasks;

namespace StudentNameApi.Controllers
{
    [Route("api/room-management")]
    [ApiController]
    public class RoomInformationController : BaseController
    {
        private readonly IRoomInformationService _roomInformationService;

        public RoomInformationController(IRoomInformationService roomInformationService)
        {
            _roomInformationService = roomInformationService;
        }

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRoomInformation([FromQuery] string? keyword,
            [FromQuery] int? id,
            [FromQuery] string? RoomNumber,
            [FromQuery] string? RoomDetailDescription,
            [FromQuery] int? RoomMaxCapacity,
            [FromQuery] int? RoomTypeId,
            [FromQuery] byte? RoomStatus,
            [FromQuery] decimal? RoomPricePerDay,
            [FromQuery] string? orderBy,
            [FromQuery] bool? isAscending,
            [FromQuery] string[]? includeProperties,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var response = await _roomInformationService.GetAll(
                isAscending: isAscending,
                filter: x => (!id.HasValue || x.RoomId == id) &&
                             (string.IsNullOrEmpty(keyword) || x.RoomNumber.Contains(keyword)) &&
                             (string.IsNullOrEmpty(RoomNumber) || x.RoomNumber.Contains(RoomNumber)) &&
                             (string.IsNullOrEmpty(RoomDetailDescription) || x.RoomDetailDescription.Contains(RoomDetailDescription)) &&
                             (!RoomMaxCapacity.HasValue || x.RoomMaxCapacity == RoomMaxCapacity) &&
                             (!RoomTypeId.HasValue || x.RoomTypeId == RoomTypeId) &&
                             (!RoomStatus.HasValue || x.RoomStatus == RoomStatus) &&
                             (!RoomPricePerDay.HasValue || x.RoomPricePerDay == RoomPricePerDay),
                orderBy: orderBy,
                includeProperties: includeProperties,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        [HttpGet("rooms/{id:int}")]
        public async Task<IActionResult> GetRoomInformationById(int id)
        {
            var response = await _roomInformationService.GetById(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        [HttpPost("rooms")]
        public async Task<IActionResult> CreateRoomInformation([FromBody] RoomInformationRequestCreate requestCreateModel)
        {
            var response = await _roomInformationService.Create(requestCreateModel);
            return response.IsError
                ? HandleErrorResponse(response.Errors)
                : Created($"api/roominformation/{response.Payload.RoomId}", response);
        }

        [HttpPut("rooms/{id:int}")]
        public async Task<IActionResult> UpdateRoomInformation(int id, [FromBody] RoomInformationRequestCreate requestModel)
        {
            var response = await _roomInformationService.Update(id, requestModel);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        [HttpDelete("rooms/{id:int}")]
        public async Task<IActionResult> DeleteRoomInformation(int id)
        {
            var response = await _roomInformationService.Delete(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
