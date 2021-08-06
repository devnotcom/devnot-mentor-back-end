using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Utilities.Email
{
    public static class EmailTemplate
    {
        public static string ApplyToMentorSubject = "Devnot Mentor Programı | Mentorluk İsteği";
        public static string ApplyToMentorBody(User mentor,User mentee)=>$"Merhaba {mentor.Name} {mentor.SurName},\n{mentee.Name} {mentee.SurName} kişisinden Mentor'u olmanız için istek var.";
        public static string AcceptMenteeBody(User mentor, User mentee) => $"Merhaba {mentee.Name} {mentee.SurName},\n{mentor.Name} {mentor.SurName} Mentee'si olma isteğini kabul etti.";

    }
}
