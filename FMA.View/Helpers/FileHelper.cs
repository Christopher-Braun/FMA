// Christopher Braun 2016

using System;
using System.IO;
using FMA.View.Properties;
using Microsoft.Win32;

namespace FMA.View.Helpers
{
    public static class FileHelper
    {
        public static string ShowFileOpenDialogForImages()
        {
            var dialog = new OpenFileDialog
            {
                Title = Resources.SelectLogoFileTitle,
                InitialDirectory = Settings.Default.LastLogoPath,
                Filter = $"{Resources.ImageFiles} | *.jpg; *.jpeg; *.bmp; *.png; *.gif | {Resources.AllFiles} | *.*"
            };

            var result = dialog.ShowDialog();
            if (result != true)
            {
                return string.Empty;
            }

            var fileName = dialog.FileName;

            Settings.Default.LastLogoPath = Path.GetDirectoryName(fileName);
            Settings.Default.Save();

            return fileName;
        }

        public static byte[] GetByteArrayFromFile(string logoFile)
        {
            var logoData = new byte[0];

            try
            {
                if (File.Exists(logoFile))
                {
                    using (var fileStream = new FileStream(logoFile, FileMode.Open))
                    {
                        using (var ms = new MemoryStream())
                        {
                            fileStream.CopyTo(ms);
                            logoData = ms.ToArray();
                        }
                    }
                }

            }
            catch (Exception e)
            {
               //TODO Log
            }

            return logoData;
        }
    }
}