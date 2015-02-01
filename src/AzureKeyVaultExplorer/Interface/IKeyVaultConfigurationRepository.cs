namespace AzureKeyVaultExplorer.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AzureKeyVaultExplorer.Model;

    public interface IKeyVaultConfigurationRepository
    {
        IEnumerable<KeyVaultConfiguration> All { get; }

        KeyVaultConfiguration Get(string keyVaultUrl);

        Task<bool> InsertOrUpdate(KeyVaultConfiguration keyVaultConfiguration);

        bool Delete(KeyVaultConfiguration keyVaultConfiguration);
    }
}