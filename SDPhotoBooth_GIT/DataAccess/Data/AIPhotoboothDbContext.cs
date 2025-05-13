using BussinessObject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public class AIPhotoboothDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AIPhotoboothDbContext(DbContextOptions<AIPhotoboothDbContext> options) : base(options)
        {
        }

        public DbSet<Location> Location { get; set; }
        public DbSet<Booth> Booth { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<PhotoHistory> PhotoHistory { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<TypeSession> TypeSession { get; set; }
        public DbSet<PhotoStyle> PhotoStyle { get; set; }

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Deposit> Deposit { get; set; }
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<Payment> Payment { get; set; }

        public DbSet<MembershipCard> MembershipCard { get; set; }
        public DbSet<LevelMembership> LevelMembership { get; set; }

        public DbSet<Frame> Frame { get; set; }
        public DbSet<FrameStyle> FrameStyle { get; set; }
        public DbSet<Coordinate> Coordinate { get; set; }
        public DbSet<StickerStyle> StickerStyle { get; set; }
        public DbSet<Sticker> Sticker { get; set; }

        public DbSet<TypeSessionProduct> TypeSessionProduct { get; set; }
        public DbSet<DepositProduct> DepositProduct { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AIPhotoboothDbContext).Assembly);
        }
    }
}
