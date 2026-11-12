using System;
using System.IO;

namespace FileTypeGuard
{
    class Program
    {
        static string GetFileType(byte[] header)
        {
            if (header == null || header.Length < 2)
            {
                return "UNKNOWN";
            }

            if (header.Length >= 4 &&
                header[0] == 0x25 && header[1] == 0x50 && header[2] == 0x44 && header[3] == 0x46)
            {
                return "PDF";
            }

            if (header.Length >= 4 &&
                header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
            {
                return "PNG";
            }

            if (header.Length >= 3 &&
                header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
            {
                return "JPEG";
            }

            if (header.Length >= 4 &&
                header[0] == 0x50 && header[1] == 0x4B && header[2] == 0x03 && header[3] == 0x04)
            {
                return "ZIP";
            }

            if (header.Length >= 2 &&
                header[0] == 0x4D && header[1] == 0x5A)
            {
                return "EXE";
            }

            if (header.Length >= 4 &&
                header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x38)
            {
                return "GIF";
            }

            return "UNKNOWN";
        }

        static (string fileType, string extension, string status) CheckFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return ("UNKNOWN", string.Empty, "ERROR");
            }

            try
            {
                if (!File.Exists(filePath))
                {
                    return ("UNKNOWN", string.Empty, "ERROR");
                }
            }
            catch
            {
                return ("UNKNOWN", string.Empty, "ERROR");
            }

            byte[] header;
            try
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    header = new byte[8];
                    int read = stream.Read(header, 0, 8);
                    if (read < 8)
                    {
                        Array.Resize(ref header, read);
                    }
                }
            }
            catch
            {
                return ("UNKNOWN", string.Empty, "ERROR");
            }

            string fileType = GetFileType(header);

            string rawExt = Path.GetExtension(filePath);
            string extension = string.IsNullOrEmpty(rawExt) ? string.Empty : rawExt.TrimStart('.').ToUpperInvariant();

            if (fileType == "UNKNOWN")
            {
                return (fileType, extension, "UNKNOWN");
            }

            if (string.IsNullOrEmpty(extension))
            {
                return (fileType, extension, "UNKNOWN");
            }

            if (fileType == extension)
            {
                return (fileType, extension, "MATCH");
            }

            if (fileType == "JPEG" && extension == "JPG")
            {
                return (fileType, extension, "MATCH");
            }

            return (fileType, extension, "MISMATCH");
        }

        static string GetRiskLevel(string status)
        {
            if (status == "MISMATCH")
            {
                return "HIGH";
            }

            if (status == "ERROR")
            {
                return "MEDIUM";
            }

            if (status == "UNKNOWN")
            {
                return "MEDIUM";
            }

            return "LOW";
        }

        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet run -- <filePath>");
                return;
            }

            string filePath = args[0];

            var result = CheckFile(filePath);

            string risk = GetRiskLevel(result.status);

            Console.WriteLine($"File:      {filePath}");
            Console.WriteLine($"Real Type: {result.fileType}");
            Console.WriteLine($"Extension: {result.extension}");
            Console.WriteLine($"Status:    {result.status}");
            Console.WriteLine($"Risk:      {risk}");

            return;
        }
    }
}