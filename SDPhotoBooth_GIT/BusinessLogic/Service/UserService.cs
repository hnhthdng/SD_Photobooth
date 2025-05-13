using AutoMapper;
using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.LocationDTO;
using BusinessLogic.DTO.MembershipCardDTO;
using BusinessLogic.DTO.UserDTO;
using BusinessLogic.DTO.WalletDTO;
using BusinessLogic.Service.IService;
using BusinessLogic.Utils;
using BussinessObject.Enums;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace BusinessLogic.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILocationService _locationService;
        private readonly IWalletService _walletService;
        private readonly IMembershipCardService _membershipCardService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IMapper mapper, UserManager<User> userManager, ILocationService locationService,
            IUnitOfWork unitOfWork, IWalletService walletService, IMembershipCardService membershipCardService, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _locationService = locationService;
            _unitOfWork = unitOfWork;
            _walletService = walletService;
            _membershipCardService = membershipCardService;
            _roleManager = roleManager;
        }

        public async Task<int> GetCount()
        {
            var users = await _userManager.Users.Where(u => !u.IsDeleted).CountAsync();
            return users;
        }   

        public async Task<UserResponseDTO> BanUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.IsBanned = true;
            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> UnbanUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.IsBanned = false;
            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> CreateUser(UserRequestDTO userRequestDTO)
        {
            var isExist = await _userManager.FindByEmailAsync(userRequestDTO.Email);
            if (isExist != null)
            {
                return null;
            }
            var user = _mapper.Map<User>(userRequestDTO);
            if(userRequestDTO.Role != UserType.Staff)
            {
                user.LocationId = null;
            }

            var result = await _userManager.CreateAsync(user, userRequestDTO.Password);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create new user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRoleAsync(user, userRequestDTO.Role.ToString());
            if(userRequestDTO.Role == UserType.Customer)
            {
                await _walletService.CreateWallet(user.Id, new WalletRequestDTO());
                await _membershipCardService.Create(new CreateMembershipCardRequestDTO() {
                    CustomerId = user.Id,
                    LevelMemberShipId = 1,
                    Points = 0,
                    IsActive = true
                });
            }
            
            var createdUser = await _userManager.Users
            .Include(u => u.MembershipCard)
            .ThenInclude(m => m.LevelMemberShip)
                .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == user.Id);

            var userDTO = _mapper.Map<UserResponseDTO>(createdUser);
            userDTO.Role = userRequestDTO.Role.ToString();
            userDTO.Location = userRequestDTO.LocationId.HasValue ? _mapper.Map<LocationResponseDTO>(await _locationService.GetLocation(userRequestDTO.LocationId.Value)) : null;
            return userDTO;
        }

        public async Task<(IEnumerable<UserResponseDTO> Data, int TotalCount)> GetAllManager(PaginationParams? pagination)
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            var totalCount = managers.Count;

            var query = managers.AsQueryable();

            if (pagination != null)
            {
                var (isPaged, pageNumber, pageSize) = pagination.Validate();
                if (isPaged)
                {
                    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
            }

            var data = query.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Gender = u.Gender,
                BirthDate = u.BirthDate,
                Avatar = u.Avatar,
                Role = "Manager",
                IsBanned = u.IsBanned
            });

            return (data.ToList(), totalCount);
        }
        public async Task<(IEnumerable<UserResponseDTO> Data, int TotalCount)> GetAllStaff(PaginationParams? pagination)
        {
            var staffList = await _userManager.GetUsersInRoleAsync("Staff");
            var totalCount = staffList.Count;

            var query = staffList.AsQueryable();
            if (pagination != null)
            {
                var (isPaged, pageNumber, pageSize) = pagination.Validate();
                if (isPaged)
                {
                    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
            }

            var users = query.ToList();

            foreach (var user in users)
            {
                if (user.LocationId.HasValue)
                {
                    var locationEntity = await _locationService.GetLocation(user.LocationId.Value);
                    user.Location = _mapper.Map<Location>(locationEntity);
                }
            }

            var data = users.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Gender = u.Gender,
                BirthDate = u.BirthDate,
                Avatar = u.Avatar,
                Role = "Staff",
                IsBanned = u.IsBanned,
                Location = _mapper.Map<LocationResponseDTO>(u.Location)
            });

            return (data.ToList(), totalCount);
        }
        public async Task<(IEnumerable<UserResponseDTO> Data, int TotalCount)> GetAllCustomer(PaginationParams? pagination)
        {
            var customers = await _userManager.GetUsersInRoleAsync("Customer");
            var customerIds = customers.Select(c => c.Id).ToList();
            var totalCount = customerIds.Count;

            var query = _userManager.Users
                .Where(u => customerIds.Contains(u.Id))
                .Include(u => u.Wallet)
                .Include(u => u.MembershipCard)
                    .ThenInclude(m => m.LevelMemberShip)
                .AsQueryable();

            if (pagination != null)
            {
                var (isPaged, pageNumber, pageSize) = pagination.Validate();
                if (isPaged)
                {
                    query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
            }

            var users = await query.ToListAsync();

            var data = users.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Gender = u.Gender,
                BirthDate = u.BirthDate,
                Avatar = u.Avatar,
                Role = "Customer",
                IsBanned = u.IsBanned,
                Wallet = _mapper.Map<WalletResponseDTO>(u.Wallet),
                MembershipCard = _mapper.Map<MemberShipCardOnUserResponseDTO>(u.MembershipCard)
            });

            return (data.ToList(), totalCount);
        }


        public async Task<UserResponseDTO> UserDetail(string email)
        {
            var user = await _userManager.Users
             .Include(u => u.MembershipCard)
             .ThenInclude(m => m.LevelMemberShip)
                .Include(u => u.Wallet)
             .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            var userDTO = _mapper.Map<UserResponseDTO>(user);
            var roleOfUser = await _userManager.GetRolesAsync(user);
            userDTO.Role = roleOfUser.FirstOrDefault() ?? "No Role";
            userDTO.Location = user.LocationId.HasValue ? _mapper.Map<LocationResponseDTO>(await _locationService.GetLocation(user.LocationId.Value)) : null;
            userDTO.Wallet = _mapper.Map<WalletResponseDTO>(user.Wallet);
            userDTO.MembershipCard = _mapper.Map<MemberShipCardOnUserResponseDTO>(user.MembershipCard);
            return userDTO;
        }

        public async Task<bool> ChangeRole(string email, UserType role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role.ToString());
            return true;
        }

        public async Task<bool> MoveLocation(string email, int locationId)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            user.LocationId = locationId;
            await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task<TotalUserStaticResponseDTO> StaticUserCreated(StaticType staticType)
        {
            var totalUserStatic = new TotalUserStaticResponseDTO();
            (DateTime start, DateTime startPrev) = TimeRangeHelper.GetTimeRange(staticType);

            totalUserStatic.TotalUser = await _unitOfWork.User.CountAsync(s => s.CreatedAt >= start && s.CreatedAt < start.Add(TimeRangeHelper.GetStepSize(staticType)) && !s.IsBanned && !s.IsDeleted);
            totalUserStatic.TotalUserPrev = await _unitOfWork.User.CountAsync(s => s.CreatedAt >= startPrev && s.CreatedAt < start && !s.IsBanned && !s.IsDeleted);

            return totalUserStatic;
        }

        public async Task<UserResponseDTO> UserDetailById(string Id)
        {
            var user = await _userManager.Users
             .Include(u => u.MembershipCard)
             .ThenInclude(m => m.LevelMemberShip)
                .Include(u => u.Wallet)
             .FirstOrDefaultAsync(u => u.Id == Id);
            if (user == null)
            {
                return null;
            }
            var userDTO = _mapper.Map<UserResponseDTO>(user);
            var roleOfUser = await _userManager.GetRolesAsync(user);
            userDTO.Role = roleOfUser.FirstOrDefault() ?? "No Role";
            userDTO.Location = user.LocationId.HasValue ? _mapper.Map<LocationResponseDTO>(await _locationService.GetLocation(user.LocationId.Value)) : null;
            userDTO.Wallet = _mapper.Map<WalletResponseDTO>(user.Wallet);
            userDTO.MembershipCard = _mapper.Map<MemberShipCardOnUserResponseDTO>(user.MembershipCard);
            return userDTO;
        }

        public async Task<RevenueStaffStatisticsResponseDTO> StaticRevenueStaffs(GroupingType staticType)
        {
            var today = DateTime.Today;

            var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
            var managerUsers = await _userManager.GetUsersInRoleAsync("Manager");
            var staffs = staffUsers.Concat(managerUsers).DistinctBy(s => s.Id).ToList();
            var staffIds = staffs.Select(s => s.Id).ToList();
            var staffIdSet = staffIds.ToHashSet();

            var orders = staticType switch
            {
                GroupingType.Day => await _unitOfWork.Order.GetAllAsync(
                    o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date >= today.AddDays(-29) && staffIdSet.Contains(o.CreatedById), asNoTracking: true),
                GroupingType.Month or GroupingType.Quarter => await _unitOfWork.Order.GetAllAsync(
                    o => o.CreatedAt.HasValue && o.CreatedAt.Value.Year == today.Year && staffIdSet.Contains(o.CreatedById), asNoTracking: true),
                GroupingType.Year => await _unitOfWork.Order.GetAllAsync(
                    o => o.CreatedAt.HasValue && staffIdSet.Contains(o.CreatedById), asNoTracking: true),
                _ => Enumerable.Empty<Order>()
            };

            return new RevenueStaffStatisticsResponseDTO
            {
                GroupingType = staticType,
                Data = GroupRevenueStaffs(staffs, orders, staticType)
            };
        }

        private List<RevenueStaffDTO> GroupRevenueStaffs(List<User> staffs, IEnumerable<Order> orders, GroupingType grouping)
        {
            var staffDict = staffs.ToDictionary(s => s.Id, s => s.Email);

            var validOrders = orders
                .Where(s => s.CreatedAt.HasValue)
                .Select(s => new
                {
                    Id = s.CreatedById,
                    Name = staffDict[s.CreatedById],
                    Amount = s.Amount,
                    CreatedAt = s.CreatedAt!.Value
                });

            var grouped = validOrders.GroupBy(s => new
            {
                s.Id,
                Day = s.CreatedAt.Date,
                Month = s.CreatedAt.Month,
                Quarter = (s.CreatedAt.Month - 1) / 3 + 1,
                Year = s.CreatedAt.Year
            });

            return grouped.Select(g =>
            {
                var key = g.Key;
                return new RevenueStaffDTO
                {
                    Id = key.Id,
                    Day = grouping == GroupingType.Day ? key.Day : null,
                    Month = grouping == GroupingType.Month ? key.Month : null,
                    Quarter = grouping == GroupingType.Quarter ? key.Quarter : null,
                    Year = grouping is GroupingType.Year or GroupingType.Quarter or GroupingType.Month ? key.Year : null,
                    Name = g.First().Name,
                    TotalRevenue = g.Sum(o => o.Amount),
                    Count = g.Count()
                };
            }).ToList();
        }

        public async Task<RevenueStaffStatisticsResponseDTO> StaticRevenue(RevenueFilterDTO revenueFilterDTO)
        {
            var today = DateTime.Today;

            var staff = await _userManager.Users
                .Where(u => !u.IsDeleted && !u.IsBanned && u.Id == revenueFilterDTO.StaffId)
                .FirstOrDefaultAsync();

            Expression<Func<Order, bool>> filter = revenueFilterDTO.StaticType switch
            {
                GroupingType.Day => o => o.CreatedAt != null && o.CreatedAt.Value.Date >= today.AddDays(-29) && o.CreatedById == staff.Id,
                GroupingType.Month or GroupingType.Quarter => o => o.CreatedAt != null && o.CreatedAt.Value.Year == today.Year && o.CreatedById == staff.Id,
                GroupingType.Year => o => o.CreatedAt != null && o.CreatedById == staff.Id,
                _ => o => false
            };

            var orders = await _unitOfWork.Order.GetAllAsync(filter, asNoTracking: true);

            return new RevenueStaffStatisticsResponseDTO
            {
                GroupingType = revenueFilterDTO.StaticType,
                Data = GroupRevenueStaff(staff, orders, revenueFilterDTO.StaticType)
            };
        }

        private List<RevenueStaffDTO> GroupRevenueStaff(User staff, IEnumerable<Order> orders, GroupingType grouping)
        {
            var validOrders = orders
                .Where(o => o.CreatedAt != null)
                .Select(o => new
                {
                    Id = o.CreatedById,
                    Name = staff.Email,
                    Amount = o.Amount,
                    CreatedAt = o.CreatedAt!.Value
                });

            var grouped = validOrders.GroupBy(o => new
            {
                o.Id,
                Day = o.CreatedAt.Date,
                Month = o.CreatedAt.Month,
                Quarter = (o.CreatedAt.Month - 1) / 3 + 1,
                Year = o.CreatedAt.Year
            });

            return grouped.Select(g =>
            {
                var key = g.Key;
                return new RevenueStaffDTO
                {
                    Id = key.Id,
                    Name = staff.Email,
                    Day = grouping == GroupingType.Day ? key.Day : null,
                    Month = grouping == GroupingType.Month ? key.Month : null,
                    Quarter = grouping == GroupingType.Quarter ? key.Quarter : null,
                    Year = grouping is GroupingType.Year or GroupingType.Quarter or GroupingType.Month ? key.Year : null,
                    TotalRevenue = g.Sum(x => x.Amount),
                    Count = g.Count()
                };
            }).ToList();
        }
    }
}
