namespace AzureKeyVaultExplorer.Tests
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AzureKeyVaultExplorer.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class KeyRepositoryTests
    {
        private const string ConfigurationsPath = "Configurations";

        private const string KeyPath = @"Configurations\{0}";

        private const string VaultName = "testVault";

        private static Key KeyWithIdentifier
        {
            get
            {
                return new Key
                       {
                           KeyIdentifier =
                               "https://test.vault.azure.net/keys/TestKeyAlternate/09d4ghadf45d423e9c294685a8aa562f",
                           Name = "TestKeyAlternate"
                       };
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            if (Directory.Exists(ConfigurationsPath))
            {
                Directory.Delete(ConfigurationsPath, true);
            }
        }

        [TestMethod]
        public void DirectoryExistsAfterKeyRepositoryCreated()
        {
            var keyRepository = new KeyRepository(VaultName);
            Assert.IsTrue(Directory.Exists(string.Format(KeyPath, VaultName)));
        }

        [TestMethod]
        public async Task AddKeyTest()
        {
            var keyRepository = new KeyRepository(VaultName);
            var isInserted = await keyRepository.Add(KeyWithIdentifier);
            Assert.IsTrue(isInserted);
            Assert.IsTrue(keyRepository.All.Count() == 1);

            var newKeyRepository = new KeyRepository(VaultName);
            Assert.IsTrue(newKeyRepository.All.Count() == 1);
        }

        [TestMethod]
        public async Task DeleteKeyTest()
        {
            this.Initialize();

            var keyRepository = new KeyRepository(VaultName);
            var isInserted = await keyRepository.Add(KeyWithIdentifier);
            Assert.IsTrue(isInserted);
            Assert.IsTrue(keyRepository.All.Count() == 1);

            var newKeyRepository = new KeyRepository(VaultName);
            Assert.IsTrue(newKeyRepository.All.Count() == 1);
            var isDeleted = newKeyRepository.Delete(newKeyRepository.All.First());
            Assert.IsTrue(isDeleted);
            Assert.IsTrue(!newKeyRepository.All.Any());

            newKeyRepository = new KeyRepository(VaultName);
            Assert.IsTrue(!newKeyRepository.All.Any());
        }
    }
}