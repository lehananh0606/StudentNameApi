using AutoMapper;
using Microsoft.Extensions.Configuration;
using Repository.UnitOfWork;
using Service.IServices;
using Service.ViewModel.Request;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    public class BookingReportService : IBookingReportService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingReportService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<BookingReport> GetBookingReport(DateTime startDate, DateTime endDate)
        {
            var startDateOnly = new DateOnly(startDate.Year, startDate.Month, startDate.Day);
            var endDateOnly = new DateOnly(endDate.Year, endDate.Month, endDate.Day);

            var bookingDetails = _unitOfWork.bookingDetailRepository.Get(
                filter: bd => bd.StartDate >= startDateOnly && bd.EndDate <= endDateOnly,
                includeProperties: "BookingReservation"
            ).ToList();

            var report = _mapper.Map<IEnumerable<BookingReport>>(bookingDetails);

            return report;
        }
    }
}
