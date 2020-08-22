﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int SecretExpirationInMinutes { get; set; }
        public string ValidIssuer { get; set; }
        public string ProfileImagePath { get; set; }
        public int MaxMentorCountOfMentee { get; set; }
        public int MaxMenteeCountOfMentor { get; set; }
        public int SecurityKeyExpiryFromHours { get; set; }
        public string UpdatePasswordWebPageUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
    }
}
