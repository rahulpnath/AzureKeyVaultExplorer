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

        [TestMethod]
        public async Task GetAllTest()
        {
            var keyvaultRepository = new KeyVaultKeyRepository(MockKeyVaultConfiguration);
            var keys = await keyvaultRepository.GetAll();
            Assert.IsNotNull(keys);
        }
    }
}