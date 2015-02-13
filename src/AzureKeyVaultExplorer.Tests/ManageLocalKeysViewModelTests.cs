namespace AzureKeyVaultExplorer.Tests
{
    using AzureKeyVaultExplorer.Interface;
    using AzureKeyVaultExplorer.Model;
    using AzureKeyVaultExplorer.ViewModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class ManageLocalKeysViewModelTests
    {
        private const string VaultName = "TestVault";

        private static Key KeyWithIdentifier
        {
            get
            {
                return new Key("https://test.vault.azure.net/keys/TestKeyAlternate/09d4ghadf45d423e9c294685a8aa562f");
            }
        }

        [TestMethod]
        public void CreateInstanceTest()
        {
            var viewModel = new ManageLocalKeysViewModel(VaultName, new Mock<IKeyRepository>().Object);
            Assert.IsNotNull(viewModel.AddKeyCommand);
            Assert.IsNull(viewModel.AddKeyViewModel);
            Assert.IsNotNull(viewModel.DeleteKeyCommand);
            Assert.IsTrue(viewModel.AddKeyCommand.CanExecute(null));
            Assert.IsFalse(viewModel.DeleteKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanDeleteKeyCommandTest()
        {
            var viewModel = new ManageLocalKeysViewModel(VaultName, new Mock<IKeyRepository>().Object);
            viewModel.SetSelectedKey(KeyWithIdentifier);
            Assert.IsTrue(viewModel.DeleteKeyCommand.CanExecute(null));
        }

        [TestMethod]
        public void DeleteKeyCommandTest()
        {
            var keyRepository = new Mock<IKeyRepository>();
            var key = KeyWithIdentifier;
            var viewModel = new ManageLocalKeysViewModel(VaultName, keyRepository.Object);
            viewModel.KeysModified += (sender, args) => Assert.IsInstanceOfType(sender, typeof(ManageLocalKeysViewModel));
            viewModel.SetSelectedKey(key);
            viewModel.DeleteKeyCommand.Execute(null);
            keyRepository.Verify(a => a.Delete(key), Times.Once);
        }
    }
}