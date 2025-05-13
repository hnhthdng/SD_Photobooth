using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBooth_App.Handler
{
    static class GetPhotoHandler
    {
        public static string FindSolutionRoot()
        {
            string currentDir = AppContext.BaseDirectory;

            while (!string.IsNullOrEmpty(currentDir))
            {
                if (Directory.GetFiles(currentDir, "*.sln").Any())
                {
                    return currentDir;
                }

                currentDir = Directory.GetParent(currentDir)?.FullName;
            }

            throw new Exception("Không tìm thấy thư mục gốc");
        }
    }
}
