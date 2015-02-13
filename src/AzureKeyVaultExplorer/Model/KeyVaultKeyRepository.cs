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

        public Task<bool> Add(Key key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Key key)
        {
            throw new NotImplementedException();
        }

        private Key MapVaultKeyToLocalKey(KeyItem vaultKey)
        {
            // TODO Refactor this to use common logic in view model too
            const string KeyIdentifierFormat = @"https://([www]?)(?<vaultname>[a-zA-Z0-9-]{3,24}).vault.azure.net/keys/(?<keyname>[a-zA-Z0-9-]{1,63})[/]?(?<keyversion>[^/]*)[/]?";
            var regex = new Regex(KeyIdentifierFormat);
            var matches = regex.Match(vaultKey.Kid);
            var keyName = matches.Groups["keyname"].Value;
            return new Key { KeyIdentifier = vaultKey.Kid, Name = keyName };
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