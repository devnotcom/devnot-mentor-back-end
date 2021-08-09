namespace DevnotMentor.Api.Common.Response
{
    public enum ResponseStatus : int
    {
        Ok = 200,
        Created = 201,
        NoContent = 204,
        BadRequest = 400,
        UnAuthorized = 401,
        Forbid = 403,
        NotFound = 404,
        Internal = 500
    }
}
