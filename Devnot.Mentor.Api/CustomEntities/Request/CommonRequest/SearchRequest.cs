using System;
using System.Collections.Generic;
using System.Linq;

namespace DevnotMentor.Api.CustomEntities.Request.CommonRequest
{
    public class SearchRequest
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }

        public SearchRequest()
        {
            Tags = new();
        }

        /// <summary>
        /// Method checks properties are valid. It means there is any property which contains some value.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !CheckAllPropertyAreNullOrEmpty();
        }

        /// <summary>
        /// Method checks properties are not valid. It means there is not any property which contains some value.
        /// </summary>
        /// <returns></returns>
        public bool IsNotValid()
        {
            return CheckAllPropertyAreNullOrEmpty();
        }

        /// <summary>
        /// Check all property are null or empty.
        /// </summary>
        /// <returns></returns>
        private bool CheckAllPropertyAreNullOrEmpty()
        {
            return String.IsNullOrEmpty(FullName) &&
                   String.IsNullOrEmpty(Title) &&
                   String.IsNullOrEmpty(Description) &&
                   !Tags.Any();
        }
    }
}
