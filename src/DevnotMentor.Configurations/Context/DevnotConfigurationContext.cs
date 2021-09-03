﻿using DevnotMentor.Configurations.Environment;

namespace DevnotMentor.Configurations.Context
{
    public class DevnotConfigurationContext : IDevnotConfigurationContext
    {
        public DevnotConfigurationContext(IEnvironmentService environmentService)
        {
            SQLServerConnectionString = environmentService.Configuration["ConnectionStrings:SQLServerConnectionString"];


            JwtSecret = environmentService.Configuration["JWT:Secret"];

            JwtSecretExpirationInMinutes = int.Parse(environmentService.Configuration["JWT:SecretExpirationInMinutes"]);

            JwtValidIssuer = environmentService.Configuration["JWT:ValidIssuer"];

            JwtValidAudience = environmentService.Configuration["JWT:ValidAudience"];



            SmtpHost = environmentService.Configuration["SMTP:Host"];

            SmtpEmail = environmentService.Configuration["SMTP:Email"];

            SmtpPassword = environmentService.Configuration["SMTP:Password"];

            SmtpPort = int.Parse(environmentService.Configuration["SMTP:Port"]);



            ProfileImagePath = environmentService.Configuration["General:ProfileImagePath"];
            ProfileImageMaxFileLength = int.Parse(environmentService.Configuration["General:ProfileImageMaxFileLength"]);

            MaxMentorCountOfMentee = int.Parse(environmentService.Configuration["General:MaxMentorCountOfMentee"]);

            MaxMenteeCountOfMentor = int.Parse(environmentService.Configuration["General:MaxMenteeCountOfMentor"]);

            UpdatePasswordWebPageUrl = environmentService.Configuration["General:UpdatePasswordWebPageUrl"];

            SecurityKeyExpiryFromHours = int.Parse(environmentService.Configuration["General:SecurityKeyExpiryFromHours"]);
        }


        public string SQLServerConnectionString { get; }
        public string JwtSecret { get; }
        public int JwtSecretExpirationInMinutes { get; }
        public string JwtValidIssuer { get; }
        public string JwtValidAudience { get; }
        public string ProfileImagePath { get; }
        public int ProfileImageMaxFileLength { get; }
        public int MaxMentorCountOfMentee { get; }
        public int MaxMenteeCountOfMentor { get; }
        public string UpdatePasswordWebPageUrl { get; }
        public int SecurityKeyExpiryFromHours { get; }
        public string SmtpHost { get; }
        public string SmtpEmail { get; }
        public string SmtpPassword { get; }
        public int SmtpPort { get; }

    }
}
