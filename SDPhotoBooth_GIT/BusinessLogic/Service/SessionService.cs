using AutoMapper;
using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.DTO.SessionCodeDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Enums;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Service
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<int> GetCount()
        {
            var sessions = await _unitOfWork.Session.GetAllAsync();
            return sessions.Count();
        }
        public async Task<SessionResponseDTO> CreateSession(long orderCode)
        {
            if (await _unitOfWork.Order.GetFirstOrDefaultAsync(o => o.Code == orderCode) == null)
            {
                return null;
            }

            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(o => o.Code == orderCode, includeProperties:"TypeSession");
            if (order.Status != OrderStatus.Completed)
            {
                return null;
            }

            var existingSession = await _unitOfWork.Session.GetFirstOrDefaultAsync(
                s => s.OrderId == order.Id);

            if (existingSession != null)
            {
                return _mapper.Map<SessionResponseDTO>(existingSession);
            }

            var newSession = new Session();
            newSession.OrderId = order.Id;
            newSession.AbleTakenNumber = order.TypeSession.AbleTakenNumber;

            await _unitOfWork.Session.AddAsync(newSession);
            await _unitOfWork.SaveAsync();

            var sessionWithDetails = await _unitOfWork.Session.GetFirstOrDefaultAsync(
                s => s.Code == newSession.Code);

            return _mapper.Map<SessionResponseDTO>(sessionWithDetails);
        }

        public async Task<IEnumerable<SessionResponseDTO>> GetAllSessions(PaginationParams? pagination)
        {
            var allSessions = await _unitOfWork.Session.GetAllAsync(pagination: pagination);
            return _mapper.Map<IEnumerable<SessionResponseDTO>>(allSessions);
        }


        public async Task<IEnumerable<SessionResponseDTO>> SearchSessionByCode(string code)
        {
            var session = await _unitOfWork.Session.GetAllAsync(s => s.Code.ToLower().Contains(code.ToLower()));
            return _mapper.Map<IEnumerable<SessionResponseDTO>>(session);
        }

        public async Task<SessionResponseDTO> GetSessionById(int Id)
        {
            var session = await _unitOfWork.Session.GetFirstOrDefaultAsync(s => s.Id == Id);
            return _mapper.Map<SessionResponseDTO>(session);
        }

        public async Task<SessionResponseDTO> GetSessionByCode(string code)
        {
            var session = await _unitOfWork.Session.GetFirstOrDefaultAsync(
                s => EF.Functions.Collate(s.Code, "Latin1_General_CS_AS") == code
            );

            return _mapper.Map<SessionResponseDTO>(session);
        }

        public async Task<SessionResponseDTO> UseSession(string code, int? boothId)
        {
            var session = await _unitOfWork.Session.GetFirstOrDefaultAsync(s => s.Code == code, includeProperties:"Order,Order.TypeSession");

            if (!session.IsActive)
            {
                session.IsActive = true;
                session.Expired = DateTime.Now.AddMinutes(session.Order.TypeSession.Duration + 1);

                // Create photo history
                var photoHistory = new PhotoHistory() {
                    CustomerId = session.Order.CustomerId,
                    SessionId = session.Id,
                    BoothId = boothId,
                };

                await _unitOfWork.PhotoHistory.AddAsync(photoHistory);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                if (session.Expired < DateTime.Now)
                {
                    return null;
                }
            }
            return _mapper.Map<SessionResponseDTO>(session);
        }

        public async Task<SessionResponseDTO> UpdateSessionAbleTaken(string code)
        {
            var session = await _unitOfWork.Session.GetFirstOrDefaultAsync(s => s.Code == code);
            if (session != null && session.AbleTakenNumber > 0)
            {
                session.AbleTakenNumber -= 1;
                _unitOfWork.Session.Update(session);
                _unitOfWork.SaveAsync();
                return _mapper.Map<SessionResponseDTO>(session);
            }

            return null;
        }

        public async Task<UsageChannelStatisticsResponseDTO> StaticUsageChannel(UsageChannelFilterDTO filter)
        {
            var today = DateTime.Today;
            string includeProps = filter.ChannelGroupingType == ChannelGroupingType.Location
                ? "PhotoHistory.Booth.Location"
                : "PhotoHistory.Booth";

            IEnumerable<Session> sessions = filter.StaticType switch
            {
                GroupingType.Day => await _unitOfWork.Session.GetAllAsync(
                    s => (filter.ChannelGroupingType == ChannelGroupingType.Booth && (s.PhotoHistory.Booth.LocationId == filter.LocationId) 
                    || filter.ChannelGroupingType == ChannelGroupingType.Location) && s.Expired.HasValue && s.Expired.Value.Date >= today.AddDays(-29),
                    includeProperties: includeProps, asNoTracking: true),

                GroupingType.Month or GroupingType.Quarter => await _unitOfWork.Session.GetAllAsync(
                    s => (filter.ChannelGroupingType == ChannelGroupingType.Booth && (s.PhotoHistory.Booth.LocationId == filter.LocationId)
                    || filter.ChannelGroupingType == ChannelGroupingType.Location) && s.Expired.HasValue && s.Expired.Value.Year == today.Year,
                    includeProperties: includeProps, asNoTracking: true),

                GroupingType.Year => await _unitOfWork.Session.GetAllAsync(
                    s => (filter.ChannelGroupingType == ChannelGroupingType.Booth && (s.PhotoHistory.Booth.LocationId == filter.LocationId)
                    || filter.ChannelGroupingType == ChannelGroupingType.Location) && s.Expired.HasValue,
                    includeProperties: includeProps, asNoTracking: true),

                _ => Enumerable.Empty<Session>()
            };

            var groupedData = GroupUsageChannel(sessions, filter.ChannelGroupingType, filter.StaticType);
            return new UsageChannelStatisticsResponseDTO
            {
                GroupingType = filter.StaticType,
                Data = groupedData
            };
        }

        private List<UsageChannelDTO> GroupUsageChannel(IEnumerable<Session> sessions, ChannelGroupingType channelType, GroupingType grouping)
        {
            var validSessions = sessions
                .Where(s => s.Expired.HasValue)
                .Select(s => new
                {
                    Name = GetGroupName(s, channelType),
                    Expired = s.Expired.Value
                });

            var grouped = validSessions.GroupBy(s => new
            {
                s.Name,
                Day = s.Expired.Date,
                Month = s.Expired.Month,
                Quarter = (s.Expired.Month - 1) / 3 + 1,
                Year = s.Expired.Year
            });

            return grouped.Select(g =>
            {
                var key = g.Key;
                return new UsageChannelDTO
                {
                    Name = key.Name,
                    Day = grouping == GroupingType.Day ? key.Day : null,
                    Month = grouping == GroupingType.Month ? key.Month : null,
                    Quarter = grouping == GroupingType.Quarter ? key.Quarter : null,
                    Year = grouping == GroupingType.Year || grouping == GroupingType.Quarter || grouping == GroupingType.Month ? key.Year : null,
                    TotalUsage = g.Count()
                };
            }).ToList();
        }

        private string GetGroupName(Session s, ChannelGroupingType type)
        {
            return type switch
            {
                ChannelGroupingType.Booth => s.PhotoHistory?.Booth?.BoothName ?? "Mobile",
                ChannelGroupingType.Location => s.PhotoHistory?.Booth?.Location?.LocationName ?? "Mobile",
                _ => "Unknown"
            };
        }
    }
}