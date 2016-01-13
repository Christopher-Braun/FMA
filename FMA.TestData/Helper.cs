using System;
using System.IO;
using System.Reflection;

namespace FMA.TestData
{
    public static class Helper
    {
        public static byte[] GetFrontSide(int id)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                return new byte[0];
            }

            var location = entryAssembly.Location;
            var dir = Path.GetDirectoryName(location);
            var flyerFileName = String.Format("Flyer_{0}.jpg", id.ToString("00"));
            if (dir == null)
            {
                throw new InvalidOperationException("Location");
            }
            var imagePath = Path.Combine(dir, flyerFileName);
            if (File.Exists(imagePath) == false)
            {
                return new byte[0];
            }

            return GetByteArrayFromFile(imagePath);
        }

        public static byte[] GetByteArrayFromFile(string filePath)
        {
            byte[] data;

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    data = ms.ToArray();
                }
            }

            return data;
        }
    }
}