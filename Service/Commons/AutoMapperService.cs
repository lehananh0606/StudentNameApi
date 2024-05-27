using AutoMapper;
using Service.ViewModel.Requet;
using Data.Models;
using Service.ViewModel.Response;
using Service.ViewModel.Request;
using Service.ViewModel.Response.Service.ViewModel.Response;


namespace Service.Commons
{
    public class AutoMapperService : Profile
    {

        public AutoMapperService(
        )
        {
            CreateMap<LoginRequest, Customer>().ReverseMap();
            CreateMap<AccountRequestCreate, Customer>()
                .ForMember(dest => dest.CustomerBirthday, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CustomerBirthday)));
            CreateMap<Customer, LoginResponse>().ReverseMap();
            CreateMap<Customer, AccountResponse>().ReverseMap();

            CreateMap<BookingDetail, BookingReport>()
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue)))
                 .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.BookingReservation.TotalPrice))
                 .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.BookingReservation.CustomerId))
                 .ForMember(dest => dest.BookingStatus, opt => opt.MapFrom(src => src.BookingReservation.BookingStatus));

            CreateMap<RoomTypeRequestCreate, RoomType>();
            CreateMap<RoomType, RoomTypeResponse>();

            CreateMap<BookingDetailRequest, BookingDetail>();
            CreateMap<BookingDetail, BookingDetailResponse>();

            CreateMap<RoomInformationRequestCreate, RoomInformation>();
            CreateMap<RoomInformation, RoomInformationResponse>();

            CreateMap<BookingReservationRequestCreate, BookingReservation>();
            CreateMap<BookingReservation, BookingReservationResponse>();
        }
    }
}


// Example
/*CreateMap<TransactionHistoryDTO, TransactionHistory>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => _unitOfWork.CustomerRepo.GetById(src.CustomerId)))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => _unitOfWork.PaymentRepo.GetById(src.PaymentId)))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => _unitOfWork.AccountTypeRepo.GetById(src.AccountTypeId)))
                .AfterMap(async (src, dest, context) =>
                {
                    var accountType = await _unitOfWork.AccountTypeRepo.GetByIdAsync(src.AccountTypeId);
                    dest.AccountType = accountType;

                    var payment = await _unitOfWork.PaymentRepo.GetByIdAsync(src.PaymentId);
                    dest.Payment = payment;
                })
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.PaymentDate))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.PaymentDate.AddDays(30)))
                 .ReverseMap();*/