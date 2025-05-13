using EOSDigital.SDK;

namespace BussinessObject.Models
{
    public class CameraAttribute
    {

        public static readonly Dictionary<DriveLens, DriveLens> FocusNearNextMap = new()
        {
            { DriveLens.Near1, DriveLens.Near2 },
            { DriveLens.Near2, DriveLens.Near3 },
            { DriveLens.Near3, DriveLens.Near1 },
        };

        public static readonly Dictionary<DriveLens, string> FocusNearTextMap = new()
        {
            { DriveLens.Near1, "GẦN 1" },
            { DriveLens.Near2, "GẦN 2" },
            { DriveLens.Near3, "GẦN 3" },
        };

        public static readonly Dictionary<DriveLens, DriveLens> FocusFarNextMap = new()
        {
            { DriveLens.Far1, DriveLens.Far2 },
            { DriveLens.Far2, DriveLens.Far3 },
            { DriveLens.Far3, DriveLens.Far1 },
        };

        public static readonly Dictionary<DriveLens, string> FocusFarTextMap = new()
        {
            { DriveLens.Far1, "XA 1" },
            { DriveLens.Far2, "XA 2" },
            { DriveLens.Far3, "XA 3" },
        };
    }
}
