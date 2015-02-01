namespace AzureKeyVaultExplorer.Tests
{
    using System.Threading.Tasks;

    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class AddKeyVaultAccountViewModelTests
    {
        private static KeyVaultConfiguration MockKeyVaultConfiguration
        {
            get
            {
                var mockKeyVaultConfiguration =
                    Mock.Of<KeyVaultConfiguration>(
                        c =>
                            c.ADApplicationClientId == "1" && c.ADApplicationSecret == "Secret" && c.VaultName == "test"
                            && c.AzureKeyVaultUrl == @"https://test.vault.azure.net/");
                return mockKeyVaultConfiguration;
            }
        }

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void CreateInstanceTest()
        {
            var mockKeyVaultConfiguration = MockKeyVaultConfiguration;

            var addKeyVaultAccountViewModel =
                new AddKeyVaultAccountViewModel(
                    new Mock<IKeyVaultConfigurationRepository>().Object,
                    mockKeyVaultConfiguration);

            Assert.AreEqual(addKeyVaultAccountViewModel.ADApplicationId, mockKeyVaultConfiguration.ADApplicationClientId);
            Assert.AreEqual(addKeyVaultAccountViewModel.KeyVaultUrl, mockKeyVaultConfiguration.AzureKeyVaultUrl);
            Assert.AreEqual(addKeyVaultAccountViewModel.ADApplicationSecret, mockKeyVaultConfiguration.ADApplicationSecret);
        }

        [TestMethod]
        public void CanAddKeyVaultCommandForEmptyConfigurationTest()
        {
            var mockKeyVaultConfiguration = new Mock<KeyVaultConfiguration>();
            var addKeyVaultAccountViewModel =
                new AddKeyVaultAccountViewModel(
                    new Mock<IKeyVaultConfigurationRepository>().Object,
                    mockKeyVaultConfiguration.Object);

            var canExecute = addKeyVaultAccountViewModel.AddKeyVaultAccountCommand.CanExecute(null);

            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void CanAddKeyVaultCommandForInvalidConfigurationUrlTest()
        {
            var mockKeyVaultConfiguration = new Mock<KeyVaultConfiguration>();
            mockKeyVaultConfiguration.SetupAllProperties();
            mockKeyVaultConfiguration.Object.ADApplicationClientId = "1";
            mockKeyVaultConfiguration.Object.AzureKeyVaultUrl = "https://invalidurl.test";
            mockKeyVaultConfiguration.Object.ADApplicationSecret = "secret";

            var addKeyVaultAccountViewModel =
                new AddKeyVaultAccountViewModel(
                    new Mock<IKeyVaultConfigurationRepository>().Object,
                    mockKeyVaultConfiguration.Object);

            var canExecute = addKeyVaultAccountViewModel.AddKeyVaultAccountCommand.CanExecute(null);

            Assert.IsFalse(canExecute);

            addKeyVaultAccountViewModel.KeyVaultUrl = @"https://.test.vault.azure.net/";

            canExecute = addKeyVaultAccountViewModel.AddKeyVaultAccountCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void CanAddKeyVaultCommandForValidConfigrationTest()
        {
            var mockKeyVaultConfiguration = MockKeyVaultConfiguration;
            var addKeyVaultAccountViewModel =
                new AddKeyVaultAccountViewModel(
                    new Mock<IKeyVaultConfigurationRepository>().Object,
                    mockKeyVaultConfiguration);
            var canExecute = addKeyVaultAccountViewModel.AddKeyVaultAccountCommand.CanExecute(null);
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void CanAddKeyVaultCommandForValidConfigrationWithWwwInUrlTest()
        {
            var mockKeyVaultConfiguration = MockKeyVaultConfiguration;
            mockKeyVaultConfiguration.AzureKeyVaultUrl = @"https://www.test.vault.azure.net/";
            var addKeyVaultAccountViewModel =
                new AddKeyVaultAccountViewModel(
                    new Mock<IKeyVaultConfigurationRepository>().Object,
                    mockKeyVaultConfiguration);
            var canExecute = addKeyVaultAccountViewModel.AddKeyVaultAccountCommand.CanExecute(null);
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void CanAddKeyVaultCommandForValidConfigrationWithNoTrailingSlashInUrlTest()
        {
            var mockKeyVaultConfiguration = MockKeyVaultConfiguration;
            mockKeyVaultConfiguration.AzureKeyVaultUrl = @"https://www.test.vault.azure.net";
            var addKeyVaultAccountViewModel =
                new AddKeyVaultAccountViewModel(
                    new Mock<IKeyVaultConfigurationRepository>().Object,
                    mockKeyVaultConfiguration);
            var canExecute = addKeyVaultAccountViewModel.AddKeyVaultAccountCommand.CanExecute(null);
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void CanAddKeyVaultCommandForExistingConfigrationTest()
        {
            var mockKeyVaultConfiguration = MockKeyVaultConfiguration;
            var keyVaultConfigurationRepository = new Mock<IKeyVaultConfigurationRepository>();
            keyVaultConfigurationRepository.Setup(
                m => m.Get(mockKeyVaultConfiguration.AzureKeyVaultUrl)).Returns(mockKeyVaultConfiguration);

            var addKeyVaultAccountViewModel =
                new AddKeyVaultAccountViewModel(
                    keyVaultConfigurationRepository.Object,
                    mockKeyVaultConfiguration);
            var canExecute = addKeyVaultAccountViewModel.AddKeyVaultAccountCommand.CanExecute(null);
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void AddKeyVaultCommandConfigurationTest()
        {
            var mockKeyVaultConfiguration = MockKeyVaultConfiguration;
            var keyVaultConfigurationRepository = new Mock<IKeyVaultConfigurationRepository>();
            keyVaultConfigurationRepository.Setup(
                a => a.InsertOrUpdate(mockKeyVaultConfiguration)).Returns(Task.FromResult(true));

            var addKeyVaultAccountViewModel =
                new AddKeyVaultAccountViewModel(
                    keyVaultConfigurationRepository.Object,
                    mockKeyVaultConfiguration);

            addKeyVaultAccountViewModel.AddKeyVaultAccountCommand.Execute(null);

            keyVaultConfigurationRepository.Verify(a => a.InsertOrUpdate(mockKeyVaultConfiguration), Times.Once);
        }
    }
}