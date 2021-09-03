namespace DevnotMentor.Business.Utilities.Security.Hash
{
    public interface IHashService
    {
        string CreateHash(string plainText);
        bool CompareHash(string hashedText, string plainText);
    }
}
