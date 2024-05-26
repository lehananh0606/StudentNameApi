using AutoMapper;
using Data.Models;
using Repository.UnitOfWork;
using Service.Commons;
using Service.IServices;
using Service.ViewModel.Request;
using Service.ViewModel.Requet;
using Service.ViewModel.Response;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BookingReservationService : IBookingReservationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BookingReservationService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<IEnumerable<BookingReservationResponse>>> GetAllBookingReservations(
            bool? isAscending,
            string? orderBy = null,
            Expression<Func<BookingReservation, bool>>? filter = null,
            string[]? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10)
        {
            var result = new OperationResult<IEnumerable<BookingReservationResponse>>();
            try
            {
                var entities = _unitOfWork.bookingRepository.FilterAll(
                    isAscending, orderBy, filter, includeProperties, pageIndex, pageSize);
                result.Payload = _mapper.Map<IEnumerable<BookingReservationResponse>>(entities);
                result.StatusCode = StatusCode.Ok;
                result.Message = "Get all booking reservations successfully";
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<BookingReservationResponse>> GetBookingReservationById(int bookingReservationId)
        {
            var result = new OperationResult<BookingReservationResponse>();
            try
            {
                var entity = await _unitOfWork.bookingRepository.GetByIdAsync(bookingReservationId);
                result.Payload = _mapper.Map<BookingReservationResponse>(entity);
                result.StatusCode = StatusCode.Ok;
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<BookingReservationResponse>> CreateBookingReservation(BookingReservationRequestCreate requestCreate)
        {
            var result = new OperationResult<BookingReservationResponse>();
            try
            {
                var newEntity = _mapper.Map<BookingReservation>(requestCreate);
                await _unitOfWork.bookingRepository.AddAsync(newEntity);
                var count = await _unitOfWork.SaveChangesAsync();

                if (count > 0)
                {
                    result.Payload = _mapper.Map<BookingReservationResponse>(newEntity);
                    result.StatusCode = StatusCode.Created;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Create booking reservation failed");
                }
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<bool>> DeleteBookingReservation(int id)
        {
            var result = new OperationResult<bool>();
            try
            {
                var entity = await _unitOfWork.bookingRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    result.AddError(StatusCode.NotFound, "Booking reservation not found");
                    return result;
                }

                _unitOfWork.bookingRepository.SoftRemove(entity);
                var count = await _unitOfWork.SaveChangesAsync();
                if (count > 0)
                {
                    result.Payload = true;
                    result.StatusCode = StatusCode.Ok;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Delete booking reservation failed");
                }
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }

        public async Task<OperationResult<BookingReservationResponse>> UpdateBookingReservation(int id, BookingReservationRequestCreate requestUpdate)
        {
            var result = new OperationResult<BookingReservationResponse>();
            try
            {
                var entity = await _unitOfWork.bookingRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    result.AddError(StatusCode.NotFound, "Booking reservation not found");
                    return result;
                }

                _mapper.Map(requestUpdate, entity);
                _unitOfWork.bookingRepository.Update(entity);
                var count = await _unitOfWork.SaveChangesAsync();

                if (count > 0)
                {
                    result.Payload = _mapper.Map<BookingReservationResponse>(entity);
                    result.StatusCode = StatusCode.Ok;
                }
                else
                {
                    result.AddError(StatusCode.BadRequest, "Update booking reservation failed");
                }
            }
            catch (Exception ex)
            {
                result.AddUnknownError(ex.Message);
                throw;
            }

            return result;
        }
    }
}

