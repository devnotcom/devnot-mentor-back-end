namespace DevnotMentor.Api.Configuration.Context
{
    /// <summary>
    /// This interface provides value of configuration properties.
    /// </summary>
    public interface IDevnotConfigurationContext
    {

        /// <summary>
        /// SQL Server connection string.
        /// </summary>
        public string SQLServerConnectionString { get; }

        /// <summary>
        /// JWT secret key.
        /// </summary>
        public string JwtSecret { get; }
        /// <summary>
        /// Jwt expiry hours.
        /// </summary>
        public int JwtSecretExpirationInMinutes { get; }
        /// <summary>
        /// Jwt valid issuer.
        /// </summary>
        public string JwtValidIssuer { get; }
        /// <summary>
        /// Jwt valid audience.
        /// </summary>
        public string JwtValidAudience { get; }

        /// <summary>
        /// Profile image path. It should be relative.
        /// </summary>
        public string ProfileImagePath { get; }

        /// <summary>
        /// Profile image maximum file length in bytes.
        /// </summary>
        public int ProfileImageMaxFileLength { get; }

        /// <summary>
        /// Max mentor count of mentee.
        /// </summary>
        public int MaxMentorCountOfMentee { get; }
        /// <summary>
        /// Max mentee count of mentor.
        /// </summary>
        public int MaxMenteeCountOfMentor { get; }

        /// <summary>
        /// Web page url for update password.
        /// </summary>
        public string UpdatePasswordWebPageUrl { get; }

        /// <summary>
        /// Security key expiry, we use when update password.
        /// </summary>
        public int SecurityKeyExpiryFromHours { get; }


        /// <summary>
        /// Smtp host.
        /// </summary>
        public string SmtpHost { get; }
        /// <summary>
        /// Smtp email.
        /// </summary>
        public string SmtpEmail { get; }
        /// <summary>
        /// Smtp password.
        /// </summary>
        public string SmtpPassword { get; }
        /// <summary>
        /// Smtp port.
        /// </summary>
        public int SmtpPort { get; }
    }
}
