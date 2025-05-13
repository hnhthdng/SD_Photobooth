using PhotoBooth_App.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoBooth_App
{
    public static class FrameHandler
    {
        public static BitmapSource ApplyFrameToImages(List<BitmapImage> selectedImages,BitmapImage frame,List<CoordinateItem> coordinates)
        {
            if (frame == null || selectedImages == null || coordinates == null)
                return null;

            int frameWidth = frame.PixelWidth;
            int frameHeight = frame.PixelHeight;

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                for (int i = 0; i < selectedImages.Count && i < coordinates.Count; i++)
                {
                    var img = selectedImages[i];
                    var coord = coordinates[i];
                    var slotRect = new Rect(coord.X, coord.Y, coord.Width, coord.Height);

                    double scaleX = slotRect.Width / img.PixelWidth;
                    double scaleY = slotRect.Height / img.PixelHeight;
                    double scale = Math.Min(scaleX, scaleY);

                    double newW = img.PixelWidth * scale;
                    double newH = img.PixelHeight * scale;
                    double offsetX = coord.X + (slotRect.Width - newW) / 2;
                    double offsetY = coord.Y + (slotRect.Height - newH) / 2;

                    dc.DrawImage(img, new Rect(offsetX, offsetY, newW, newH));
                }

                dc.DrawImage(frame, new Rect(0, 0, frameWidth, frameHeight));
            }

            var merged = new RenderTargetBitmap(frameWidth, frameHeight, 96, 96, PixelFormats.Pbgra32);
            merged.Render(dv);
            return merged;
        }
    }
}