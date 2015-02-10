namespace AzureKeyVaultExplorer.Interface
{
    public interface IDataConverter
    {
        string DisplayMessage { get; }

        string ConvertToString(byte[] data);

        byte[] ConvertToByteArray(string data);
    }
}