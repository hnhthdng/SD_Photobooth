namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBoothRepository Booth { get; }
        ICouponRepository Coupon { get; }
        ILocationRepository Location { get; }
        IOrderRepository Order { get; }
        IPaymentMethodRepository PaymentMethod { get; }
        IPaymentRepository Payment { get; }
        ITransactionRepository Transaction { get; }
        IDepositRepository Deposit { get; }
        IPhotoHistoryRepository PhotoHistory { get; }
        IPhotoRepository Photo { get; }
        ISessionRepository Session { get; }
        ITypeSessionRepository TypeSession { get; }
        IPhotoStyleRepository PhotoStyle { get; }
        IUserRepository User { get; }
        IStickerRepository Sticker { get; }
        IStickerStyleRepository StickerStyle { get; }
        IFrameStyleRepository FrameStyle { get; }
        IFrameRepository Frame { get; }
        ICoordinateRepository Coordinate { get; }
        ILevelMembershipRepository LevelMembership { get; }
        IMembershipCardRepository MembershipCard { get; }
        IWalletRepository Wallet { get; }
        ITypeSessionProductRepository TypeSessionProduct { get; }
        IDepositProductRepository DepositProduct { get; }
        void Save();
        Task SaveAsync();
    }
}
