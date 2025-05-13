using BussinessObject.Models;
using DataAccess.Data;
using DataAccess.Repository.IRepository;

namespace DataAccess.Repository
{
    public class PhotoStyleRepository : Repository<PhotoStyle>, IPhotoStyleRepository
    {
        private readonly AIPhotoboothDbContext _db;
        public PhotoStyleRepository(AIPhotoboothDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PhotoStyle photoStyle)
        {
            var objFromDb = _db.PhotoStyle.FirstOrDefault(s => s.Id == photoStyle.Id);
            objFromDb.Name = photoStyle.Name;
            objFromDb.Description = photoStyle.Description;
            objFromDb.ImageUrl = photoStyle.ImageUrl;
            objFromDb.Prompt = photoStyle.Prompt;
            objFromDb.NegativePrompt = photoStyle.NegativePrompt;
            objFromDb.Controlnets = photoStyle.Controlnets;
            objFromDb.NumImagesPerGen = photoStyle.NumImagesPerGen;
            objFromDb.BackgroundColor = photoStyle.BackgroundColor;
            objFromDb.Height = photoStyle.Height;
            objFromDb.Width = photoStyle.Width;
            objFromDb.IPAdapterScale = photoStyle.IPAdapterScale;
            objFromDb.Mode = photoStyle.Mode;
            objFromDb.NumInferenceSteps = photoStyle.NumInferenceSteps;
            objFromDb.GuidanceScale = photoStyle.GuidanceScale;
            objFromDb.Strength = photoStyle.Strength;
            objFromDb.BackgroundRemover = photoStyle.BackgroundRemover;
        }

        public Task UpdateAsync(PhotoStyle photoStyle)
        {
            var objFromDb = _db.PhotoStyle.FirstOrDefault(s => s.Id == photoStyle.Id);
            objFromDb.Name = photoStyle.Name;
            objFromDb.Description = photoStyle.Description;
            objFromDb.ImageUrl = photoStyle.ImageUrl;
            objFromDb.Prompt = photoStyle.Prompt;
            objFromDb.NegativePrompt = photoStyle.NegativePrompt;
            objFromDb.Controlnets = photoStyle.Controlnets;
            objFromDb.NumImagesPerGen = photoStyle.NumImagesPerGen;
            objFromDb.BackgroundColor = photoStyle.BackgroundColor;
            objFromDb.Height = photoStyle.Height;
            objFromDb.Width = photoStyle.Width;
            objFromDb.IPAdapterScale = photoStyle.IPAdapterScale;
            objFromDb.Mode = photoStyle.Mode;
            objFromDb.NumInferenceSteps = photoStyle.NumInferenceSteps;
            objFromDb.GuidanceScale = photoStyle.GuidanceScale;
            objFromDb.Strength = photoStyle.Strength;
            objFromDb.BackgroundRemover = photoStyle.BackgroundRemover;
            return Task.CompletedTask;
        }
    }
}
