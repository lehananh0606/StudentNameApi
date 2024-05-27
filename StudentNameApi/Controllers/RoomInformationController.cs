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


        /// <summary>
        /// Get room information based on provided filters.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="id"></param>
        /// <param name="RoomNumber"></param>
        /// <param name="RoomDetailDescription"></param>
        /// <param name="RoomMaxCapacity"></param>
        /// <param name="RoomTypeId"></param>
        /// <param name="RoomStatus"></param>
        /// <param name="RoomPricePerDay"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAscending"></param>
        /// <param name="includeProperties"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns>
        /// A collection of room information matching the specified criteria.
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///     GET 
        ///     id=1
        ///     RoomNumber=101
        ///     RoomDetailDescription=Deluxe Room
        ///     RoomMaxCapacity=2
        ///     RoomTypeId=3
        ///     RoomStatus=1
        ///     RoomPricePerDay=100.00
        ///     orderBy=RoomNumber
        ///     isAscending=true
        ///     includeProperties=RoomType
        ///     pageIndex=0
        ///     pageSize=10
        /// </remarks>
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


        /// <summary>
        /// Get room information by its ID.
        /// </summary>
        /// <param name="id">The ID of the room.</param>
        /// <returns>The room information with the specified ID.</returns>
        [HttpGet("rooms/{id:int}")]
        public async Task<IActionResult> GetRoomInformationById(int id)
        {
            var response = await _roomInformationService.GetById(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Create a new room information.
        /// </summary>
        /// <param name="requestCreateModel">The model containing information to create the room.</param>
        /// <returns>The newly created room information.</returns>
        /// <remarks>
        /// Sample request:
        ///     POST 
        ///     {
        ///         "RoomNumber": "101",
        ///         "RoomDetailDescription": "Deluxe Room",
        ///         "RoomMaxCapacity": 2,
        ///         "RoomTypeId": 3,
        ///         "RoomStatus": 1,
        ///         "RoomPricePerDay": 100.00
        ///     }
        /// </remarks>
        /// <response code="201">Created new room information successfully.</response>
        [HttpPost("rooms")]
        public async Task<IActionResult> CreateRoomInformation([FromBody] RoomInformationRequestCreate requestCreateModel)
        {
            var response = await _roomInformationService.Create(requestCreateModel);
            return response.IsError
                ? HandleErrorResponse(response.Errors)
                : Created($"api/roominformation/{response.Payload.RoomId}", response);
        }


        /// <summary>
        /// Update an existing room information.
        /// </summary>
        /// <param name="id">The ID of the room to update.</param>
        /// <param name="requestModel">The model containing updated information for the room.</param>
        /// <returns>The updated room information.</returns>
        /// <remarks>
        /// Sample request:
        /// PUT
        /// {
        /// "RoomNumber": "101",
        /// "RoomDetailDescription": "Spacious room with city view",
        /// "RoomMaxCapacity": 2,
        /// "RoomTypeId": 1,
        /// "RoomStatus": 1,
        /// "RoomPricePerDay": 120.00
        /// }
        /// </remarks>
        [HttpPut("rooms/{id:int}")]
        public async Task<IActionResult> UpdateRoomInformation(int id, [FromBody] RoomInformationRequestCreate requestModel)
        {
            var response = await _roomInformationService.Update(id, requestModel);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Delete room information by its ID.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("rooms/{id:int}")]
        public async Task<IActionResult> DeleteRoomInformation(int id)
        {
            var response = await _roomInformationService.Delete(id);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }


        /// <summary>
        /// Search for rooms by keyword.
        /// </summary>
        /// <param name="keyword">The keyword to search for in room information.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A collection of rooms matching the search criteria.</returns>
        [HttpGet("rooms/search")]
        public async Task<IActionResult> SearchRooms([FromQuery] string keyword, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var response = await _roomInformationService.Search(keyword, pageIndex, pageSize);
            return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response);
        }
    }
}
