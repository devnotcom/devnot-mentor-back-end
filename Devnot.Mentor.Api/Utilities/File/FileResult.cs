namespace DevnotMentor.Api.Utilities.File
{
    /// <summary>
    /// It provides file operation result.
    /// </summary>
    public class FileResult
    {
        public FileResult(bool isSuccess, string errorMessage, string newFileName, string relativeFilePath)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            NewFileName = newFileName;
            RelativeFilePath = relativeFilePath;
        }

        /// <summary>
        /// Check operation worked successfully.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// You can use error message when IsSuccess is false
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// New file name after the inserting.
        /// </summary>
        public string NewFileName { get; set; }

        /// <summary>
        /// New file name with relative path from main project files.
        /// </summary>
        public string RelativeFilePath { get; set; }
    }
}
