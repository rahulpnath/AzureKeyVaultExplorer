namespace AzureKeyVaultExplorer.Model
{
    using System.IO;

    using AzureKeyVaultExplorer.Interface;

    public class KeyVaultConfigurationRepository : IKeyVaultConfigurationRepository
    {
        public KeyVaultConfigurationRepository()
        {
        }

        public KeyVaultConfiguration Get(string keyVaultUrl)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<KeyVaultConfiguration> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public bool Insert(KeyVaultConfiguration keyVaultConfiguration)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(KeyVaultConfiguration keyVaultConfiguration)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(KeyVaultConfiguration keyVaultConfiguration)
        {
            throw new System.NotImplementedException();
        }
    }
}