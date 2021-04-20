using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EVCharging.Utilities.Helpers
{
    public class FileHelper
    {
        private readonly ConfigHelper _configHelper;
        public FileHelper()
        {
            _configHelper = FoundationObject.FoundationObj.configHelper;
        }
        public bool CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return true;
        }
        public string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public string[] GetFiles(string path)
        {
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path);
            }
            return new string[] { };
        }

        public bool FileDeleteIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return true;
        }

        public bool FileExistCheck(string path)
        {
            return File.Exists(path);


        }



        public string GetGUIDGeneratedName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName) + "-" + Guid.NewGuid().ToString("N") + fileName.Substring(fileName.LastIndexOf(".",
                                 System.StringComparison.Ordinal));
        }


        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public string ReadAllTextFromFile(string path)
        {
            return File.ReadAllText(path);
        }


        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }



        public void SaveBase64Image(string categoryPath, string base64Image)
        {
            string imagecontent = string.Empty;
            if (base64Image.Contains("data:image/jpeg;base64"))
            {
                imagecontent = base64Image.Replace("data:image/jpeg;base64,", String.Empty);
            }
            else if (base64Image.Contains("data:image/png;base64"))
            {
                imagecontent = base64Image.Replace("data:image/png;base64,", String.Empty);
            }
            File.WriteAllBytes(categoryPath, Convert.FromBase64String(imagecontent));
        }

        public string GetFileType(string extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
        #region Private Methods
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats  officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        private IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
        {".png", "image/png"},
        {".jpeg", "image/jpeg"},
        {".jpg", "image/jpg"},
        {".mp4", "video/mp4"},
        {".pdf", "application/pdf"},
        {".doc", "application/msword"},
        {".txt", "text/plain"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"} };

        public byte[] GetBytesFromPath(string path)
        {
            return File.ReadAllBytes(path);
        }

        public string GetFileNameFromPath(string path)
        {
            return Path.GetFileName(path);
        }

        public string GetExtensionFromPath(string path)
        {
            return Path.GetExtension(path);
        }
        #endregion
    }
}
