using PhotoBooth_App.Handler;
using PhotoBooth_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBooth_App
{
    public static class UserSession
    {
        public static string UserRole { get; set; } = "Guest";
        public static string SessionId { get; set; }
        public static DateTime Expired { get; set; }
        public static CameraHandler cameraHandler { get; set; }

        public static int BoothId { get; set; }
        public static long OrderId { get; set; }
        public static FrameItem SelectedFrame { get; set; }
        public static string SessionCode { get; set; }
        public static void Logout()
        {
            UserRole = null;
            SessionId = null;
            SessionCode = null;
            Expired = DateTime.MinValue;
        }
    }
}
