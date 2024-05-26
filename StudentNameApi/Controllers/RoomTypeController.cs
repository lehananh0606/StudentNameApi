using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.ViewModel.Requet;
using System.Net;

namespace StudentNameApi.Controllers
{
    [Route("api/room-type-management")]
    [ApiController]
    public class RoomTypeController : BaseController
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        [HttpGet("room-types")]
        public async Task<IActionResult> GetRoomTypes([FromQuery] string? keyword,
            [FromQuery] int? id,
            [FromQuery] string? roomTypeName,
            [FromQuery] string? typeDescription,
            [FromQuery] string? typeNote,
            [FromQuery] string? orderBy,
            [FromQuery] bool? isAscending,
            [FromQuery] string[]? includeProperties,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10)
        {
            var response = await _roomTypeService.GetAllRoomTypes(
                isAscending: isAscending,
                filter: x => (!id.HasValue || x.RoomTypeId == id) &&
                         (string.IsNullOrEmpty(keyword) || x.RoomTypeName.Contains(keyword) ||
                          (x.TypeDescription != null && x.TypeDescription.Contains(keyword)) ||
                          (x.TypeNote != null && x.TypeNote.Contains(keyword))) &&
                         (string.IsNullOrEmpty(roomTypeName) || x.RoomTypeName.Contains(roomTypeName)) &&
                         (string.IsNullOrEmpty(typeDescription) || (x.TypeDescription != null && x.TypeDescription.Contains(typeDescription))) &&
                         (string.IsNullOrEmpty(typeNote) || (x.TypeNote != null && x.TypeNote.Contains(typeNote))),
                orderBy: orderBy,
                includeProperties: includeProperties,
                pageIndex: pageIndex,
                pageSize: pageSize
            );
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }

        [HttpGet("room-types/{id:int}")]
        public async Task<IActionResult> GetRoomType(int id)
        {
            var response = await _roomTypeService.GetRoomType(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
