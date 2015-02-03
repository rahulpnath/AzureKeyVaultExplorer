namespace AzureKeyVaultExplorer.Model
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    using AzureKeyVaultExplorer.Interface;

    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.KeyVault.Client;

    public class KeyOperations : IKeyOperations
    {
        private readonly KeyVaultConfiguration keyVaultConfiguration;

        private readonly KeyVaultClient keyVaultClient;

        public KeyOperations(KeyVaultConfiguration keyVaultConfiguration)
        {
            this.keyVaultConfiguration = keyVaultConfiguration;
            this.keyVaultClient = new KeyVaultClient(this.HandleKeyVaultAuthenticationCallback);
        }

        public async Task<string> Encrypt(Key key, string dataToEncrypt)
        {
            var byteData = Convert.FromBase64String(dataToEncrypt);
            var result = await this.keyVaultClient.EncryptDataAsync(key.KeyIdentifier, "RSA_OAEP", byteData);
            return Convert.ToBase64String(result.Result);
        }

        public async Task<string> Decrypt(Key key, string dataToDecrypt)
        {
            var byteData = Convert.FromBase64String(dataToDecrypt);
            var result = await this.keyVaultClient.DecryptDataAsync(key.KeyIdentifier, "RSA_OAEP", byteData);
            return Convert.ToBase64String(result.Result);
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