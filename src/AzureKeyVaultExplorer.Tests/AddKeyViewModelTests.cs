namespace AzureKeyVaultExplorer.Tests
{
    using System.IO;
    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AddKeyViewModelTests
    {
        private const string ConfigurationsPath = "Configurations";

        private const string KeyVaultName = "Test";

        private static Key KeyWithoutIdentifier
        {
            get
            {
                return new Key { KeyIdentifier = "https://test.vault.azure.net/keys/TestKey/", Name = "TestKey" };
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
        public void ViewModelPropertiesOnCreatedTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName, KeyWithoutIdentifier);
            Assert.IsTrue(viewModel.AddKeyCommand != null);
            Assert.IsTrue(viewModel.CancelAddKeyCommand != null);
            Assert.IsTrue(viewModel.AddKeyCommand.CanExecute(null));
            Assert.IsTrue(viewModel.CancelAddKeyCommand.CanExecute(null));
            Assert.AreEqual(viewModel.KeyIdentifier, KeyWithoutIdentifier.KeyIdentifier);
        }

        [TestMethod]
        public void CanExecuteCommandTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName, new Key());
            Assert.IsFalse(viewModel.AddKeyCommand.CanExecute(null));
            Assert.IsTrue(viewModel.CancelAddKeyCommand.CanExecute(null));

            viewModel.KeyIdentifier = "invaididentifier";
            Assert.IsFalse(viewModel.AddKeyCommand.CanExecute(null));

            viewModel.KeyIdentifier = "https://test.vault.azure.net/keys/TestKey/";

            Assert.IsTrue(viewModel.AddKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddKeyCommandExecuteTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var key = KeyWithoutIdentifier;
            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName, key);
            viewModel.RequestClose += (sender, args) => Assert.AreEqual(sender, viewModel);
            viewModel.AddKeyCommand.Execute(null);

            keyRepository.Verify(a => a.Add(key), Times.Once);
        }

        [TestMethod]
        public void CanAddKeyCommandUrlWithNoTrailingSlashAndNoVersionNumberTest()
        {
            var addKeyViewModel = new AddKeyViewModel(new Mock<IKeyRepository>().Object, KeyVaultName, new Key());
            var keyNamePrivate = new PrivateObject(addKeyViewModel);
            addKeyViewModel.KeyIdentifier = "https://test.vault.azure.net/keys/TestKey";
            Assert.IsTrue(addKeyViewModel.AddKeyCommand.CanExecute(null));
            Assert.AreEqual("TestKey", keyNamePrivate.GetField("keyName"));
        }

        [TestMethod]
        public void CanAddKeyCommandUrlWithTrailingSlashAndNoVersionNumberTest()
        {
            var addKeyViewModel = new AddKeyViewModel(new Mock<IKeyRepository>().Object, KeyVaultName, new Key());
            var keyNamePrivate = new PrivateObject(addKeyViewModel);
            addKeyViewModel.KeyIdentifier = "https://test.vault.azure.net/keys/TestKey/";
            Assert.IsTrue(addKeyViewModel.AddKeyCommand.CanExecute(null));
            Assert.AreEqual("TestKey", keyNamePrivate.GetField("keyName"));
        }

        [TestMethod]
        public void CanAddKeyCommandUrlWithNoTrailingSlashAndVersionNumberTest()
        {
            var addKeyViewModel = new AddKeyViewModel(new Mock<IKeyRepository>().Object, KeyVaultName, new Key());
            var keyNamePrivate = new PrivateObject(addKeyViewModel);
            addKeyViewModel.KeyIdentifier = "https://test.vault.azure.net/keys/TestKey/123456";
            Assert.IsTrue(addKeyViewModel.AddKeyCommand.CanExecute(null));
            Assert.AreEqual("TestKey", keyNamePrivate.GetField("keyName"));
            Assert.AreEqual("123456", keyNamePrivate.GetField("keyVersion"));
        }

        [TestMethod]
        public void CanAddKeyCommandUrlWithTrailingSlashAndVersionNumberTest()
        {
            var addKeyViewModel = new AddKeyViewModel(new Mock<IKeyRepository>().Object, KeyVaultName, new Key());
            var keyNamePrivate = new PrivateObject(addKeyViewModel);
            addKeyViewModel.KeyIdentifier = "https://test.vault.azure.net/keys/TestKey/123456";
            Assert.IsTrue(addKeyViewModel.AddKeyCommand.CanExecute(null));
            Assert.AreEqual("TestKey", keyNamePrivate.GetField("keyName"));
            Assert.AreEqual("123456", keyNamePrivate.GetField("keyVersion"));
        }

        [TestMethod]
        public void CancelKeyCommandExecuteTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var key = KeyWithoutIdentifier;
            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName, key);
            viewModel.RequestClose += (sender, args) => Assert.AreEqual(sender, viewModel);
            viewModel.CancelAddKeyCommand.Execute(null);
        }
    }
}