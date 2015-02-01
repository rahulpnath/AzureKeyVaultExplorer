namespace AzureKeyVaultExplorer.Interface
{
    using System.Collections.Generic;

    using AzureKeyVaultExplorer.Model;

    public interface IKeyVaultConfigurationRepository
    {
        KeyVaultConfiguration Get(string keyVaultUrl);

        IEnumerable<KeyVaultConfiguration> GetAll();

        bool Insert(KeyVaultConfiguration keyVaultConfiguration);

        bool Update(KeyVaultConfiguration keyVaultConfiguration);

        bool Delete(KeyVaultConfiguration keyVaultConfiguration);
    }
}