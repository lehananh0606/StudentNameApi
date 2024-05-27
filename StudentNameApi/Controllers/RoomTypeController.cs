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


        /// <summary>
        /// Get room types based on provided filters.
        /// </summary>
        /// <param name="keyword">The keyword to search for in room type names, descriptions, or notes.</param>
        /// <param name="id">The ID of the room type to retrieve.</param>
        /// <param name="roomTypeName">The name of the room type to search for.</param>
        /// <param name="typeDescription">The description of the room type to search for.</param>
        /// <param name="typeNote">The note of the room type to search for.</param>
        /// <param name="orderBy">The property to order the results by.</param>
        /// <param name="isAscending">Indicates whether to sort the results in ascending order.</param>
        /// <param name="includeProperties">The related entities to include in the result.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <returns>A collection of room types matching the specified criteria.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     keyword=double
        ///     id=1
        ///     roomTypeName=Double
        ///     typeDescription=Spacious
        ///     typeNote=Extra
        ///     orderBy=roomTypeName
        ///     isAscending=true
        ///     includeProperties=Rooms
        ///     pageIndex=0
        ///     pageSize=10
        /// </remarks>

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

        /// <summary>
        /// Get room type by ID.
        /// </summary>
        /// <param name="id">The ID of the room type.</param>
        /// <returns>The room type with the specified ID.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET 
        ///     id = 1
        /// </remarks>
        [HttpGet("room-types/{id:int}")]
        public async Task<IActionResult> GetRoomType(int id)
        {
            var response = await _roomTypeService.GetRoomType(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
