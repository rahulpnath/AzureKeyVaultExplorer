namespace AzureKeyVaultExplorer.Tests
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class KeyVaultConfigurationRepositoryTests
    {
        private const string ConfigurationsPath = "Configurations";

        private static KeyVaultConfiguration MockKeyVaultConfiguration
        {
            get
            {
                return new KeyVaultConfiguration()
                {
                    ADApplicationClientId = "1",
                    ADApplicationSecret = "Secret",
                    VaultName = "test",
                    AzureKeyVaultUrl = @"https://test.vault.azure.net/"
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
        public void DirectoryExistsAfterRepositoryInitializedTest()
        {
            IKeyVaultConfigurationRepository repository = new KeyVaultConfigurationRepository();
            Assert.IsTrue(Directory.Exists(ConfigurationsPath));
        }

        [TestMethod]
        public async Task AddNewConfigurationTest()
        {
            IKeyVaultConfigurationRepository repository = new KeyVaultConfigurationRepository();
            var isInserted = await repository.InsertOrUpdate(MockKeyVaultConfiguration);
            Assert.IsTrue(isInserted);
            Assert.IsTrue(repository.All.Count() == 1);
        }

        [TestMethod]
        public async Task UpdateExistingConfigurationTest()
        {
            this.Initialize();
            var keyVaultConfiguration = new KeyVaultConfiguration()
                {
                    ADApplicationClientId = "1",
                    ADApplicationSecret = "Secret",
                    VaultName = "test",
                    AzureKeyVaultUrl = @"https://test.vault.azure.net/"
                };

            IKeyVaultConfigurationRepository repository = new KeyVaultConfigurationRepository();
            var isInserted = await repository.InsertOrUpdate(keyVaultConfiguration);
            Assert.IsTrue(isInserted);
            Assert.IsTrue(repository.All.Count() == 1);

            keyVaultConfiguration.ADApplicationSecret = "Updated Secret";
            await repository.InsertOrUpdate(keyVaultConfiguration);
            
            IKeyVaultConfigurationRepository newRepository = new KeyVaultConfigurationRepository();
            Assert.IsTrue(newRepository.All.Count() == 1);
            Assert.IsTrue(newRepository.All.First().ADApplicationSecret == "Updated Secret");
        }

        [TestMethod]
        public async Task DeleteExistingConfigurationTest()
        {
            this.Initialize();
            IKeyVaultConfigurationRepository repository = new KeyVaultConfigurationRepository();
            await repository.InsertOrUpdate(MockKeyVaultConfiguration);
            Assert.IsTrue(repository.All.Count() == 1);

            repository.Delete(MockKeyVaultConfiguration);
            Assert.IsTrue(!repository.All.Any());
        }
    }
}