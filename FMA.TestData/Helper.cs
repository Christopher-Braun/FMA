using System;
using System.IO;
using System.Reflection;
using FMA.View.Helpers;

namespace FMA.TestData
{
    public static class Helper
    {
        public static byte[] GetBackground(int id, bool frontSide = true)
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                return new byte[0];
            }

            var location = entryAssembly.Location;
            var dir = Path.GetDirectoryName(location);

            var flyerFileName = string.Format(frontSide ? "Backgrounds\\Flyer_{0}.jpg" : "Backgrounds\\Flyer_{0}_hinten.jpg", id.ToString("00"));
            if (dir           == null)
            {
                throw new InvalidOperationException("Location");
            }
            var imagePath = Path.Combine(dir, flyerFileName);
            if (File.Exists(imagePath) == false)
            {
                return new byte[0];
            }

            return FileHelper.GetByteArrayFromFile(imagePath);
        }


    }
}