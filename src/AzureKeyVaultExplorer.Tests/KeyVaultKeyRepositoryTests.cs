namespace AzureKeyVaultExplorer.Tests
{
    using System.Threading.Tasks;

    using AzureKeyVaultExplorer.Model;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class KeyVaultKeyRepositoryTests
    {
        private static KeyVaultConfiguration MockKeyVaultConfiguration
        {
            get
            {
                return new KeyVaultConfiguration()
                {
                    ADApplicationClientId = "6aefaf53-b8bb-47ae-b39e-484ec6b9ca01",
                    ADApplicationSecret = "ddgd0lEBgtGcCblxYRqLzOeFhKIdhPzsSBLtxHdVBMY=",
                    VaultName = "testvaultrahul",
                    AzureKeyVaultUrl = @"https://testvaultrahul.vault.azure.net"
                };
            }
        }

        private static Key Key
        {
            get
            {
                return new Key
                {
                    KeyIdentifier =
                        "https://testvaultrahul.vault.azure.net/keys/rahulkey/0f653b06c1d94159bc7090596bbf7784",
                    Name = "rahulkey"
                };
            }
        }

        [TestMethod]
        public async Task GetAllTest()
        {
            var keyvaultRepository = new KeyVaultKeyRepository(MockKeyVaultConfiguration);
            var keys = await keyvaultRepository.GetAll();
            Assert.IsNotNull(keys);
        }

        [TestMethod]
        public async Task AddTest()
        {
            var keyvaultRepository = new KeyVaultKeyRepository(MockKeyVaultConfiguration);
            await keyvaultRepository.Add(Key);
        }

        [TestMethod]
        public async Task DeleteTest()
        {
            var keyvaultRepository = new KeyVaultKeyRepository(MockKeyVaultConfiguration);
            await keyvaultRepository.Delete(Key);
        }
    }
}