﻿using DevnotMentor.Api.Entities;

namespace DevnotMentor.Api.Utilities.Email
{
    public static class EmailTemplate
    {
        public const string ApplicationSubject = "Devnot Mentor Programı | Mentorluk";
        public static string ApplicationAppliedBody(User mentor, User mentee) => $"Merhaba {mentor.Name} {mentor.SurName},\n{mentee.Name} {mentee.SurName} kişisinden Mentor'u olmanız için istek var.";
        public static string ApplicationApprovedBody(User mentor, User mentee) => $"Merhaba {mentee.Name} {mentee.SurName},\n{mentor.Name} {mentor.SurName} Mentee'si olma isteğini kabul etti.";
    }
}
