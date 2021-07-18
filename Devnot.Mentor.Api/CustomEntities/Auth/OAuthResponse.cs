namespace DevnotMentor.Api.CustomEntities.OAuth
{
    public class OAuthResponse
    {
        #region github & google
        public string id { get; set; }          // google, github: id
        public string name { get; set; }        // google, github: fullname
        public string email { get; set; }       // google, github : email (it can be null, if it's coming from github)
        #endregion
        
        #region just google
        public string picture { get; set; }     // google: profile picture
        #endregion

        #region just github
        public string login { get; set; }       // github: username
        public string avatar_url { get; set; }  // github: profilepicture
        #endregion
    }
}