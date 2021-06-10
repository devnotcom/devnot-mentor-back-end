using System;

namespace DevnotMentor.Api.Helpers.Extensions
{
    public static class EnumExtensions
    {
        public static int? ToInt(this Enum status)
        {
            return Convert.ToInt32(status);
        }
    }
}
