namespace DevnotMentor.Api.Common
{
    /// <summary>
    /// It provides global result message keys.
    /// </summary>
    public class ResultMessage
    {
        public static string Success => "messages.success.general.ok";
        public static string InvalidModel => "messages.error.general.invalidModel";
        public static string UnhandledException => "messages.error.general.unhandledException";
        public static string UnAuthorized => "messages.error.general.unAuthorized";

        public static string InternalServerError => "messages.error.general.internalServerError";

        public static string TokenCanNotBeEmptyOrNull => "messages.error.token.tokenCanNotBeEmptyOrNull";
        public static string TokenMustStartWithBearerKeyword => "messages.error.token.tokenMustStartWithBearerKeyword";
        public static string InvalidToken => "messages.error.token.invalidToken";


        public static string NotFoundUser => "messages.error.user.notFoundUser";
        public static string InvalidSecurityKey => "messages.error.user.invalidSecurityKey";
        public static string InvalidProfileImage => "messages.error.user.invalidProfileImage";
        public static string InvalidProfileImageSize => "messages.error.user.invalidProfileImageSize";
        public static string ProfileImageCanNotBeNullOrEmpty => "messages.error.user.profileImageCanNotBeNullOrEmpty";
        public static string UserNameIsNotValidated => "messages.error.user.userNameIsNotValidated";
        public static string InvalidUserNameOrPassword => "messages.error.user.invalidUserNameOrPassword";
        public static string SecurityKeyExpiryDateAlreadyExpired => "messages.error.user.securityKeyExpiryDateAlreadyExpired";


        public static string NotFoundMentor => "messages.error.mentor.notFoundMentor";
        public static string FailedToAddMentor => "messages.error.mentor.failedToAddMentor";
        public static string MentorAlreadyRegistered => "messages.error.mentor.mentorAlreadyRegistered";
        public static string MentorAlreadyHasTheMaxMenteeCount => "messages.error.mentor.mentorAlreadyHasTheMaxMenteeCount";


        public static string NotFoundMentee => "messages.error.mentee.notFoundMentee";
        public static string FailedToAddMentee => "messages.error.mentee.failedToAddMentee";
        public static string MenteeAlreadyRegistered => "messages.error.mentee.menteeAlreadyRegistered";
        public static string MenteeCanNotBeSelfMentor => "messages.error.mentee.menteeCanNotBeSelfMentor";
        public static string MenteeAlreadyHasTheMaxMentorCount => "messages.error.mentee.menteeAlreadyHasTheMaxMentorCount";


        public static string NotFoundMentorMenteePair => "messages.error.pair.notFoundMentorMenteePair";
        public static string ApplicationAlreadyApproved => "messages.error.pair.applicationAlreadyApproved";
        public static string MentorMenteePairAlreadyExist => "messages.error.pair.mentorMenteePairAlreadyExist";
        public static string ApplicationNotFoundWhenWaitingStatus => "messages.error.pair.notFoundAplicationWhenWaitingStatus";

    }
}
