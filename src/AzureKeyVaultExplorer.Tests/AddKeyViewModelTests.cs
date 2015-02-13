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
                return new Key("https://test.vault.azure.net/keys/TestKey/");
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
        public void AddKeyViewModelCreateInstanceTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName);
            Assert.IsTrue(viewModel.AddKeyCommand != null);
            Assert.IsTrue(viewModel.CancelAddKeyCommand != null);
            Assert.IsFalse(viewModel.AddKeyCommand.CanExecute(null));
            Assert.IsTrue(viewModel.CancelAddKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteAddKeyCommandWithOnlyKeyNameTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName);
            Assert.IsFalse(viewModel.AddKeyCommand.CanExecute(null));
            Assert.IsTrue(viewModel.CancelAddKeyCommand.CanExecute(null));

            viewModel.KeyName = "Keyname";
            Assert.IsTrue(viewModel.AddKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteAddKeyCommandWithAllInputTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName);
            Assert.IsFalse(viewModel.AddKeyCommand.CanExecute(null));
            Assert.IsTrue(viewModel.CancelAddKeyCommand.CanExecute(null));

            viewModel.KeyName = "Keyname";
            viewModel.KeyVersion = "0f653b06c1d94159bc7090596bbf7784";
            Assert.IsTrue(viewModel.AddKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteAddKeyCommandWithOnlyKeyVersionTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName);
            Assert.IsFalse(viewModel.AddKeyCommand.CanExecute(null));
            Assert.IsTrue(viewModel.CancelAddKeyCommand.CanExecute(null));

            viewModel.KeyVersion = "12344";
            Assert.IsFalse(viewModel.AddKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddKeyCommandExecuteTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();
            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName);
            viewModel.KeyName = "TestKey";
            viewModel.RequestClose += (sender, args) => Assert.AreEqual(sender, viewModel);
            viewModel.AddKeyCommand.Execute(null);
            keyRepository.Verify(a => a.Add(It.IsAny<Key>()), Times.Once);
        }

        [TestMethod]
        public void CancelKeyCommandExecuteTest()
        {
            this.Initialize();
            var keyRepository = new Mock<IKeyRepository>();

            var key = KeyWithoutIdentifier;
            var viewModel = new AddKeyViewModel(keyRepository.Object, KeyVaultName);
            viewModel.RequestClose += (sender, args) => Assert.AreEqual(sender, viewModel);
            viewModel.CancelAddKeyCommand.Execute(null);
        }
    }
}