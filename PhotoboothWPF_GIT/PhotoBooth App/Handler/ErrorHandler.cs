using EOSDigital.SDK;
using System;
using System.Windows;

namespace PhotoBooth_App.Handler
{
    public static class ErrorHandler
    {
        public static event Action<Exception> SevereErrorHappened;
        public static event Action<ErrorCode> NonSevereErrorHappened;
        private static UIHandler uiHandler;

        public static void Initialize(UIHandler handler)
        {
            uiHandler = handler;
        }

        public static void ReportError(string message, bool lockdown)
        {
            if (lockdown) uiHandler?.EnableUI(false);
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void OnSevereError(Exception ex)
        {
            SevereErrorHappened?.Invoke(ex);
        }

        public static void OnNonSevereError(ErrorCode ex)
        {
            NonSevereErrorHappened?.Invoke(ex);
        }

        public static void ErrorHandler_NonSevereErrorHappened(object sender, ErrorCode ex)
        {
            ReportError($"SDK Error code: {ex} ({((int)ex).ToString("X")})", false);
        }

        public static void ErrorHandler_SevereErrorHappened(object sender, Exception ex)
        {
            ReportError(ex.Message, true);
        }
    }
}