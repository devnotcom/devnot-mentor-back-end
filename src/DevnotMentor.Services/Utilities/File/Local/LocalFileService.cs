using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevnotMentor.Common.API;
using DevnotMentor.Configurations.Context;
using Microsoft.AspNetCore.Http;

namespace DevnotMentor.Services.Utilities.File.Local
{
    public class LocalFileService : IFileService
    {
        private readonly string[] validImageMimeTypes;

        private readonly IDevnotConfigurationContext devnotConfigurationContext;

        public LocalFileService(IDevnotConfigurationContext devnotConfigurationContext)
        {
            validImageMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            this.devnotConfigurationContext = devnotConfigurationContext;
        }

        public async Task<FileResult> InsertProfileImageAsync(IFormFile profileImageFile)
        {
            if (profileImageFile == null || profileImageFile.Length == 0)
            {
                return new FileResult(isSuccess: false, errorMessage: ResultMessage.ProfileImageCanNotBeNullOrEmpty, newFileName: default, relativeFilePath: default);
            }

            //Check image max file length.
            if (profileImageFile.Length > this.devnotConfigurationContext.ProfileImageMaxFileLength)
            {
                return new FileResult(isSuccess: false, errorMessage: ResultMessage.InvalidProfileImageSize, newFileName: default, relativeFilePath: default);
            }

            // Check image has valid extension?
            if (validImageMimeTypes.Contains(profileImageFile.ContentType.ToLower()) == false)
            {
                return new FileResult(isSuccess: false, errorMessage: ResultMessage.InvalidProfileImage, newFileName: default, relativeFilePath: default);
            }

            // Generate unique name for file name.
            var fileName = Guid.NewGuid();

            var fileExtension = Path.GetExtension(profileImageFile.FileName);

            // File full name.
            var fileNameWithExtension = fileName + fileExtension;

            // Relative file path from main project files.
            var relativeFilePath = devnotConfigurationContext.ProfileImagePath + fileNameWithExtension;

            await using var stream = new FileStream(relativeFilePath, FileMode.Create);

            await profileImageFile.CopyToAsync(stream);

            return new FileResult(isSuccess: true, errorMessage: default, newFileName: fileNameWithExtension, relativeFilePath: relativeFilePath);
        }
    }
}
