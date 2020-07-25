using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Helpers
{
    public class FileHelper
    {
        public static bool IsValidProfileImage(IFormFile file)
        {
            if (file.Length > 0 && file.ContentType.Contains("image"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<string> UploadProfileImage(IFormFile file, AppSettings appSettings)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = appSettings.ProfileImagePath + fileName;

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
