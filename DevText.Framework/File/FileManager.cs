using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DevText.Framework.File
{
    public static class FileManager
    {
        public static DirectoryInfo DataDirectory
        {
            get 
            {
                var appDataPath = (string)AppDomain.CurrentDomain.GetData("DataDirectory") ??
                           AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                return new DirectoryInfo(appDataPath);
            }
        }

        public static FileInfo GetUniqueFile(DirectoryInfo dir,FileInfo uploadedFile)
        {
            string fullname;
            string name = uploadedFile.Name;
            fullname = Path.Combine(dir.FullName, name);
            int counter = 1;

            while (System.IO.File.Exists(fullname) == true)
            {
                string basename = Path.GetFileNameWithoutExtension(name);
                string ext = Path.GetExtension(name);
                fullname = Path.Combine(dir.FullName, String.Format("{0}{1:d3}{2}",
                                                         Path.GetFileNameWithoutExtension(name),
                                                         counter,
                                                         Path.GetExtension(name)));
                counter++;
                if (counter >= 1000)
                {
                    throw new Exception("Could not generate unique filename during upload. Operation Aborted");
                }
            }
            return new FileInfo(fullname);
       
        }

        public static DirectoryInfo ImageDirectory
        {
            get
            {
                return FileManager.GetDirectory("Images");
            }
        }

        public static DirectoryInfo FileDirectory
        {
            get { return FileManager.GetDirectory("Files"); }
        }

        public static DirectoryInfo EmailDirectory
        {
            get { return FileManager.GetDirectory("Email"); }
        }

        public static string GetImageMarkup(FileInfo info)
        {
            return String.Format("<img src='/Images/{0}'>", info.Name);
        }

        public static string GetFileMarkup(FileInfo info)
        {
            return String.Format("<a href='/Files/{0}' target='_blank'>{0}{1}</a>", info.Name, info.Length.FormatBytes());
        }

        public static DirectoryInfo GetDirectory(string relativePath)
        {
          string path =Path.Combine(DataDirectory.FullName,relativePath);
          return new DirectoryInfo(path);
        }

        public static Boolean IsImage(string filename)
        {
            string imagePattern = @"^.*\.(jpg|JPG|gif|GIF|phg|PNG|jpeg|JPEG|tif|TIF)$";
            return Regex.IsMatch(filename, imagePattern);
        }

        public static Boolean isImage(FileInfo info)
        {
            return IsImage(info.FullName);
        }

        public static string GetContentType(string filename)
        {
            FileInfo file = new FileInfo(filename);
            switch(file.Extension.ToUpper())
            {
                case ".PNG": return "image/png";
                case ".JPG": return "image/jpeg";
                case "JPEG": return "image/jpeg";
                case "GIF": return "image/gif";
                case "BMP": return "image/bmp";
                case "TIFF": return "image/tiff";
                default:
                    throw new NotSupportedException("The Specified File Type is Not supported");
            }
        }

        public static string FormatBytes(this long bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);
            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);
                max /= scale;
            }
            return "0 Bytes";
        }

    }
}
