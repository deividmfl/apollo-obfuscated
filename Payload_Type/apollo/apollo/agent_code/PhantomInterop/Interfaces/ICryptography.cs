namespace PhantomInterop.Interfaces
{
    public interface GhostHandlerA290
    {
        string Encrypt(string plaintext);
        string Decrypt(string encrypted);
        bool UpdateUUID(string uuid);
        bool UpdateKey(string key);

        string GetUUID();
    }
}
