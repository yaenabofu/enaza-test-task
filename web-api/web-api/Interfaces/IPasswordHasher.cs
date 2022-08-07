namespace web_api.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
