namespace AzureKeyVaultExplorer.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AzureKeyVaultExplorer.Interface;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.KeyVault.Client;

    public class KeyVaultKeyRepository : IKeyRepository
    {
        private readonly KeyVaultConfiguration keyVaultConfiguration;

        private readonly KeyVaultClient keyVaultClient;

        public KeyVaultKeyRepository(KeyVaultConfiguration keyVaultConfiguration)
        {
            this.keyVaultConfiguration = keyVaultConfiguration;

            // TODO : Move this into a common provider as it is used in multiple places
            this.keyVaultClient = new KeyVaultClient(this.HandleKeyVaultAuthenticationCallback);
        }

        public async Task<IEnumerable<Key>> GetAll()
        {
            var keys = new List<Key>();
            try
            {
                var vaultKeys = await this.keyVaultClient.GetKeysAsync(this.keyVaultConfiguration.AzureKeyVaultUrl);
                keys.AddRange(vaultKeys.Select(this.MapVaultKeyToLocalKey));
            }
            catch (Exception)
            {
                throw new Exception("throw custom exception that get is not allowed");
            }

            return keys;
        }

        public Task<Key> Get(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Add(Key key)
        {
            await this.keyVaultClient.CreateKeyAsync(
               this.keyVaultConfiguration.AzureKeyVaultUrl,
               key.Name,
               key.KeyType);
            return true;
        }

        public async Task<bool> Delete(Key key)
        {
            await this.keyVaultClient.DeleteKeyAsync(this.keyVaultConfiguration.AzureKeyVaultUrl, key.Name);
            return true;
        }

        private Key MapVaultKeyToLocalKey(KeyItem vaultKey)
        {
           return new Key(vaultKey.Kid);
        }

        private string HandleKeyVaultAuthenticationCallback(string authority, string resource, string scope)
        {
            var adCredential = new ClientCredential(
                this.keyVaultConfiguration.ADApplicationClientId,
                this.keyVaultConfiguration.ADApplicationSecret);
            var authenticationContext = new AuthenticationContext(authority, null);
            return authenticationContext.AcquireToken(resource, adCredential).AccessToken;
        }
    }
}