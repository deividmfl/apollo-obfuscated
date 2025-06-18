namespace PhantomInterop.Interfaces
{
    public interface StrategicTransformer755F
    {
        bool TryAddOrUpdate(string keyName, byte[] data);

        bool TryGetValue(string keyName, out byte[] data);

        string GetScript();
        void SetScript(string script);
        void SetScript(byte[] script);
    }
}
