namespace AzureKeyVaultExplorer.Interface
{
    using System.Threading.Tasks;

    using AzureKeyVaultExplorer.Model;

    public interface IKeyOperations
    {
        Task<string> Encrypt(Key key, string dataToEncrypt, IDataConverter dataConverter);

        Task<string> Decrypt(Key key, string dataToDecrypt, IDataConverter dataConverter);
    }
}