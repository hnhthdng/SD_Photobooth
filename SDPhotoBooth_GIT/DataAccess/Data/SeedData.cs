using BussinessObject.Enums;
using BussinessObject.Models;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

public static class SeedData
{
    public static class ProductIdHelper
    {
        public static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string noDiacritics = RemoveDiacritics(input);

            return noDiacritics
                .Trim()
                .ToLower()
                .Replace(" ", "_")
                .Replace("__", "_");
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }

    public static async Task Seed(AIPhotoboothDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var random = new Random();

        Random rand = new Random();
        var now = DateTime.UtcNow;

        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            #region Seed data for Location
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Location] ON");

            if (!context.Location.Any())
            {
                context.Location.AddRange(
                     new Location { Id = 1, LocationName = "Ho Chi Minh City", Address = "123 Central Avenue, District 1, Ho Chi Minh City", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 2, LocationName = "Hanoi", Address = "456 East Street, Hoan Kiem District, Hanoi", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 3, LocationName = "Da Nang", Address = "789 West Avenue, Hai Chau District, Da Nang", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 4, LocationName = "Quy Nhon", Address = "12 Mountain Road, Quy Hoa, Quy Nhon", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 5, LocationName = "Da Lat", Address = "34 Lake View, Xuan An Ward, Da Lat", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 6, LocationName = "Can Tho", Address = "56 Le Loi, Ninh Kieu District, Can Tho", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 7, LocationName = "Hai Phong", Address = "78 Tran Phu, Ngo Quyen District, Hai Phong", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 8, LocationName = "Vung Tau", Address = "22 Nguyen An Ninh, Ward 1, Vung Tau", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 9, LocationName = "Nha Trang", Address = "15 Tran Phu, Vinh Hai, Nha Trang", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false },
                     new Location {Id = 10, LocationName = "Hue", Address = "89 Phan Dinh Phung, Phu Hoa Ward, Hue", CreatedAt = DateTime.UtcNow, LastModified = DateTime.UtcNow, IsDeleted = false }
                 );


                context.SaveChanges();
            }
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Location] OFF");
            #endregion

            #region Seed data for Role
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            if (!await roleManager.RoleExistsAsync("Staff"))
            {
                await roleManager.CreateAsync(new IdentityRole("Staff"));
            }
            if (!await roleManager.RoleExistsAsync("Manager"))
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }
            #endregion

            #region Seed data for User

            if (!context.Users.Any())
            {
                var password = "User123";

                var users = new List<User>
                {
                    new User {
                        UserName     = "phannguyenxuanhien",
                        Email        = "hienphannguyenxuan@gmail.com",
                        FullName     = "phannguyenxuanhien",
                        BirthDate    = new DateTime(2003, 1, 1),
                        PhoneNumber  = "07774567890",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "raikungfu",
                        Email        = "raikungfu@gmail.com",
                        FullName     = "raikungfu",
                        BirthDate    = new DateTime(2003, 12, 1),
                        PhoneNumber  = "07774567891",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "doannguyenhuyentrang",
                        Email        = "doannguyenhuyentrang9303@gmail.com",
                        FullName     = "doannguyenhuyentrang",
                        BirthDate    = new DateTime(2003, 12, 12),
                        PhoneNumber  = "07774567892",
                        Gender       = BussinessObject.Enums.UserGender.Female,
                        CreatedAt    = now.AddDays(-1),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "raikungfu98",
                        Email        = "raikungfu98@gmail.com",
                        FullName     = "raikungfu98",
                        BirthDate    = new DateTime(2003, 11, 12),
                        PhoneNumber  = "07774567891",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-2),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "sinhhoctebao0903",
                        Email        = "sinhhoctebao0903@gmail.com",
                        FullName     = "sinhhoctebao0903",
                        BirthDate    = new DateTime(2003, 8, 12),
                        PhoneNumber  = "07774567892",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-3),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false,
                        LocationId = 3
                    },
                    new User {
                        UserName     = "vovannam641",
                        Email        = "vovannam641@gmail.com",
                        FullName     = "vovannam641",
                        BirthDate    = new DateTime(2003, 6, 12),
                        PhoneNumber  = "07774567893",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-4),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "phuongtbde160476",
                        Email        = "phuongtbde160476@fpt.edu.vn",
                        FullName     = "phuongtbde160476",
                        BirthDate    = new DateTime(2003, 6, 12),
                        PhoneNumber  = "07774567894",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-5),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "trangdnhqe170154",
                        Email        = "trangdnhqe170154@fpt.edu.vn",
                        FullName     = "trangdnhqe170154",
                        BirthDate    = new DateTime(2003, 6, 12),
                        PhoneNumber  = "07774567895",
                        Gender       = BussinessObject.Enums.UserGender.Female,
                        CreatedAt    = now.AddDays(-6),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false,
                        LocationId = 2
                    },
                    new User {
                        UserName     = "hienpnxqe170189",
                        Email        = "hienpnxqe170189@fpt.edu.vn",
                        FullName     = "hienpnxqe170189",
                        BirthDate    = new DateTime(2003, 6, 12),
                        PhoneNumber  = "07774567896",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-7),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "staff.sd.photobooth",
                        Email        = "staff@sdphotobooth.com",
                        FullName     = "Staff SD Photobooth",
                        BirthDate    = new DateTime(2003, 6, 12),
                        PhoneNumber  = "07774567897",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-8),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false,
                        LocationId = 1
                    },
                    new User {
                        UserName     = "trang.doan.2003",
                        Email        = "trang.doan.2003@gmail.com",
                        FullName     = "trang.doan.2003",
                        BirthDate    = new DateTime(2003, 6, 12),
                        PhoneNumber  = "07774567897",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-9),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "admin.sd.photobooth",
                        Email        = "sdphotobooth.ai@gmail.com",
                        FullName     = "Admin SD Photobooth",
                        BirthDate    = new DateTime(2003, 6, 12),
                        PhoneNumber  = "07774567897",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-9),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                    new User {
                        UserName     = "manager.sd.photobooth",
                        Email        = "manager@sdphotobooth.com",
                        FullName     = "Manager SD Photobooth",
                        BirthDate    = new DateTime(2003, 6, 12),
                        PhoneNumber  = "07774567897",
                        Gender       = BussinessObject.Enums.UserGender.Male,
                        CreatedAt    = now.AddDays(-9),
                        LastModified = DateTime.UtcNow,
                        IsDeleted    = false,
                        IsBanned     = false
                    },
                };

                var roles = new Dictionary<string, string>
                {
                    { "phannguyenxuanhien", "Admin" },
                    { "raikungfu", "Admin" },
                    { "doannguyenhuyentrang", "Admin" },
                    { "admin.sd.photobooth", "Admin" },
                    { "raikungfu98", "Manager" },
                    { "vovannam641", "Manager" },
                    { "trang.doan.2003", "Manager" },
                    { "manager.sd.photobooth", "Manager" },
                    { "sinhhoctebao0903", "Staff" },
                    { "trangdnhqe170154", "Staff" },
                    { "staff.sd.photobooth", "Staff" },
                    { "hienpnxqe170189", "Customer" },
                    { "phuongtbde160476", "Customer" }
                };

                foreach (var user in users)
                {
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, roles[user.UserName]);
                    }
                    else
                    {
                        throw new Exception($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }

                //Dummy Data
                int counter = 100;

                for (int i = 1; i <= 20; i++)
                {
                    var user = new User
                    {
                        UserName = $"admin_gen_{i}",
                        Email = $"admin_gen_{i}@example.com",
                        PhoneNumber = $"090000{counter++:D3}",
                        Gender = BussinessObject.Enums.UserGender.Male,
                        FullName = $"admin_gen_{i}",
                        BirthDate = new DateTime(2003, 8, 8),
                        CreatedAt = now.AddDays(rand.Next(-720,-10)),
                        LastModified = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "Admin");
                }

                for (int i = 1; i <= 20; i++)
                {
                    var user = new User
                    {
                        UserName = $"manager_gen_{i}",
                        Email = $"manager_gen_{i}@example.com",
                        PhoneNumber = $"090000{counter++:D3}",
                        Gender = BussinessObject.Enums.UserGender.Male,
                        FullName = $"manager_gen_{i}",
                        BirthDate = new DateTime(2003, 7, 7),
                        CreatedAt = now.AddDays(rand.Next(-720, -10)),
                        LastModified = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "Manager");
                }

                for (int i = 1; i <= 20; i++)
                {
                    var user = new User
                    {
                        UserName = $"staff_gen_{i}",
                        Email = $"staff_gen_{i}@example.com",
                        PhoneNumber = $"090000{counter++:D3}",
                        Gender = BussinessObject.Enums.UserGender.Female,
                        FullName = $"staff_gen_{i}",
                        BirthDate = new DateTime(2003, 6, 6),
                        CreatedAt = now.AddDays(rand.Next(-720, -10)),
                        LastModified = DateTime.UtcNow,
                        IsDeleted = false,
                        LocationId = random.Next(1, 11),
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "Staff");
                }

                for (int i = 1; i <= 20; i++)
                {
                    var user = new User
                    {
                        UserName = $"customer_gen_{i}",
                        Email = $"customer_gen_{i}@example.com",
                        PhoneNumber = $"090000{counter++:D3}",
                        Gender = BussinessObject.Enums.UserGender.Male,
                        FullName = $"customer_gen_{i}",
                        BirthDate = new DateTime(2003, 6, 6),
                        CreatedAt = now.AddDays(rand.Next(-720, -10)),
                        LastModified = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "Customer");
                }

                
            }

            var admin = await userManager.FindByNameAsync("phannguyenxuanhien");
            var admin2 = await userManager.FindByNameAsync("raikungfu");
            var admin3 = await userManager.FindByNameAsync("admin.sd.photobooth");
            var manager = await userManager.FindByNameAsync("manager.sd.photobooth");
            var manager2 = await userManager.FindByNameAsync("raikungfu98");
            var manager3 = await userManager.FindByNameAsync("vovannam641");
            var staff = await userManager.FindByNameAsync("trangdnhqe170154");
            var staff2 = await userManager.FindByNameAsync("staff.sd.photobooth");
            var staff3 = await userManager.FindByNameAsync("sinhhoctebao0903");
            var customer = await userManager.FindByNameAsync("phuongtbde160476");
            var customer2 = await userManager.FindByNameAsync("hienpnxqe170189");

            #endregion

            #region Seed data for Wallet
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Wallet] ON");
            if (!context.Wallet.Any())
            {
                //Create wallet for customer
                var customers = await userManager.GetUsersInRoleAsync("Customer");
                var wallets = new List<Wallet>();
                int nextId = 1;
                foreach (var customerUser in customers)
                {
                    wallets.Add(new Wallet
                    {
                        Id = nextId++,
                        CustomerId = customerUser.Id,
                        Balance = random.Next(100_000, 1_000_000),
                        IsLocked = false
                    });
                }
                context.Wallet.AddRange(wallets);
                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Wallet] OFF");

            #endregion

            #region Seed data for LevelMembership
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [LevelMembership] ON");

            if (!context.LevelMembership.Any())
            {
                context.LevelMembership.AddRange(
                new LevelMembership { Id = 5, Name = "Platinum", Description = "Platinum membership level", Point = 1000000, DiscountPercent = 0.20m, MaxDiscount = 10000, MinOrder = 10000, IsActive = true, CreatedById = admin.Id, LastModifiedById = admin.Id },
                new LevelMembership { Id = 4, NextLevelId = 5, Name = "Gold", Description = "Gold membership level", Point = 600000, DiscountPercent = 0.15m, MaxDiscount = 7500, MinOrder = 10000, IsActive = true, CreatedById = admin.Id, LastModifiedById = admin.Id },
                new LevelMembership { Id = 3, NextLevelId = 4, Name = "Silver", Description = "Silver membership level", Point = 300000, DiscountPercent = 0.10m, MaxDiscount = 5000, MinOrder = 10000, IsActive = true, CreatedById = admin.Id, LastModifiedById = admin.Id },
                new LevelMembership { Id = 2, NextLevelId = 3, Name = "Bronze", Description = "Bronze membership level", Point = 100000, DiscountPercent = 0.05m, MaxDiscount = 2500, MinOrder = 10000, IsActive = true, CreatedById = admin.Id, LastModifiedById = admin.Id },
                new LevelMembership { Id = 1, NextLevelId = 2, Name = "Basic", Description = "Basic membership level", Point = 0, DiscountPercent = 0.02m, MaxDiscount = 1000, MinOrder = 10000, IsActive = true, CreatedById = admin.Id, LastModifiedById = admin.Id }
                );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [LevelMembership] OFF");

            #endregion

            #region Seed data for MembershipCard
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [MembershipCard] ON");
            if (!context.MembershipCard.Any())
            {
                var customers = await userManager.GetUsersInRoleAsync("Customer");
                var membershipCards = new List<MembershipCard>();
                int nextId = 1;

                foreach (var customerUser in customers)
                {
                    int levelId = random.Next(1, 6);
                    membershipCards.Add(new MembershipCard
                    {
                        Id = nextId++,
                        CustomerId = customerUser.Id,
                        LevelMemberShipId = levelId,
                        Points = random.Next(100_000, 500_000),
                        Description = $"Membership card cấp {levelId} cho {customerUser.UserName}",
                        IsActive = true,
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    });
                }
                context.MembershipCard.AddRange(membershipCards);
                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [MembershipCard] OFF");

            #endregion

            #region Seed data for Booth

            if (!context.Booth.Any())
            {
                var locations = context.Location
                .Select(l => new { l.Id, l.LocationName })
                .ToList();

                var booths = new List<Booth>();

                foreach (var loc in locations)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        booths.Add(new Booth
                        {
                            Id = rand.Next(100000000, 999999999),
                            LocationId = loc.Id,
                            BoothName = $"Booth {loc.LocationName}-{i}",
                            Description = $"Booth {i} tại {loc.LocationName}",
                            Status = true,
                            CreatedAt = DateTime.UtcNow,
                            LastModified = DateTime.UtcNow,
                            IsDeleted = false,
                            CreatedById = manager.Id,
                            LastModifiedById = manager.Id
                        });
                    }
                }
                context.Booth.AddRange(booths);
                context.SaveChanges();
            }

            #endregion

            #region Seed data for PaymentMethod
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [PaymentMethod] ON");

            if (!context.PaymentMethod.Any())
            {
                context.PaymentMethod.AddRange(
                    new PaymentMethod { Id = 1, MethodName = "Wallet", ForMobile = true, IsOnline = true, Description = "Internal wallet balance", IsActive = true, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new PaymentMethod { Id = 2, MethodName = "Cash", Description = "Pay with cash upon delivery", IsActive = true, IsOnline = false, ForMobile = false, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new PaymentMethod { Id = 3, MethodName = "Banking", IsOnline = true, ForMobile = false, Description = "Bank transfer via online banking", IsActive = true, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new PaymentMethod { Id = 4, MethodName = "Google Billing", ForMobile = true, IsOnline = true, Description = "Momo e-wallet", IsActive = true, CreatedById = admin.Id, LastModifiedById = admin.Id }
                );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [PaymentMethod] OFF");

            #endregion

            #region Seed data for PhotoStyle
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [PhotoStyle] ON");

            if (!context.PhotoStyle.Any())
            {
                context.PhotoStyle.AddRange(
                    new PhotoStyle
                    {
                        Id = 1,
                        Name = "ID Photo",
                        Description = "Professional ID photo with studio lighting and sharp focus.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/idphoto.jpg?alt=media",
                        Prompt = "a person wearing a formal suit, white shirt, professional portrait, studio lighting, upper body, front view, neutral expression, clean background, high quality, photorealistic",
                        NegativePrompt = "blurry, low quality, cartoon, 3d, deformed, extra arms, bad hands, exaggerated makeup, jewelry, messy hair, hat.",
                        //Prompt = "Super Portrait, professional ID photo, studio lighting, sharp focus, neutral background, straight-on angle, natural skin texture, neutral expression, straight posture, ultra-high resolution.",
                        //NegativePrompt = "blur, distortion, extra hair, heavy makeup, artificial skin smoothing, exaggerated features, harsh shadows, unrealistic retouching, incorrect posture, head tilt.",
                        Controlnets = null,
                        Mode = InpaintMode.KeepFace,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 20,
                        GuidanceScale = 4.0,
                        Strength = 0.3,
                        BackgroundRemover = true,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:04:20.5382419"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    //new PhotoStyle
                    //{
                    //    Id = 2,
                    //    Name = "ID Photo (ControlNet)",
                    //    Description = "Professional ID photo with enhanced details using ControlNet.",
                    //    ImageUrl = "https://cdn.tgdd.vn/Files/2023/07/10/1537629/cachkhacphucloitaianh4x6lenhochieuonline-130723-145414-800-resize.jpg",
                    //    Prompt = "Super Portrait, professional ID photo, studio lighting, sharp focus, neutral background, ultra-high resolution, natural skin texture, neutral expression.",
                    //    NegativePrompt = "blur, distortion, extra hair, heavy makeup, artificial skin smoothing, exaggerated features, harsh shadows, unrealistic retouching.",
                    //    Controlnets = "softedge, depth",
                    //    Mode = InpaintMode.KeepStructure,
                    //    NumImagesPerGen = 6,
                    //    Height = null,
                    //    Width = null,
                    //    NumInferenceSteps = 30,
                    //    GuidanceScale = 7.5,
                    //    Strength = 0.8,
                    //    FaceImage = true,
                    //    BackgroundRemover = true,
                    //    CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                    //    LastModified = DateTime.Parse("2025-03-27T08:06:00.0000000"),
                    //    CreatedById = admin.Id,
                    //    LastModifiedById = admin.Id
                    //},
                    new PhotoStyle
                    {
                        Id = 2,
                        Name = "Anime",
                        Description = "Anime-style portrait with refined details, vibrant colors, and balanced composition.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/anime.jpg?alt=media",
                        Prompt = "Create a high-quality anime image featuring a youthful character with bright eyes and soft gradients. Use vibrant colors and perfect symmetry to enhance the character's appeal. The background should be detailed and colorful, complementing the character's design while maintaining a cohesive anime aesthetic.",
                        NegativePrompt = "ugly, extra fingers, mutated hands, extra limbs, long neck, watermark, text, logo, Rough lines, muted colors, photorealistic details, missing parts, extra limbs, or blurry outlines.",
                        //Prompt = "High-quality anime illustration, smooth shading, expressive eyes, perfect symmetry, vibrant color palette. The background is detailed yet balanced, complementing the character's design.",
                        //NegativePrompt = "blurred outlines, muted colors, photorealistic textures, distorted anatomy, excessive shading.",
                        Controlnets = "pose, canny",
                        Mode = InpaintMode.KeepStructure, 
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 26, 
                        GuidanceScale = 12.0,
                        Strength = 0.9,
                        BackgroundRemover = true,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:06:00.0000000"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 3,
                        Name = "Fashion Style",
                        Description = "Editorial fashion portrait with high-definition details and elegant outfit.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/nature.jpg?alt=media",
                        //Prompt = "Editorial fashion portrait, striking pose, high-definition details, elegant outfit, perfect fit, confident expression, soft studio lighting. The background is clean and minimalistic, enhancing the subject.",
                        //NegativePrompt = "bad lighting, ill-fitting clothes, low-quality textures, unnatural expression, distorted proportions, cluttered background.",
                        Prompt = "Generate a high-fashion editorial image showcasing a model with radiant skin and a trendy outfit. The outfit should be perfectly fitted and elegant, with a striking pose that conveys confidence. Use soft studio lighting to enhance the model's features, and ensure the background is minimalistic to keep the focus on the fashion.",
                        NegativePrompt = "ugly, extra fingers, mutated hands, extra limbs, long neck, watermark, text, logo, messy hair, cheap fabrics, bad fit, wrinkles, dull colors, or distracting elements in the background.",
                        Controlnets = "canny, pose",
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        IPAdapterScale = 0.5,
                        NumInferenceSteps = 26,
                        GuidanceScale = 9.0,
                        Strength = 0.8,
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:16:22.4802536"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 4,
                        Name = "Cartoon",
                        Description = "Vibrant cartoon character with smooth shading and bold outlines.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/cartoon.jpg?alt=media",
                        //Prompt = "Vibrant cartoon character, smooth shading, expressive eyes, bold outlines, dynamic pose. The background is colorful and playful, enhancing the lively personality.",
                        //NegativePrompt = "blurred lines, dull colors, photorealistic details, excessive shading, missing body parts, distorted proportions.",
                        Prompt = "Create a cartoon character with smooth features, big eyes, sharp outlines, and bright colors. The background should be playful and colorful. The character should have a lively expression and dynamic pose, capturing the essence of a classic cartoon style.",
                        NegativePrompt = "ugly, extra fingers, mutated hands, extra limbs, long neck, watermark, text, logo, rough textures, distorted, realistic anatomy, muted colors, blurred lines, missing body parts, unnatural proportions.",
                        Controlnets = "pose, canny",
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 32,
                        GuidanceScale = 12,
                        Strength = 0.9,
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:10:24.5847326"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 5,
                        Name = "Artistic",
                        Description = "Fine art portrait with soft lighting and warm color harmony.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/ar.png?alt=media",
                        //Prompt = "Fine art portrait, soft and balanced lighting, rich textures, warm color harmony. The subject exudes elegance and emotion, resembling classical masterpieces.",
                        //NegativePrompt = "oversaturated, artificial blur, harsh contrast, distorted proportions, lifeless expression, missing elements.",
                        Prompt = "Design a masterpiece portrait with elegant features and soft lighting. The colors should be balanced and glowing, creating a dreamy aesthetic. The subject should be posed gracefully, and the overall composition should evoke a sense of tranquility and beauty, reminiscent of classical art.",
                        NegativePrompt = "ugly, extra fingers, mutated hands, extra limbs, long neck, watermark, text, logo, harsh shadows, rough textures, dull colors, distortions, missing parts, or excessive blurring.",
                        Controlnets = "pose, depth",
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 28,
                        IPAdapterScale = 0.5,
                        GuidanceScale = 7.0,
                        Strength = 0.5,
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:06:34.262168"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 6,
                        Name = "Vintage",
                        Description = "Classic vintage portrait with warm sepia tones and subtle grain texture.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/vintage.jpg?alt=media",
                        //Prompt = "Classic vintage portrait, warm sepia tones, soft film-like contrast, subtle grain texture. The subject appears timeless with natural lighting and elegant styling.",
                        //NegativePrompt = "modern sharpness, digital artifacts, unnatural lighting, reflections, artificial blur, excessive grain.",
                        Prompt = "Create a classic vintage photo with a sepia tone and soft contrast. The image should evoke a nostalgic feel, featuring smooth skin and a natural expression. Use high-quality textures to enhance the vintage look, and ensure the background complements the overall aesthetic without being distracting.",
                        NegativePrompt = "ugly, extra fingers, mutated hands, extra limbs, long neck, watermark, text, logo, modern sharpness, digital artifacts, unnatural lighting, reflections, excessive grain, or aged skin.",
                        Controlnets = "pose, canny",
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 25,
                        IPAdapterScale = 0.5,
                        GuidanceScale = 7.5,
                        Strength = 0.6,
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:34:08.9543789"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 7,
                        Name = "Fantasy",
                        Description = "Ethereal fantasy portrait with glowing atmosphere and magical details.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/fantasy.jpg?alt=media",
                        //Prompt = "Ethereal fantasy portrait, glowing atmosphere, intricate costume details, vibrant magical background. The subject radiates an enchanting aura with a dreamlike aesthetic.",
                        //NegativePrompt = "lifeless expression, dull tones, distorted anatomy, harsh shadows, CGI artifacts, unnatural blur.",
                        Prompt = "Generate an enchanting fantasy scene featuring a radiant character with an ethereal glow. Use soft lighting to create a magical aura, and focus on delicate and elegant details in the character's attire. The background should be a whimsical landscape, filled with vibrant colors and fantastical elements.",
                        NegativePrompt = "ugly, extra fingers, mutated hands, extra limbs, long neck, watermark, text, logo, harsh shadows, dull colors, distortions, missing parts, or lifeless expressions.",
                        Controlnets = "pose, depth",
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 35,
                        IPAdapterScale = 0.5,
                        GuidanceScale = 8.0,
                        Strength = 0.6,
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 8,
                        Name = "Modern",
                        Description = "Sleek modern portrait with high-definition textures and stylish composition.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/modern.jpg?alt=media",
                        Prompt = "Create a futuristic style image featuring a sleek model in a stylish outfit. Use sharp lighting to highlight the model's features and ensure perfect symmetry in the composition. The background should be modern and minimalistic, with vibrant colors that enhance the overall editorial feel of the shot.",
                        NegativePrompt = "ugly, extra fingers, mutated hands, extra limbs, long neck, watermark, text, logo, old-fashioned elements, dull colors, messy hair, asymmetry, wrinkles, or noisy backgrounds.",
                        //Prompt = "Sleek modern portrait, sharp lighting, high-definition textures, dynamic composition, stylish outfit. The background is minimalistic with a futuristic touch.",
                        //NegativePrompt = "low resolution, washed-out colors, bad lighting, asymmetry, distorted shapes, excessive blur.",
                        Controlnets = "canny, depth",
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 25,
                        IPAdapterScale = 0.5,
                        GuidanceScale = 7.5,
                        Strength = 0.5,
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:27:24.4834447"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 9,
                        Name = "Nature",
                        Description = "Natural outdoor portrait with warm sunlight and scenic backgrounds.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/wp.jpg?alt=media",
                        Prompt = "Design an outdoor photography scene with a model showcasing glowing skin and wind-blown hair. Use soft sunlight to create a fresh atmosphere, and incorporate vibrant colors in the natural surroundings. The setting should be peaceful and serene, capturing the beauty of nature.",
                        NegativePrompt = "ugly, extra fingers, mutated hands, extra limbs, long neck, watermark, text, logo, dry skin, dull tones, harsh shadows, distortions, cluttered backgrounds, or artificial lighting.",
                        //Prompt = "Natural outdoor portrait, warm sunlight, soft breeze, fresh atmosphere, glowing skin. The background features lush greenery or scenic landscapes, complementing the subject.",
                        //NegativePrompt = "harsh lighting, artificial colors, dull tones, cluttered background, distortion, missing details.",
                        Controlnets = "pose, depth",
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 30,
                        IPAdapterScale = 0.5,
                        GuidanceScale = 8.0,
                        Strength = 0.6,
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:29:05.8312544"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 10,
                        Name = "Cyberpunk", 
                        Description = "Futuristic cyberpunk aesthetic with neon lights and high-tech details.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/cyberpunk.webp19c68ca8-c452-4ce0-97cd-061d18a957dc?alt=media",
                        Prompt = "Futuristic cyberpunk aesthetic, neon lights, high-tech details, glowing cityscape, cybernetic enhancements.",
                        NegativePrompt = "low contrast, dull colors, historical elements, medieval designs, washed-out lighting.",
                        Controlnets = "depth, canny", 
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 30,
                        GuidanceScale = 7.5,
                        Strength = 0.5,   // 0.78 -> 0.5
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:38:00.0000000"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 11,
                        Name = "Watercolor Painting",
                        Description = "Soft and elegant watercolor painting with delicate brush strokes.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Watercolor.jpge2aa55c6-9cc2-4bcc-a6e5-7cdf5385c5b0?alt=media",
                        Prompt = "Soft and elegant watercolor painting, delicate brush strokes, smooth color blending, artistic expression.",
                        NegativePrompt = "pixelated edges, harsh shadows, digital artifacts, rigid outlines, unrealistic proportions.",
                        Controlnets = "pose, depth", // softedge,scribble -> pose, depth
                        Mode = null,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 25,
                        IPAdapterScale = 0.5,
                        GuidanceScale = 8.0,  //6.8 -> 8.0
                        Strength = 0.6,     //0.72 -> 0.6
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:39:00.0000000"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 12,
                        Name = "Black & White Classic",
                        Description = "Timeless black and white portrait with deep contrast and classic lighting.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/blackwhite.jpg5d9c012c-d715-4fb8-8970-e8b5b5af7ff2?alt=media",
                        Prompt = "Timeless black and white portrait, deep contrast, classic lighting, elegant composition.",
                        NegativePrompt = "grainy texture, washed-out details, weak contrast, digital noise, unbalanced lighting.",
                        Controlnets = "pose, canny", //depth, canny -> pose,canny
                        Mode = InpaintMode.KeepStructure,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 22,
                        IPAdapterScale = 0.5,
                        GuidanceScale = 7.5, //6.2 -> 7.5
                        Strength = 0.6, //0.65 -> 0.6
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:40:00.0000000"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    },
                    new PhotoStyle
                    {
                        Id = 13,
                        Name = "Oil Painting",
                        Description = "Rich and textured oil painting style with thick brush strokes.",
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/oilpaint.jpgf9e0162a-2c9d-45e0-a70b-87d2f9f3c7b1?alt=media",
                        Prompt = "Rich and textured oil painting style, thick brush strokes, deep color saturation, artistic rendering.",
                        NegativePrompt = "flat colors, digital sharpness, pixelation, unrealistic lighting, blurred edges.",
                        Controlnets = "pose, canny", //scribble, softedge -> pose, canny
                        Mode = null,
                        NumImagesPerGen = 6,
                        Height = null,
                        Width = null,
                        NumInferenceSteps = 27,
                        IPAdapterScale = 0.5,
                        GuidanceScale = 12.0, //7.3 -> 12
                        Strength = 0.9, //0.76 -> 0.9
                        BackgroundRemover = false,
                        CreatedAt = DateTime.Parse("2025-03-14T05:45:36.871683"),
                        LastModified = DateTime.Parse("2025-03-27T08:41:00.0000000"),
                        CreatedById = admin.Id,
                        LastModifiedById = admin.Id
                    }
            );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [PhotoStyle] OFF");

            #endregion
            
            #region Seed data for StickerStyle
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [StickerStyle] ON");

            if (!context.StickerStyle.Any())
            {
                context.StickerStyle.AddRange(
                    new StickerStyle { Id = 1, StickerStyleName = "Cute", Description = "Adorable and playful stickers", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 2, StickerStyleName = "Minimal", Description = "Simple and clean sticker designs", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 3, StickerStyleName = "Retro", Description = "Vintage and classic themed stickers", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 4, StickerStyleName = "Nature", Description = "Leaves, trees, and natural elements", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 5, StickerStyleName = "Emoji", Description = "Expressive emoji-style stickers", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 6, StickerStyleName = "Cartoon", Description = "Fun and colorful cartoon stickers", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 7, StickerStyleName = "Food", Description = "Food and drink-themed stickers", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 8, StickerStyleName = "Animal", Description = "Cute animal illustrations", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 9, StickerStyleName = "Holiday", Description = "Christmas, Halloween, and festive themes", CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new StickerStyle { Id = 10, StickerStyleName = "Love", Description = "Hearts, kisses, and romantic designs", CreatedById = admin.Id, LastModifiedById = admin.Id }
                );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [StickerStyle] OFF");

            #endregion

            #region Seed data for Sticker
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Sticker] ON");

            if (!context.Sticker.Any())
            {
                context.Sticker.AddRange(
                new Sticker { Id = 1, Name = "Pray", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/pray.png?alt=media", StickerStyleId = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 2, Name = "Coffee", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/coffee.png?alt=media", StickerStyleId = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 3, Name = "Cloud", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/cloud.png?alt=media", StickerStyleId = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 4, Name = "Bean", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/bean.png?alt=media", StickerStyleId = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 5, Name = "Ice_cream", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/ice-cream.png?alt=media", StickerStyleId = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 6, Name = "Movie_Sticket", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/movie-ticket.png?alt=media", StickerStyleId = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 7, Name = "Plant", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/plant.png?alt=media", StickerStyleId = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 8, Name = "Rainbow", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/rainbow.png?alt=media", StickerStyleId = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 9, Name = "Smile", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/smile.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 10, Name = "Wink", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/wink.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 11, Name = "Cool", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/cool.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 12, Name = "Scare", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/scare.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 13, Name = "Proud", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/gold-star.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 14, Name = "Swag", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/dab.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 15, Name = "Angry", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/angry.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 16, Name = "Confused", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/confused.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 17, Name = "Happy", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/happy.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 18, Name = "Love", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/love.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 19, Name = "Sad", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/sad.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 20, Name = "Sick", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/sick.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 21, Name = "Sleep", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/sleep.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 22, Name = "Smiley", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/smiley.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 23, Name = "Star", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/star.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 24, Name = "Suprises", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/surprised.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 25, Name = "Vain", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/vain.png?alt=media", StickerStyleId = 5, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 26, Name = "Death", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/angry.png?alt=media", StickerStyleId = 6, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 27, Name = "Apple", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/apple.png?alt=media", StickerStyleId = 6, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 28, Name = "Fire", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/fire.png?alt=media", StickerStyleId = 6, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 29, Name = "Ghost", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/ghost.png?alt=media", StickerStyleId = 6, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 30, Name = "Mushroom", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/mushroom.png?alt=media", StickerStyleId = 6, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 31, Name = "Avocado", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/avocado.png?alt=media", StickerStyleId = 7, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 32, Name = "French-fire", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/french-fries.png?alt=media", StickerStyleId = 7, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 33, Name = "Egg", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/fried-egg.png?alt=media", StickerStyleId = 7, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 34, Name = "Sushi", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/japanese-food.png?alt=media", StickerStyleId = 7, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 35, Name = "Noodle", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/noodle.png?alt=media", StickerStyleId = 7, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 36, Name = "Pancake", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/pancake.png?alt=media", StickerStyleId = 7, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 37, Name = "Sandwich", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/sandwich.png?alt=media", StickerStyleId = 7, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 38, Name = "Bee", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/bee.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 39, Name = "Dog", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/corgi.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 40, Name = "Dragon", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Dragon_read.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 41, Name = "Racoon eat", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/eat.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 42, Name = "Frog", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/frog.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 43, Name = "Rabbit", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/full.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 44, Name = "Racoon hi", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/hi.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 45, Name = "Panda", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/panda.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 46, Name = "Panda1", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/panda%20%281%29.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 47, Name = "Unicorn", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/unicorn.png?alt=media", StickerStyleId = 8, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 48, Name = "Halloween", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/castle-house.png?alt=media", StickerStyleId = 9, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 49, Name = "Christmas", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/christmas.png?alt=media", StickerStyleId = 9, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 50, Name = "Holly", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/holly.png?alt=media", StickerStyleId = 9, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 51, Name = "Suitcase1", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/suitcase%20%281%29.png?alt=media", StickerStyleId = 9, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 52, Name = "Suitcase", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/suitcase.png?alt=media", StickerStyleId = 9, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 53, Name = "Umbrella", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/sun-umbrella.png?alt=media", StickerStyleId = 9, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 54, Name = "Cassette_love", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/cassette-tape.png?alt=media", StickerStyleId = 10, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 55, Name = "Love1", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/love%20%281%29.png?alt=media", StickerStyleId = 10, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 56, Name = "Love2", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/love-badge.png?alt=media", StickerStyleId = 10, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 57, Name = "Love3", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/love-you.png?alt=media", StickerStyleId = 10, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 58, Name = "Love4", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/love.png?alt=media", StickerStyleId = 10, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 59, Name = "Love5", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/wink.png?alt=media", StickerStyleId = 10, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 60, Name = "Cupid", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/cupid.png?alt=media", StickerStyleId = 10, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                new Sticker { Id = 61, Name = "Wedding", StickerUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/wedding-couple.png?alt=media", StickerStyleId = 10, CreatedAt = DateTime.Parse("2025-03-28 13:15:23"), LastModified = DateTime.Parse("2025-03-28 13:15:23"), IsDeleted = false, CreatedById = null, LastModifiedById = null }
            );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Sticker] OFF");

            #endregion

            #region Seed data for FrameStyle
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [FrameStyle] ON");

            if (!context.FrameStyle.Any())
            {
                context.FrameStyle.AddRange(
                    new FrameStyle { Id = 1, Name = "Wedding", Description = "Elegant, romantic wooden frame, perfect for wedding photobooths", ImageUrl = "https://mlf2vmoci1kz.i.optimole.com/w:auto/h:auto/q:mauto/f:best/https://finetemplate.com/wp-content/uploads/2024/02/Mr-Mrs-Bergin-19th-august-2023-copy-Mockup-1-copy-scaled.jpg", CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new FrameStyle { Id = 2, Name = "Party", Description = "Colorful, vibrant frame that creates a fun atmosphere for party photobooths", ImageUrl = "https://i.etsystatic.com/25728422/r/il/f059c8/2728468113/il_570xN.2728468113_huxk.jpg", CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new FrameStyle { Id = 3, Name = "Retro", Description = "Vintage frame with a nostalgic feel for retro-themed photobooths", ImageUrl = "https://i.etsystatic.com/33935117/r/il/ef952a/3657377352/il_570xN.3657377352_sg9z.jpg", CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new FrameStyle { Id = 4, Name = "Holiday", Description = "Festive frame with cheerful decorations ideal for seasonal photobooths", ImageUrl = "https://i.etsystatic.com/21809061/r/il/10cd17/4430822420/il_570xN.4430822420_dmz3.jpg", CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new FrameStyle { Id = 5, Name = "Corporate", Description = "Sleek, modern frame designed for professional photobooths at corporate events", ImageUrl = "https://www.prostarra.com/wp-content/uploads/2018/08/sqsp-simple-modern-white-6x4-photo-booth-template-3.jpg", CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new FrameStyle { Id = 6, Name = "School", Description = "A fun, youthful frame style perfect for school events and graduation photobooths", ImageUrl = "https://i.etsystatic.com/21809061/r/il/5bfe4d/6014778330/il_570xN.6014778330_m1tp.jpg", CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new FrameStyle { Id = 7, Name = "Travel", Description = "A dynamic frame style capturing the spirit of adventure, ideal for travel photobooths", ImageUrl = "https://i.etsystatic.com/22697668/r/il/c2ae6d/5439314654/il_fullxfull.5439314654_9oxo.jpg", CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null }
                );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [FrameStyle] OFF");


            #endregion

            #region Seed data for Frame
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Frame] ON");

            if (!context.Frame.Any())
            {
                context.Frame.AddRange(
                    new Frame { Id = 1, Name = "HappyBirthday", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Frame1_Happy%20Birthday.png?alt=media", FrameStyleId = 2, SlotCount = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 2, Name = "Instagram", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Frame1_Instagram.png?alt=media", FrameStyleId = 5, SlotCount = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 3, Name = "Japan", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Japan.png?alt=media", FrameStyleId = 7, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 4, Name = "Happy Wedding", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Wedding.png?alt=media", FrameStyleId = 1, SlotCount = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 5, Name = "Graduation", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Graduation.png?alt=media", FrameStyleId = 6, SlotCount = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 6, Name = "90's Style", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Retro.png?alt=media", FrameStyleId = 3, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 7, Name = "Zoo", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Zoo.png?alt=media", FrameStyleId = 7, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 8, Name = "Merry Christmas", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Merry%20Christmas.png?alt=media", FrameStyleId = 4, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 9, Name = "Halloween", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Halloween.png?alt=media", FrameStyleId = 4, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 10, Name = "Summer Sea", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Summer.png?alt=media", FrameStyleId = 7, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 11, Name = "Night Party", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Night%20Party.png?alt=media", FrameStyleId = 2, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 12, Name = "Vintage Camera", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Vintage%20Camera.png?alt=media", FrameStyleId = 3, SlotCount = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 13, Name = "Happy New Year", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/New%20Year.png?alt=media", FrameStyleId = 4, SlotCount = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 14, Name = "Happy Mid-Autumn", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Mid%20Autumn%20Festival.png?alt=media", FrameStyleId = 4, SlotCount = 1, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 15, Name = "VietNam Travel", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Vietnam%20Travel.png?alt=media", FrameStyleId = 7, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 16, Name = "Grunge Retro", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/Grunge%20Retro.png?alt=media", FrameStyleId = 3, SlotCount = 4, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null },
                    new Frame { Id = 17, Name = "Happy 30/4", FrameUrl = "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/304%20%282%29.png?alt=media", FrameStyleId = 4, SlotCount = 1, ForMobile = true, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false, CreatedById = null, LastModifiedById = null }
                );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Frame] OFF");

            #endregion

            #region Seed data for Coordinate
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Coordinate] ON");

            if (!context.Coordinate.Any())
            {
                context.Coordinate.AddRange(
                    new Coordinate { Id = 1, FrameId = 1, X = 290, Y = 104, Width = 1414, Height = 943, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 2, FrameId = 2, X = 290, Y = 104, Width = 1414, Height = 943, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 3, FrameId = 4, X = 290, Y = 104, Width = 1414, Height = 943, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 4, FrameId = 5, X = 290, Y = 104, Width = 1414, Height = 943, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 5, FrameId = 3, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 6, FrameId = 3, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 7, FrameId = 3, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 8, FrameId = 3, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 9, FrameId = 6, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 10, FrameId = 6, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 11, FrameId = 6, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 12, FrameId = 6, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 13, FrameId = 7, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 14, FrameId = 7, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 15, FrameId = 7, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 16, FrameId = 7, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 17, FrameId = 8, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 18, FrameId = 8, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 19, FrameId = 8, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 20, FrameId = 8, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 21, FrameId = 9, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 22, FrameId = 9, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 23, FrameId = 9, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 24, FrameId = 9, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 25, FrameId = 10, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 26, FrameId = 10, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 27, FrameId = 10, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 28, FrameId = 10, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 29, FrameId = 11, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 30, FrameId = 11, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 31, FrameId = 11, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 32, FrameId = 11, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 33, FrameId = 12, X = 290, Y = 104, Width = 1414, Height = 943, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 34, FrameId = 13, X = 290, Y = 104, Width = 1414, Height = 943, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 35, FrameId = 14, X = 290, Y = 104, Width = 1414, Height = 943, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 36, FrameId = 15, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 37, FrameId = 15, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 38, FrameId = 15, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 39, FrameId = 15, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 40, FrameId = 16, X = 187, Y = 243, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 41, FrameId = 16, X = 1058, Y = 240, Width = 732, Height = 488, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 42, FrameId = 16, X = 187, Y = 833, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false },
                    new Coordinate { Id = 43, FrameId = 16, X = 1056, Y = 831, Width = 734, Height = 489, CreatedAt = DateTime.Parse("2025-03-28 13:15:24"), LastModified = DateTime.Parse("2025-03-28 13:15:24"), IsDeleted = false }
                );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Coordinate] OFF");

            #endregion

            #region Seed data for TypeSession
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [TypeSession] ON");

            if (!context.TypeSession.Any())
            {
                context.TypeSession.AddRange(
                    new TypeSession { Id = 1, Name = "Photography", Description = "Photo session", Duration = 60, Price = 10000, AbleTakenNumber = 5, ForMobile = true, CreatedAt = DateTime.Parse("2025-03-14T05:45:36.4862448"), LastModified = DateTime.Parse("2025-03-14T05:45:36.4862448"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 2, Name = "Videography", Description = "Video session", Duration = 120, Price = 20000, AbleTakenNumber = 3, ForMobile = true, CreatedAt = DateTime.Parse("2025-03-14T05:45:36.4862448"), LastModified = DateTime.Parse("2025-03-14T05:45:36.4862448"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 3, Name = "Basic Package", Description = "Basic printing service package", Duration = 30, Price = 50000, AbleTakenNumber = 10, ForMobile = true, CreatedAt = DateTime.Parse("2025-03-28T04:24:45.6577221"), LastModified = DateTime.Parse("2025-03-28T04:24:45.6577221"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 4, Name = "Printing Consultation", Description = "Consultation for suitable printing type", Duration = 20, Price = 10000, AbleTakenNumber = 5, ForMobile = false, CreatedAt = DateTime.Parse("2025-03-30T08:00:00"), LastModified = DateTime.Parse("2025-03-30T08:00:00"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 5, Name = "File Editing", Description = "Edit files before printing", Duration = 30, Price = 20000, AbleTakenNumber = 4, ForMobile = false, CreatedAt = DateTime.Parse("2025-03-30T08:01:00"), LastModified = DateTime.Parse("2025-03-30T08:01:00"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 6, Name = "Poster Design", Description = "Design posters on demand", Duration = 45, Price = 40000, AbleTakenNumber = 3, ForMobile = false, CreatedAt = DateTime.Parse("2025-03-30T08:02:00"), LastModified = DateTime.Parse("2025-03-30T08:02:00"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 7, Name = "Namecard Design", Description = "Create namecard designs", Duration = 25, Price = 15000, AbleTakenNumber = 6, ForMobile = false, CreatedAt = DateTime.Parse("2025-03-30T08:03:00"), LastModified = DateTime.Parse("2025-03-30T08:03:00"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 8, Name = "Flyer Design", Description = "Design brochures and promotional flyers", Duration = 35, Price = 35000, AbleTakenNumber = 5, ForMobile = false, CreatedAt = DateTime.Parse("2025-03-30T08:04:00"), LastModified = DateTime.Parse("2025-03-30T08:04:00"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 9, Name = "Layout Consultation", Description = "Consult on content layout for print", Duration = 20, Price = 10000, AbleTakenNumber = 7, ForMobile = false, CreatedAt = DateTime.Parse("2025-03-30T08:05:00"), LastModified = DateTime.Parse("2025-03-30T08:05:00"), CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new TypeSession { Id = 10, Name = "Product Photography", Description = "Professional product photography", Duration = 60, Price = 25000, AbleTakenNumber = 2, ForMobile = false, CreatedAt = DateTime.Parse("2025-03-30T08:06:00"), LastModified = DateTime.Parse("2025-03-30T08:06:00"), CreatedById = admin.Id, LastModifiedById = admin.Id }
                    );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [TypeSession] OFF");

            #endregion

            #region Seed data for Coupon
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Coupon] ON");

            if (!context.Coupon.Any())
            {
                context.Coupon.AddRange(
                    new Coupon { Id = 1, Name = "New Year Deal", Code = "NY2025", Description = "Chào đón năm mới 2025, chúng tôi mang đến cho bạn một chương trình khuyến mãi đặc biệt để cùng bạn đón Tết Nguyên Đán. Giảm giá ngay 5000 đồng cho mỗi đơn hàng và cơ hội nhận các ưu đãi tuyệt vời khác. Hãy tận dụng ngay!", Discount = 5000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), MaxUse = 100, MaxDiscount = 100000, MinOrder = 10000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 2, Name = "Spring Sale", Code = "SPRING25", Description = "Chào mừng mùa xuân đến với những ưu đãi hấp dẫn! Giảm giá 3000 đồng cho các đơn hàng mùa xuân. Khám phá các sản phẩm mới và tận hưởng không khí tươi mới của mùa xuân với mức giá cực kỳ ưu đãi.", Discount = 3000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(2), MaxUse = 50, MaxDiscount = 50000, MinOrder = 15000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 3, Name = "Summer Heat", Code = "SUMMERHEAT", Description = "Cùng làm dịu cơn nóng mùa hè với chương trình khuyến mãi 'Giảm Giá Mùa Hè'. Nhận ngay ưu đãi giảm giá 4000 đồng cho các đơn hàng mùa hè. Chương trình kéo dài trong 2 tháng, đừng bỏ lỡ!", Discount = 4000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(2), MaxUse = 70, MaxDiscount = 60000, MinOrder = 10000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 4, Name = "Back to School", Code = "SCHOOL25", Description = "Mùa tựu trường đã đến và chúng tôi có một chương trình ưu đãi đặc biệt dành cho các bạn học sinh và sinh viên. Giảm giá 2500 đồng cho mỗi đơn hàng, cùng những món quà thú vị chuẩn bị cho năm học mới.", Discount = 2500, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), MaxUse = 30, MaxDiscount = 40000, MinOrder = 12000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 5, Name = "Autumn Bonus", Code = "AUTUMN22", Description = "Mùa thu mang đến không khí tươi mới và những chương trình khuyến mãi hấp dẫn. Với 'Thưởng Mùa Thu', bạn sẽ nhận được giảm giá 2200 đồng cho mỗi đơn hàng. Đặc biệt, chương trình này chỉ kéo dài trong 1 tháng, đừng bỏ qua!", Discount = 2200, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), MaxUse = 60, MaxDiscount = 45000, MinOrder = 13000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 6, Name = "Black Friday", Code = "BLKFRI50", Description = "Ngày hội mua sắm lớn nhất trong năm - Black Friday đã đến! Giảm giá lên đến 50% cho các sản phẩm hot nhất. Đừng bỏ lỡ cơ hội mua sắm tiết kiệm vào dịp cuối năm. Chương trình áp dụng đến hết 10 ngày từ khi khởi động.", DiscountPercent = 0.50m, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(10), MaxUse = 500, MaxDiscount = 200000, MinOrder = 10000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 7, Name = "Cyber Monday", Code = "CYBER2025", Description = "Cyber Monday cũng đến với những ưu đãi hấp dẫn. Giảm giá 35% cho tất cả các sản phẩm trong chương trình này. Chương trình áp dụng đến hết 7 ngày từ khi bắt đầu. Hãy nhanh tay kẻo lỡ!", DiscountPercent = 0.35m, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), MaxUse = 300, MaxDiscount = 150000, MinOrder = 20000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 8, Name = "Xmas Cheer", Code = "XMAS2025", Description = "Mừng Giáng Sinh với chương trình khuyến mãi hấp dẫn. Giảm giá lớn cho các đơn hàng dịp lễ Giáng Sinh. Chỉ áp dụng trong vòng 1 tháng và có giới hạn số lượng sử dụng. Đừng bỏ lỡ cơ hội này để có những món quà tuyệt vời!", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), MaxUse = 250, MaxDiscount = 180000, MinOrder = 200000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 9, Name = "Birthday Gift", Code = "BIRTHDAY", Description = "Chúc mừng sinh nhật với một món quà đặc biệt từ chúng tôi! Giảm giá 20% cho các đơn hàng sinh nhật. Chương trình này áp dụng trong 5 ngày và chỉ có số lượng hạn chế, nên hãy nhanh tay tận dụng!", DiscountPercent = 0.20m, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5), MaxUse = 100, MaxDiscount = 80000, MinOrder = 5000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    new Coupon { Id = 10, Name = "Flash Sale", Code = "FLASH2025", Description = "Flash Sale siêu khuyến mãi trong thời gian ngắn! Giảm giá ngay 15% cho các sản phẩm nổi bật. Chương trình chỉ kéo dài 2 ngày và số lượng có hạn, hãy nhanh tay mua sắm.", DiscountPercent = 0.15m, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(2), MaxUse = 80, MaxDiscount = 60000, MinOrder = 4000, CreatedById = admin.Id, LastModifiedById = admin.Id }

                    //new Coupon { Id = 1, Name = "Khuyến Mãi Tết Nguyên Đán", Code = "NY2025", Description = "Chào đón năm mới 2025, chúng tôi mang đến cho bạn một chương trình khuyến mãi đặc biệt để cùng bạn đón Tết Nguyên Đán. Giảm giá ngay 5000 đồng cho mỗi đơn hàng và cơ hội nhận các ưu đãi tuyệt vời khác. Hãy tận dụng ngay!", Discount = 5000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), MaxUse = 100, MaxDiscount = 100000, MinOrder = 10000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 2, Name = "Giảm Giá Mùa Xuân", Code = "SPRING25", Description = "Chào mừng mùa xuân đến với những ưu đãi hấp dẫn! Giảm giá 3000 đồng cho các đơn hàng mùa xuân. Khám phá các sản phẩm mới và tận hưởng không khí tươi mới của mùa xuân với mức giá cực kỳ ưu đãi.", Discount = 3000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(2), MaxUse = 50, MaxDiscount = 50000, MinOrder = 15000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 3, Name = "Giảm Giá Mùa Hè", Code = "SUMMERHEAT", Description = "Cùng làm dịu cơn nóng mùa hè với chương trình khuyến mãi 'Giảm Giá Mùa Hè'. Nhận ngay ưu đãi giảm giá 4000 đồng cho các đơn hàng mùa hè. Chương trình kéo dài trong 2 tháng, đừng bỏ lỡ!", Discount = 4000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(2), MaxUse = 70, MaxDiscount = 60000, MinOrder = 10000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 4, Name = "Khuyến Mãi Trở Lại Trường", Code = "SCHOOL25", Description = "Mùa tựu trường đã đến và chúng tôi có một chương trình ưu đãi đặc biệt dành cho các bạn học sinh và sinh viên. Giảm giá 2500 đồng cho mỗi đơn hàng, cùng những món quà thú vị chuẩn bị cho năm học mới.", Discount = 2500, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), MaxUse = 30, MaxDiscount = 40000, MinOrder = 12000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 5, Name = "Thưởng Mùa Thu", Code = "AUTUMN22", Description = "Mùa thu mang đến không khí tươi mới và những chương trình khuyến mãi hấp dẫn. Với 'Thưởng Mùa Thu', bạn sẽ nhận được giảm giá 2200 đồng cho mỗi đơn hàng. Đặc biệt, chương trình này chỉ kéo dài trong 1 tháng, đừng bỏ qua!", Discount = 2200, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), MaxUse = 60, MaxDiscount = 45000, MinOrder = 13000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 6, Name = "Black Friday", Code = "BLKFRI50", Description = "Ngày hội mua sắm lớn nhất trong năm - Black Friday đã đến! Giảm giá lên đến 50% cho các sản phẩm hot nhất. Đừng bỏ lỡ cơ hội mua sắm tiết kiệm vào dịp cuối năm. Chương trình áp dụng đến hết 10 ngày từ khi khởi động.", DiscountPercent = 0.50m, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(10), MaxUse = 500, MaxDiscount = 200000, MinOrder = 10000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 7, Name = "Cyber Monday", Code = "CYBER2025", Description = "Cyber Monday cũng đến với những ưu đãi hấp dẫn. Giảm giá 35% cho tất cả các sản phẩm trong chương trình này. Chương trình áp dụng đến hết 7 ngày từ khi bắt đầu. Hãy nhanh tay kẻo lỡ!", DiscountPercent = 0.35m, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7), MaxUse = 300, MaxDiscount = 150000, MinOrder = 20000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 8, Name = "Giảm Giá Giáng Sinh", Code = "XMAS2025", Description = "Mừng Giáng Sinh với chương trình khuyến mãi hấp dẫn. Giảm giá lớn cho các đơn hàng dịp lễ Giáng Sinh. Chỉ áp dụng trong vòng 1 tháng và có giới hạn số lượng sử dụng. Đừng bỏ lỡ cơ hội này để có những món quà tuyệt vời!", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1), MaxUse = 250, MaxDiscount = 180000, MinOrder = 200000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 9, Name = "Quà Sinh Nhật", Code = "BIRTHDAY", Description = "Chúc mừng sinh nhật với một món quà đặc biệt từ chúng tôi! Giảm giá 20% cho các đơn hàng sinh nhật. Chương trình này áp dụng trong 5 ngày và chỉ có số lượng hạn chế, nên hãy nhanh tay tận dụng!", DiscountPercent = 0.20m, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5), MaxUse = 100, MaxDiscount = 80000, MinOrder = 5000, CreatedById = admin.Id, LastModifiedById = admin.Id },
                    //new Coupon { Id = 10, Name = "Giảm Giá Flash Sale", Code = "FLASH2025", Description = "Flash Sale siêu khuyến mãi trong thời gian ngắn! Giảm giá ngay 15% cho các sản phẩm nổi bật. Chương trình chỉ kéo dài 2 ngày và số lượng có hạn, hãy nhanh tay mua sắm.", DiscountPercent = 0.15m, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(2), MaxUse = 80, MaxDiscount = 60000, MinOrder = 4000, CreatedById = admin.Id, LastModifiedById = admin.Id }
                );

                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Coupon] OFF");

            #endregion

            #region Seed data for TypeSessionProduct
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [TypeSessionProduct] ON");

            if (!context.TypeSessionProduct.Any())
            {
                var typeSessionProducts = new List<TypeSessionProduct>();
                int idCounter = 1;

                for (int typeSessionId = 1; typeSessionId <= 3; typeSessionId++)
                {
                    var typeSession = context.TypeSession.Find(typeSessionId);

                    for (int levelMembershipId = 1; levelMembershipId <= 5; levelMembershipId++)
                    {
                        var levelMembership = context.LevelMembership.Find(levelMembershipId);
                        typeSessionProducts.Add(new TypeSessionProduct
                        {
                            Id = idCounter++,
                            Name = $"{typeSession.Name}({levelMembership.Name})",
                            TypeSessionId = typeSessionId,
                            LevelMembershipId = levelMembershipId,
                            ProductId = $"product_session_{ProductIdHelper.Normalize(typeSession.Name)}_{ProductIdHelper.Normalize(levelMembership.Name)}",
                            CreatedById = admin.Id,
                            LastModifiedById = admin.Id
                        });
                    }

                    for (int couponId = 1; couponId <= 10; couponId++)
                    {
                        var coupon = context.Coupon.Find(couponId);
                        typeSessionProducts.Add(new TypeSessionProduct
                        {
                            Id = idCounter++,
                            Name = $"{typeSession.Name}({coupon.Name})",
                            TypeSessionId = typeSessionId,
                            CouponId = couponId,
                            ProductId = $"product_session_{ProductIdHelper.Normalize(typeSession.Name)}_{ProductIdHelper.Normalize(coupon.Name)}",
                            CreatedById = admin.Id,
                            LastModifiedById = admin.Id
                        });

                    }
                }

                context.TypeSessionProduct.AddRange(typeSessionProducts);
                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [TypeSessionProduct] OFF");
            #endregion

            #region Seed data for DepositProduct
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [DepositProduct] ON");
            if (!context.DepositProduct.Any())
            {
                context.DepositProduct.AddRange(
                new DepositProduct { Id = 1, Name = "Nạp 20 nghìn", Description = "Giá 20 nghìn kèm khuyến mãi 10%", AmountAdd = 2000, ProductId = $"product_{ProductIdHelper.Normalize("Nạp 20 nghìn")}", Price = 20000 },
                new DepositProduct { Id = 2, Name = "Nạp 15 nghìn", Description = "Giá 15 nghìn kèm khuyến mãi 10%", AmountAdd = 1500, ProductId = $"product_{ProductIdHelper.Normalize("Nạp 15 nghìn")}", Price = 15000 },
                new DepositProduct { Id = 3, Name = "Nạp 12 nghìn", Description = "Giá 12 nghìn kèm khuyến mãi 10%", AmountAdd = 1200, ProductId = $"product_{ProductIdHelper.Normalize("Nạp 12 nghìn")}", Price = 12000 },
                new DepositProduct { Id = 4, Name = "Nạp 10 nghìn", Description = "Giá 10 nghìn", AmountAdd = 0, ProductId = $"product_{ProductIdHelper.Normalize("Nạp 10 nghìn")}", Price = 10000 },
                new DepositProduct { Id = 5, Name = "Nạp 17 nghìn", Description = "Giá 17 nghìn kèm khuyến mãi 10%", AmountAdd = 1700, ProductId = $"product_{ProductIdHelper.Normalize("Nạp 17 nghìn")}", Price = 17000 }
            );
                context.SaveChanges();
            }
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [DepositProduct] OFF");
            #endregion

            #region Seed data for Order
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Order] ON");

            var createdByOptions = new[] { staff.Id, staff2.Id, staff3.Id, manager.Id, manager2.Id, manager3.Id };
            var orders = new List<Order>();

            for (int i = 1; i <= 200; i++)
            {
                var createdById = createdByOptions[random.Next(createdByOptions.Length)];
                var daysBack = (int)(Math.Pow(random.NextDouble(), 2) * 720);
                var createdAt = now.AddDays(-daysBack); 

                orders.Add(new Order
                {
                    Id = i,
                    Code = random.NextInt64(1_000_000_000, 9_999_999_999),
                    CustomerId = (i % 2 == 0) ? customer.Id : customer2.Id,  
                    Status = random.Next(10) < 7 ? OrderStatus.Completed : OrderStatus.Failed,
                    Amount = random.Next(10_000, 100_001),
                    Email = (i % 2 == 0) ? customer.Email : customer2.Email,
                    Phone = "0" + random.Next(100_000_000, 1_000_000_000).ToString(),
                    CouponId = random.Next(1, 11), 
                    TypeSessionId = random.Next(1, 11), 
                    CreatedById = createdById,
                    LastModifiedById = (i % 2 == 0) ? customer.Id : customer2.Id,
                    CreatedAt = createdAt
                });
            }
            context.Order.AddRange(orders);
            context.SaveChanges();

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Order] OFF");
            #endregion

            #region Seed data for Session
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Session] ON");

            var ordersCompleteId = context.Order
                .Where(o => o.Status == OrderStatus.Completed)
                 .Select(o => new { o.Id, o.CreatedAt })
                .ToList();

            if (!context.Session.Any())
            {
                var sessions = new List<Session>();

                for (int i = 0; i < ordersCompleteId.Count; i++)
                {
                    var orderId = ordersCompleteId[i];

                    if (i == 0)
                    {
                        // Giữ nguyên session đầu tiên
                        sessions.Add(new Session
                        {
                            Id = 1,
                            Code = "SESSION",
                            Expired = DateTime.Now.AddDays(60),
                            AbleTakenNumber = 100,
                            IsActive = false,
                            OrderId = orderId.Id
                        });
                    }
                    else
                    {
                        sessions.Add(new Session
                        {
                            Id = i + 1, // Id bắt đầu từ 1
                            //Code = $"SESSION{i + 1}",
                            Expired = orderId.CreatedAt.Value.AddDays(random.Next(1, 6)),
                            AbleTakenNumber = random.Next(1, 11), // random 1..10
                            IsActive = random.Next(2) == 0, // random true/false
                            OrderId = orderId.Id
                        });
                    }
                }

                context.Session.AddRange(sessions);
                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Session] OFF");
            #endregion

            #region Seed data for PhotoHistory
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [PhotoHistory] ON");

            var boothIds = context.Booth
                       .Select(b => b.Id)
                       .ToList();
            var sessionIds = context.Session
                       .Select(s => s.Id)
                       .ToList();

            if (!context.PhotoHistory.Any())
            {
                var histories = Enumerable.Range(1, sessionIds.Count / 2)
                    .Select(i => new PhotoHistory
                    {
                        Id = i,
                        CustomerId = (i % 2 == 1) ? customer.Id : customer2.Id,
                        BoothId = boothIds[(i - 1) % boothIds.Count],
                        SessionId = sessionIds[(i - 1) % sessionIds.Count]
                    })
                    .ToList();

                context.PhotoHistory.AddRange(histories);
                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [PhotoHistory] OFF");
            #endregion

            #region Seed data for Photo
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Photo] ON");

            var photoHistoryIds = context.PhotoHistory
                .Select(ph => ph.Id)
                .ToList();

            var photoUrls = new List<string>
                {
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2FlmtEDH8kbb.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2F3EQLsfozA8.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2FRF583N9tuh.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2Fj5gbl9Iq2l.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2F9hcRtilHjX.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2Fra2403PcfG.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2F75suz3e6eU.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2FOl6sWrWzVi.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2FKvtV11AeCU.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2FXVQ7YgFsi9.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2FRFqyNFBk8D.png?alt=media",
                    "https://firebasestorage.googleapis.com/v0/b/social-networking-8c187.appspot.com/o/processed_images%2FlRJjkNJynq.png?alt=media"
                };


            if (!context.Photo.Any())
            {
                int id = 1;
                var photos = new List<Photo>();

                foreach (var photoHistoryId in photoHistoryIds)
                {
                    for (int i = 0; i < photoUrls.Count; i++)
                    {
                        photos.Add(new Photo
                        {
                            Id = id++,
                            PhotoHistoryId = photoHistoryId,
                            Url = photoUrls[i],
                            PhotoStyleId = (i % photoUrls.Count) + 1,
                            CreatedAt = DateTime.UtcNow,
                            LastModified = DateTime.UtcNow
                        });
                    }
                }

                context.Photo.AddRange(photos);
                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Photo] OFF");
            #endregion

            #region Seed data for Deposit
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Deposit] ON");

            var customerWalletIds = context.Wallet
            .Select(w => w.Id)
            .ToList();

            var deposits = new List<Deposit>();

            for (int i = 1; i <= 100; i++)
            {
                var walletId = customerWalletIds[random.Next(customerWalletIds.Count)];
                var status = random.Next(10) < 7 ? DepositStatus.Success : DepositStatus.Failed; // 70% success
                var amount = random.Next(10_000, 100_001);
                var createdAt = now.AddDays(-random.Next(0, 720));
                var depositProductId = random.Next(1, 5);

                deposits.Add(new Deposit
                {
                    Id = i,
                    WalletId = walletId,
                    Amount = amount,
                    Status = status,
                    DepositProductId = depositProductId,
                    CreatedAt = createdAt
                });
            }

            context.Deposit.AddRange(deposits);
            context.SaveChanges();

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Deposit] OFF");

            #endregion

            #region Seed data for Payment
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Payment] ON");

            var payments = new List<Payment>();
            int paymentId = 1;

            var ordersForDeposit = context.Order.ToList();
            foreach (var order in ordersForDeposit)
            {
                payments.Add(new Payment
                {
                    Id = paymentId++,
                    OrderId = order.Id,
                    DepositId = null,
                    PaymentMethodId = random.Next(1, 5), 
                    Status = order.Status == OrderStatus.Completed ? PaymentStatus.Success : PaymentStatus.Failed,
                    Amount = order.Amount,
                    CreatedAt = order.CreatedAt ?? DateTime.UtcNow
                });
            }
            // Tạo payment từ Deposit
            var depositsForPayment = context.Deposit.ToList();
            foreach (var deposit in depositsForPayment)
            {
                payments.Add(new Payment
                {
                    Id = paymentId++,
                    OrderId = null,
                    DepositId = deposit.Id,
                    PaymentMethodId = 4,
                    Status = deposit.Status == DepositStatus.Success ? PaymentStatus.Success : PaymentStatus.Failed,
                    Amount = deposit.Amount,
                    CreatedAt = deposit.CreatedAt
                });
            }

            context.Payment.AddRange(payments);
            context.SaveChanges();

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Payment] OFF");

            #endregion

            #region Seed data for Transaction
            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Transaction] ON");

            int transactionId = 1;
            var transactions = new List<Transaction>();
            var validPayments = context.Payment
            .Where(p => p.PaymentMethodId != 1 && p.Status == PaymentStatus.Success)
            .ToList();

            foreach (var payment in validPayments)
            {
                transactions.Add(new Transaction
                {
                    Id = transactionId++,
                    Type = payment.OrderId.HasValue ? TransactionType.Purchase : TransactionType.Deposit,
                    Amount = payment.Amount,
                    PaymentId = payment.Id,
                    CreatedAt = payment.CreatedAt
                });
            }
            context.Transaction.AddRange(transactions);
            context.SaveChanges();

            context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [Transaction] OFF");

            #endregion

            await transaction.CommitAsync();
            
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Error seeding data: " + ex.Message);
        }
    }
    public static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString()
            .Normalize(NormalizationForm.FormC)
            .Trim()
            .ToLower()
            .Replace(" ", "_");
    }
}
