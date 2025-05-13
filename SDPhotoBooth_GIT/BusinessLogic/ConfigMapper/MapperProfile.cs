using AutoMapper;
using BusinessLogic.DTO.BoothDTO;
using BusinessLogic.DTO.CoordinateDTO;
using BusinessLogic.DTO.CouponDTO;
using BusinessLogic.DTO.DepositDTO;
using BusinessLogic.DTO.DepositProductDTO;
using BusinessLogic.DTO.ExportProductDTO;
using BusinessLogic.DTO.FrameDTO;
using BusinessLogic.DTO.LevelMembershipDTO;
using BusinessLogic.DTO.LocationDTO;
using BusinessLogic.DTO.MembershipCardDTO;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.DTO.PaymentDTO;
using BusinessLogic.DTO.PaymentMethod;
using BusinessLogic.DTO.PhotoDTO;
using BusinessLogic.DTO.PhotoHistoryDTO;
using BusinessLogic.DTO.PhotoStyleDTO;
using BusinessLogic.DTO.SessionCodeDTO;
using BusinessLogic.DTO.StickerDTO;
using BusinessLogic.DTO.StickerStyleDTO;
using BusinessLogic.DTO.TransactionDTO;
using BusinessLogic.DTO.TypeSessionDTO;
using BusinessLogic.DTO.TypeSessionProductDTO;
using BusinessLogic.DTO.UserDTO;
using BusinessLogic.DTO.WalletDTO;
using BussinessObject.Models;

namespace BusinessLogic.ConfigMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LocationRequestDTO, Location>();
            CreateMap<Location, LocationResponseDTO>().ReverseMap();

            CreateMap<PaymentMethodRequestDTO, PaymentMethod>();
            CreateMap<PaymentMethod, PaymentMethodResponseDTO>();

            CreateMap<UserRequestDTO, User>();
            CreateMap<User, UserResponseDTO>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location != null ? new LocationResponseDTO
            {
                Id = src.Location.Id,
                LocationName = src.Location.LocationName,
                Address = src.Location.Address,
                CreatedAt = src.Location.CreatedAt,
                LastModified = src.Location.LastModified,
                CreatedById = src.Location.CreatedById,
                LastModifiedById = src.Location.LastModifiedById
            } : null));

            CreateMap<MembershipCard, MemberShipCardOnUserResponseDTO>() 
                .ForMember(dest => dest.CurrentLevel, opt => opt.MapFrom(src => src.LevelMemberShip != null ? src.LevelMemberShip.Name : null))
                .ForMember(dest => dest.CurrentPoint, opt => opt.MapFrom(src => src.Points))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<TypeSessionRequestDTO, TypeSession>();
            CreateMap<TypeSession, TypeSessionResponseDTO>();

            CreateMap<BoothRequestDTO, Booth>();
            CreateMap<Booth, BoothResponseDTO>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location != null ? new LocationResponseDTO
                {
                    Id = src.Location.Id,
                    LocationName = src.Location.LocationName,
                    Address = src.Location.Address,
                    CreatedAt = src.Location.CreatedAt,
                    LastModified = src.Location.LastModified,
                    CreatedById = src.Location.CreatedById,
                    LastModifiedById = src.Location.LastModifiedById
                } : null));

            CreateMap<CouponRequestDTO, Coupon>();
            CreateMap<Coupon, CouponResponseDTO>();

            CreateMap<SessionRequestDTO, Session>();
            CreateMap<Session, SessionResponseDTO>().ForMember(dest => dest.Expired,
                 opt => opt.MapFrom(src => src.Expired.HasValue
                                            ? src.Expired.Value.ToLocalTime()
                                            : (DateTime?)null));




            CreateMap<OrderRequestDTO, Order>();
            CreateMap<Order, OrderResponseDTO>();

            CreateMap<Order, OrderResponseDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Customer != null ? new UserResponseOnOrderDTO
                {
                    Id = src.Customer.Id,
                    Email = src.Customer.Email,
                    PhoneNumber = src.Customer.PhoneNumber
                } : null))
                .ForMember(dest => dest.SessionCode, opt => opt.MapFrom(src => src.Session != null ? src.Session.Code : null))
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.Payment != null ? src.Payment.PaymentMethod.MethodName : null))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Session != null ? src.Session.PhotoHistory.Booth.Location.LocationName : null))
                .ForMember(dest => dest.BoothName, opt => opt.MapFrom(src => src.Session != null ? src.Session.PhotoHistory.Booth.BoothName : null))
                .ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.Coupon != null ? src.Coupon.Code : null))
                .ForMember(dest => dest.TypeSessionName, opt => opt.MapFrom(src => src.TypeSession.Name != null ? src.TypeSession.Name : null));
            
            CreateMap<PhotoStyle, PhotoStyleResponseDTO>();
            CreateMap<PhotoStyleRequestDTO, PhotoStyle>();
            CreateMap<PhotostyleWithFormFileRequestDTO, PhotoStyleRequestDTO>();

            CreateMap<PhotoHistory, PhotoHistoryResponseDTO>()
                 .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Select(p => new PhotoResponseDTO
                 {
                     Url = p.Url,
                     PhotoStyleName = p.PhotoStyle.Name
                 }).ToArray()));

            CreateMap<StickerRequestDTO, Sticker>();
            CreateMap<Sticker, StickerResponseDTO>()
                .ForMember(dest => dest.StickerStyleName, opt => opt.MapFrom(src => src.StickerStyle != null ? src.StickerStyle.StickerStyleName : null));

            CreateMap<StickerStyleRequestDTO, StickerStyle>();
            CreateMap<StickerStyle, StickerStyleResponseDTO>()
                .ForMember(dest => dest.Stickers, opt => opt.MapFrom(src => src.Stickers.Select(s => new StickerOnStickerStyleResponseDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    StickerUrl = s.StickerUrl
                })));

            CreateMap<FrameStyleRequestDTO, FrameStyle>();
            CreateMap<FrameStyle, FrameStyleResponseDTO>();

            CreateMap<FrameRequestDTO, Frame>();
            CreateMap<Frame, FrameResponseDTO>()
                .ForMember(dest => dest.FrameStyleName, opt => opt.MapFrom(src => src.FrameStyle != null ? src.FrameStyle.Name : null));
            CreateMap<FrameWithFormFileDTO, FrameRequestDTO>();
            CreateMap<Coordinate, CordianteOnFrameResponseDTO>();


            CreateMap<CoordinateRequestDTO, Coordinate>();
            CreateMap<Coordinate, CoordinateResponseDTO>()
                .ForMember(dest => dest.FrameName, opt => opt.MapFrom(src => src.Frame != null ? src.Frame.Name : null));

            CreateMap<PaymentRequestDTO, Payment>();
            CreateMap<Payment, PaymentResponseDTO>()
               .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.PaymentMethod != null ? src.PaymentMethod.MethodName : null))
               .ForMember(dest => dest.OrderCode, opt => opt.MapFrom(src => src.Order != null ? src.Order.Code : (long?)null));

            CreateMap<TransactionRequestDTO, Transaction>();
            CreateMap<Transaction, TransactionResponseDTO>()
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.Payment != null && src.Payment.PaymentMethod != null ? src.Payment.PaymentMethod.MethodName : null))
               .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Payment.OrderId != null ? src.Payment.OrderId : (int?)null))
               .ForMember(dest => dest.DepositId, opt => opt.MapFrom(src => src.Payment.DepositId != null ? src.Payment.DepositId : (int?)null));

            CreateMap<DepositRequestDTO, Deposit>();
            CreateMap<Deposit, DepositResponseDTO>()
                .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.Payment != null && src.Payment.PaymentMethod != null ? src.Payment.PaymentMethod.MethodName : null));
            CreateMap<CreateLevelMembershipRequestDTO, LevelMembership>();
            CreateMap<LevelMembership, LevelMembershipResponseDTO>();

            CreateMap<CreateMembershipCardRequestDTO, MembershipCard>();
            CreateMap<MembershipCard, MembershipCardResponseDTO>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer != null ? new CustomerOnMembershipCard 
                {
                    Id = src.Customer.Id,
                    FullName = src.Customer.FullName,
                    Gender = src.Customer.Gender,
                    BirthDate = src.Customer.BirthDate,
                    Email = src.Customer.Email,
                    PhoneNumber = src.Customer.PhoneNumber
                } : null))
                .ForMember(dest => dest.LevelMemberShip, opt => opt.MapFrom(src => src.LevelMemberShip != null ? new LevelMembershipOnMembershipCard
                {
                    Id = src.LevelMemberShip.Id,
                    Name = src.LevelMemberShip.Name,
                    Point = src.LevelMemberShip.Point,
                    NextLevelId = src.LevelMemberShip.NextLevelId
                } : null));

            CreateMap<WalletRequestDTO, Wallet>();
            CreateMap<Wallet, WalletResponseDTO>();


            CreateMap<TypeSessionProductCreateRequestDTO, TypeSessionProduct>();
            CreateMap<TypeSessionProduct, TypeSessionProductResponseDTO>();

            CreateMap<DepositProductCreateRequestDTO,  DepositProduct>();
            CreateMap<DepositProduct, DepositProductResponseDTO>();

            CreateMap<TypeSessionProductResponseDTO, ExportProductResponseDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.typeSession.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.typeSession != null ? $"VN;{(long)((src.typeSession.Price - (src.Discount ?? 0)) * 1000000)}" : null));

            CreateMap<DepositProductResponseDTO, ExportProductResponseDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price != null ? $"VN;{(long)(src.Price * 1000000)}" : null));
        }
    }
}
